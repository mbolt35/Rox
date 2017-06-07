using System;
using OpenGL;
using OpenGL.Platform;
using System.Diagnostics;
using Rox.Core;
using Rox.Render;
using Rox.Render.GL;
using Rox.Util;
using System.Threading;

public class RoxMain {

    /// <summary>
    /// Entry Point
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args) {
        var rox = new RoxMain(1280, 720);
        rox.Run();
    }
    
    /// <summary>
    /// Vertex shader 
    /// </summary>
    private static string vertexShader2Source = @"
        #version 330 core

        uniform mat4 ProjectionMatrix;
        uniform mat4 ModelMatrix;
        uniform mat4 ViewMatrix;

        in vec3 in_position;
        in vec3 in_normal;
        in vec2 in_uv;

        out vec3 FragPos;
        out vec3 FragNormal;
        out vec2 FragUV;

        void main(void)
        {
            vec4 pos = ModelMatrix * vec4(in_position, 1);
            vec4 norm = ModelMatrix * vec4(in_normal, 1);

            FragPos = pos.xyz;
            FragNormal = norm.xyz;
            FragUV = in_uv;
    
            gl_Position = ProjectionMatrix * ViewMatrix * pos;
        }";

    /// <summary>
    /// Fragment Shader
    /// </summary>
    public static string fragmentShader2Source = @"
        #version 330 core

        struct Material {
            vec3 Color;
        };

        struct Light {
            vec3 Position;
  
            vec3 Ambient;
            vec3 Diffuse;
            vec3 Specular;
	
            float Constant;
            float Linear;
            float Quadratic;
        };

        uniform Material material;
        uniform Light light;

        uniform sampler2D tex;

        in vec3 FragPosition;
        in vec3 FragNormal;
        in vec2 FragUV;

        out vec4 FragColor;

        float attenuation(in Light l, in float distance) {
            return 1.0f / (l.Constant + l.Linear * distance + l.Quadratic * (distance * distance));
        }

        void main(void)
        {
            vec3 lightDelta = light.Position - FragPosition;

            vec3 n = normalize(FragNormal);
            vec3 l = normalize(lightDelta);

            float cosTheta = clamp(dot(n, l), 0.0f, 1.0f);
            float lightAttenuation = attenuation(light, length(lightDelta));

            vec3 ambient = light.Ambient * lightAttenuation;
            vec3 diffuse = light.Diffuse * lightAttenuation * cosTheta;

            vec3 fragColor = (ambient + diffuse) * texture(tex, FragUV).rgb;
            //vec3 fragColor = (ambient + diffuse) * material.Color;
            //vec3 fragColor = texture(tex, FragUV).rgb;
            FragColor = vec4(fragColor, 1);
        }";

    // Move Increment
    private static readonly float Inc = 0.1f;

    // -- Scene Stuff --
    private IRenderer _renderer;
    private Viewport _viewport;
    private Camera _mainCamera;

    private IRenderable _cube;
    private ShaderProgram _program;

    // Instance properties
    private bool _upDown = false;
    private bool _downDown = false;
    private bool _leftDown = false;
    private bool _rightDown = false;
    private bool _forwardDown = false;
    private bool _backDown = false;

    // Frame throttling
    private Stopwatch _stopwatch = Stopwatch.StartNew();
    private long _frameTime = 16;
    private long _lastUpdate = 0;
    private float _rotation = 0.0f;

    // Texture
    private Texture _blockTexture;

    /// <summary>
    /// Rox Main Entry
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public RoxMain(int width, int height) {
        _viewport = new Viewport(width, height);

        Init();
    }

    /// <summary>
    /// Initialize GL Window, Shader, and Cube VAO
    /// </summary>
    private void Init() {
        CreateWindow();
        InitInput();

        _renderer = new OpenGLRenderer();
        Console.WriteLine(_renderer.Version);

        _blockTexture = new Texture("resources/terrain.png");

        _cube = NewCubeRenderable();

        _mainCamera = new Camera("MainCamera", _viewport);
        _mainCamera.MoveTo(2, 5, -10);
        _mainCamera.LookAt(_cube.Model, Vector3.UnitY);
    }
    
    private void CreateWindow() {
        Window.CreateWindow("OpenGL", _viewport.Width, _viewport.Height);

        Window.OnReshapeCallbacks.Add(() => {
            _viewport = new Viewport(Window.Width, Window.Height);
            _mainCamera.Viewport = _viewport;
        });
    }

    private void InitInput() {
        Input.Subscribe('w', new Event((bool state) => _forwardDown = state));
        Input.Subscribe('a', new Event((bool state) => _leftDown = state));
        Input.Subscribe('s', new Event((bool state) => _backDown = state));
        Input.Subscribe('d', new Event((bool state) => _rightDown = state));
        Input.Subscribe('q', new Event((bool state) => _upDown = state));
        Input.Subscribe('e', new Event((bool state) => _downDown = state));
        
        Window.OnMouseCallbacks.Add((b, s, x, y) => {
            Console.WriteLine($"button: {b}, state: {s}, x: {x}, y: {y}");
            return true;
        });
    }

    private void SwapBuffers() {
        Window.SwapBuffers();
    }
    
    public void Run() {
        // handle events and render the frame
        while (Window.Open) {
            Window.HandleEvents();
            if (!IsElapsedFrame()) {
                continue;
            }

            UpdateScene();

            _renderer.Clear();
            _blockTexture.Use(0);
            _renderer.Render(_mainCamera, _cube);

            SwapBuffers();
        }

        DisposeScene();
    }

    public bool IsElapsedFrame() {
        var currentTime = _stopwatch.ElapsedMilliseconds;
        if (currentTime - _lastUpdate < _frameTime) {
            return false;
        }

        _lastUpdate = currentTime;
        return true;
    }

    private void UpdateScene() {
        var forwardDir = _mainCamera.Forward.Normalize();
        var upDir = _mainCamera.Up.Normalize();
        var rightDir = forwardDir.Cross(upDir).Normalize();

        float forwardAdjust = _forwardDown ? Inc : 0.0f;
        forwardAdjust += _backDown ? -Inc : 0.0f;

        float rightAdjust = _rightDown ? Inc : 0.0f;
        rightAdjust += _leftDown ? -Inc : 0.0f;

        float upAdjust = _upDown ? Inc : 0.0f;
        upAdjust += _downDown ? -Inc : 0.0f;

        var moveAmount = (forwardAdjust * forwardDir)
            + (rightAdjust * rightDir)
            + (upAdjust * upDir);

        _mainCamera.Move(moveAmount);

        _rotation += (1.0f * RoxMath.ToRadians);

        _cube.Model.RotateY(_rotation);
        _cube.Model.RotateZ(_rotation / 2.0f);
    }
    
    private void DisposeScene() {
        _blockTexture.Dispose();
        _program.Dispose();
        _cube.Dispose();
    }

    /// <summary>
    /// Creates a new renderer instance
    /// </summary>
    /// <returns></returns>
    private IRenderer NewRenderer() {
        return new OpenGLRenderer();
    }

    /// <summary>
    /// Creates a new cube renderable
    /// </summary>
    /// <returns></returns>
    private IRenderable NewCubeRenderable() {
        var cubeObject = new RoxObject(new Vector3(0, 0, 0));
        var cubeGeometry = NewCubeWithLightShader();

        return new OpenGLRenderable(cubeObject, cubeGeometry);
    }

    /// <summary>
    /// Creates a new cube geometry (VAO) with a light shader.
    /// </summary>
    /// <returns></returns>
    private VAO NewCubeWithLightShader() {
        _program = NewLightShader();

        /*
        var cubeVao = Geometry.CreateCubeWithNormals(
            _program,
            new Vector3(-1, -1, -1),
            new Vector3(1, 1, 1)); 
        */

        float uvWidth = 2048.0f / 16.0f;
        float uvCell = uvWidth / 2048.0f;

        var x = uvCell * 3.0f;
        var y = uvCell * 15.0f;

        var cubeVao = CreateQuad(
            _program, 
            new Vector2(-1, -1), 
            new Vector2(2, 2), 
            new Vector2(x, y), 
            new Vector2(uvCell, uvCell));

        cubeVao.DisposeChildren = true;
        return cubeVao;
    }

    /// <summary>
    /// Creates and sets default values for our light shader.
    /// </summary>
    /// <returns></returns>
    private ShaderProgram NewLightShader() {
        var shader = new ShaderProgram(vertexShader2Source, fragmentShader2Source);
        Console.WriteLine("Vertex Shader: {0}", shader.VertexShader.ShaderLog);
        Console.WriteLine("Fragment Shader: {0}", shader.FragmentShader.ShaderLog);

        shader["light.Position"].SetValue(new Vector3(5, 5, -2));
        shader["light.Ambient"].SetValue(new Vector3(0.3f, 0.3f, 0.3f));
        shader["light.Diffuse"].SetValue(Vector3.One);
        shader["light.Specular"].SetValue(Vector3.One);
        shader["light.Constant"].SetValue(1.0f);
        shader["light.Linear"].SetValue(0.045f);
        shader["light.Quadratic"].SetValue(0.0075f);

        shader["material.Color"].SetValue(new Vector3(0.3f, 0.3f, 1.0f));
        shader["tex"].SetValue(0);

        return shader;
    }

    public static VAO CreateQuad(ShaderProgram program, Vector2 location, Vector2 size, Vector2 uvloc, Vector2 uvsize) {
        Vector3[] vertices = new Vector3[] {
            new Vector3(location.X, location.Y, 0),
            new Vector3(location.X + size.X, location.Y, 0),
            new Vector3(location.X + size.X, location.Y + size.Y, 0),
            new Vector3(location.X, location.Y + size.Y, 0)
        };

        Vector2[] uvs = new Vector2[] {
            uvloc,
            new Vector2(uvloc.X + uvsize.X, uvloc.Y),
            new Vector2(uvloc.X + uvsize.X, uvloc.Y + uvsize.Y),
            new Vector2(uvloc.X, uvloc.Y + uvsize.Y)
        };

        Vector3[] normals = new Vector3[] {
            Vector3.UnitZ,
            Vector3.UnitZ,
            Vector3.UnitZ,
            Vector3.UnitZ
        };

        int[] indices = new int[] { 0, 1, 2, 2, 3, 0 };

        return new VAO(program, 
            new VBO<Vector3>(vertices), 
            new VBO<Vector3>(normals), 
            new VBO<Vector2>(uvs), 
            new VBO<int>(indices, BufferTarget.ElementArrayBuffer, 
            BufferUsageHint.StaticRead));
    }
}

// 1. Create Window
//   - Windows APIs to CreateWindow
//   - Create OpenGL Context
//   - Attach OpenGL Context to Window
// 2. Run Loop
//   - Checks for Window existance 
//   - Handles incoming Input Events
//   - Renders Scene, Flip Buffers
// 3. On Window Exit
//   - Destroy Window
//   - Destroy OpenGL Context