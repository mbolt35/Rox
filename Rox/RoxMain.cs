using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using OpenGL.Platform;
using System.Diagnostics;
using Rox.Core;
using Rox.Render;

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

        uniform mat4 projection_matrix;
        uniform mat4 model_matrix;
        uniform mat4 view_matrix;

        in vec3 in_position;
        in vec3 in_normal;

        out vec3 aPos;
        out vec3 aNormal;

        void main(void)
        {
            vec4 pos = model_matrix * vec4(in_position, 1);
            vec4 norm = model_matrix * vec4(in_normal, 1);

            aPos = pos.xyz;
            aNormal = norm.xyz;
    
            gl_Position = projection_matrix * view_matrix * pos;
        }";

    /// <summary>
    /// Fragment Shader
    /// </summary>
    public static string fragmentShader2Source = @"
        #version 330 core
        uniform vec3 color;
        uniform vec3 lightPosition;

        in vec3 aPos;
        in vec3 aNormal;

        void main(void)
        {
            vec3 lightColor = vec3(1, 1, 1);
            float ambientIntensity = 0.1f;
            vec3 ambient = ambientIntensity * lightColor;

            vec3 n = normalize(aNormal.xyz);
            vec3 l = normalize(lightPosition - aPos.xyz);

            float cosTheta = clamp(dot(n, l), 0.0f, 1.0f);
            vec3 diffuse = cosTheta * lightColor;

            vec3 fragColor = (ambient + diffuse) * color;
            gl_FragColor = vec4(fragColor, 1);
        }";

    // -- Scene Stuff --

    private IRoxObject _cube;
    private Viewport _viewport;
    private Camera _mainCamera;

    // -- Scene Stuff -- 

    private float _rotation = 0.0f;
    private Vector3 _rotationAxis = new Vector3(0, -1, 0);
    private Vector3 _lightPosition;

    private long _frameTime = 16;
    private long _lastUpdate = 0;
    private bool _forward = true;

    private Stopwatch _stopwatch = Stopwatch.StartNew();

    // Shader Program
    private ShaderProgram _shader;

    // Cube Vertex Array Object (VAO)
    private VAO _cubeVao;
        
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
        OpenGLVersions();

        // --- Initialization/Window Options ---
        Gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        Gl.ClearDepthf(1.0f);

        Gl.Enable(EnableCap.Multisample);
        Gl.Enable(EnableCap.DepthTest);
        Gl.DepthFunc(DepthFunction.Lequal);

        Gl.Enable(EnableCap.CullFace);
        Gl.CullFace(CullFaceMode.Back);

        Gl.Enable(EnableCap.Blend);
        Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        // --- Initialization/Window Options ---

        // -- Scene --
        _cube = new RoxObject(new Vector3(0, 0, 0));
        _lightPosition = new Vector3(10, 15, -20);
        
        // -- Scene --

        // --- Camera/Renderer ---
        _shader = new ShaderProgram(vertexShader2Source, fragmentShader2Source);

        _mainCamera = new Camera("MainCamera", _viewport);
        _mainCamera.MoveTo(2, 5, -10);
        _mainCamera.LookAt(_cube, Vector3.UnitY);
        
        _shader["projection_matrix"].SetValue(_mainCamera.Projection);
        _shader["view_matrix"].SetValue(_mainCamera.Transform);
        _shader["lightPosition"].SetValue(_lightPosition);

        // set the color to blue
        _shader["color"].SetValue(new Vector3(1, 0, 1));
        // --- Camera/Renderer ---

        _cubeVao = Geometry.CreateCubeWithNormals(_shader, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        _cubeVao.DisposeChildren = true;
        _cubeVao.DisposeElementArray = true;
    }
    

    public void Run() {
        // handle events and render the frame
        while (Window.Open) {
            Window.HandleEvents();
            if (!IsElapsedFrame()) {
                continue;
            }

            UpdateScene();

            _shader["view_matrix"].SetValue(_mainCamera.Transform);
            _shader["model_matrix"].SetValue(_cube.Transform);

            OnRenderFrame();
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
        _rotation += 0.01f;

        if (_forward) {
            _mainCamera.Move(0.0f, 0.02f, 0.0f);
        }
        else {
            _mainCamera.Move(0.0f, -0.02f, 0.0f);
        }

        if (_forward && _mainCamera.Position.Y > 2.0f) {
            _forward = false;
        }
        else if (!_forward && _mainCamera.Position.Y < -2.0f) {
            _forward = true;
        }

        _cube.Rotate(_rotationAxis, _rotation);
        _mainCamera.LookAt(_cube, Vector3.UnitY);
    }

    private void CreateWindow() {
        Window.CreateWindow("OpenGL", _viewport.Width, _viewport.Height);
    }

    private void DisposeScene() {
        _shader.Dispose();
        _cubeVao.Dispose();
    }

    private void OnRenderFrame() {
        UpdateViewport();
        Clear();

        _cubeVao.Program.Use();
        _cubeVao.Draw();

        Window.SwapBuffers();
    }

    public void Clear() {
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void UpdateViewport() {
        Gl.Viewport(_viewport.X, _viewport.Y, _viewport.Width, _viewport.Height);
    }

    private static void OpenGLVersions()
    {
        Console.WriteLine("OpenGL Version: {0}", Gl.GetString(OpenGL.StringName.Version));
        Console.WriteLine("GLSL Version: {0}", Gl.GetString(StringName.ShadingLanguageVersion));
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