using OpenGL;
using OpenGL.Platform;
using System.Diagnostics;
using Rox.Core;
using Rox.Render;
using Rox.Render.GL;
using Rox.Util;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenGL.UI;
using Rox.Voxel;
using Rox.Geom;
using Rox.Terrain;
using Rox.Terrain.Noise;
using Rox.Voxel.Render;
using Console = System.Console;

public class RoxMain {

    /// <summary>
    /// Entry Point
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args) {
        var rox = new RoxMain(1280, 720);
        rox.Run();
        //Scratch();
    }

    private static void Scratch() {
        var _radius = 8;
        var min = new Vector3(
            (Chunk.Width * _radius) / 2.0f,
            (Chunk.Height * _radius) / 2.0f,
            (Chunk.Depth * _radius) / 2.0f);

        var max = min + new Vector3(Chunk.Width, Chunk.Height, Chunk.Depth);

        min.X = (int) min.X / Chunk.Width;
        min.Y = (int) min.Y / Chunk.Height;
        min.Z = (int) min.Z / Chunk.Depth;

        max.X = (int)max.X / Chunk.Width;
        max.Y = (int)max.Y / Chunk.Height;
        max.Z = (int)max.Z / Chunk.Depth;

        Console.WriteLine($"Min: {min}\nMax: {max}");
        Console.ReadLine();
    }
    
    /// <summary>
    /// Renderable with an acompanying color value.
    /// </summary>
    private class ColorRenderable {
        public IRenderable Renderable;
        public Vector3 Color;
    }

    // Move Increment
    private static readonly float Inc = 0.1f;

    // -- Scene Stuff --
    private IRenderer _renderer;
    private Viewport _viewport;
    private Camera _mainCamera;

    private OpenGLRenderable _cube;
    private List<ColorRenderable> _axis;

    private ShaderProgram _program;
    private ShaderProgram _simple;

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

    // Input
    private float _mouseX = 0;
    private float _mouseY = 0;
    private Vector2 _lookRotation = new Vector2();

    // Chunk!
    private GeometryPool _pool = new GeometryPool();
    private TerrainGenerator _generator = new TerrainGenerator(new OpenSimplexNoise(123456L));

    private readonly FPS _fps = new FPS();

    private List<Chunk> _chunks;
    private List<IRenderable> _chunkMeshes;

    private Text _text;


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
        InitUi();

        _fps.OnFrameCount += frameCount => Console.WriteLine($"FPS: {frameCount}");
        _fps.Start();

        _renderer = new OpenGLRenderer();

        //Gl.PolygonMode(MaterialFace.Front, PolygonMode.Line);

        _blockTexture = new Texture("resources/terrain.png");

        _cube = NewCubeRenderable();
        _axis = NewAxisGeometry();

        _mainCamera = new Camera("MainCamera", _viewport);
        _mainCamera.MoveTo(2, 5, -10);
        _mainCamera.LookAt(_cube.Model, Vector3.UnitY);

        _chunks = InitChunks();

        _chunkMeshes = CreateMeshes(_chunks);
    }
    
    /// <summary>
    /// 
    /// </summary>
    private List<Chunk> InitChunks() {
        List<Chunk> list = new List<Chunk>();

        for (uint x = 0; x < 3; ++x) {
            for (uint z = 0; z < 3; ++z) {
                var pos = new Vector3(x * Chunk.Width, 0, z * Chunk.Depth);
                list.Add(_generator.Generate(pos));
            }
        }
        return list;
    }

    private List<IRenderable> CreateMeshes(List<Chunk> chunks) {
        float uvWidth = 2048.0f / 16.0f;
        float uvCell = uvWidth / 2048.0f;

        float xUv = uvCell * 2.0f;
        float yUv = uvCell * 15.0f;
        Vector2 uvCoordinate = new Vector2(xUv, yUv);
        Vector2 uvOffset = new Vector2(uvCell, uvCell);

        var blockPos = new Vector3();
        var list = new List<IRenderable>();
        foreach (var chunk in chunks) {
            for (uint x = 0; x < Chunk.Width; ++x) {
                for (uint y = 0; y < Chunk.Height; ++y) {
                    for (uint z = 0; z < Chunk.Depth; ++z) {
                        blockPos.X = x;
                        blockPos.Y = y;
                        blockPos.Z = z;

                        var block = chunk.At(x, y, z);

                        if (block.BlockType != BlockType.Air) {
                            foreach (Side side in Sides.All) {
                                // Chunk borders TBD
                                if (Chunk.IsChunkBorder(x, y, z)) {
                                    _pool.AddFace(Quads.QuadFor(side), block.World, uvCoordinate, uvOffset);
                                    continue;
                                }

                                // Check block next to current block in direction of the side. 
                                //  - If neighbor exists and not air block, don't draw
                                var dir = Directions.DirectionFor(side);
                                if (chunk.At(blockPos + dir).BlockType == BlockType.Air) {
                                    _pool.AddFace(Quads.QuadFor(side), block.World, uvCoordinate, uvOffset);
                                }
                            }
                        }
                    }
                }
            }
            list.Add(new ChunkMesh(chunk, _pool.ToMesh(_program)));
            _pool.Reset();
        }

        return list;
    }
    
    private void CreateWindow() {
        Window.CreateWindow("OpenGL", _viewport.Width, _viewport.Height);
        
        Window.OnReshapeCallbacks.Add(() => {
            _viewport = new Viewport(Window.Width, Window.Height);
            _mainCamera.Viewport = _viewport;
        });

        Window.OnCloseCallbacks.Add(DisposeScene);

        Console.WriteLine($"{GLHelper.DumpComputeShaderCounts()}");
    }

    private void InitInput() {
        Input.Subscribe('w', new Event(state => _forwardDown = state));
        Input.Subscribe('a', new Event(state => _leftDown = state));
        Input.Subscribe('s', new Event(state => _backDown = state));
        Input.Subscribe('d', new Event(state => _rightDown = state));
        Input.Subscribe('q', new Event(state => _upDown = state));
        Input.Subscribe('e', new Event(state => _downDown = state));

        RelativeMouse.Enabled = true;

        const float SpeedReduction = 5.0f;
        const float ClampY = 89.0f * RoxMath.ToRadians;

        // Relative mouse movement handler (Mouse lock enabled)
        RelativeMouse.AddListener((x, y) => {
            // Aggregate x and y values -- Hard Constraint on +Y axis
            _mouseX = _mouseX + (x / SpeedReduction);
            _mouseY = RoxMath.Min(_mouseY + (y / SpeedReduction), _viewport.Height - 1.0f);
            
            // Pull any out of bounds coordinates back into window coordinates
            _mouseX = (_mouseX % _viewport.Width);
            _mouseY = (_mouseY % _viewport.Height);

            // Map to Window ratio
            _lookRotation.X = (_mouseX / _viewport.Width) * RoxMath.TwoPi;
            _lookRotation.Y = (_mouseY / _viewport.Height) * RoxMath.Pi;

            // Constrain to:
            //   (-360, 360) for x-axis
            //   (-89, 89) for y-axis
            _lookRotation.X = RoxMath.Clamp(_lookRotation.X, RoxMath.TwoPi);
            _lookRotation.Y = RoxMath.Clamp(_lookRotation.Y, ClampY);
        });

        // TBD: Voxel modification
        Window.OnMouseCallbacks.Add((b, s, x, y) => {
            Console.WriteLine($"button: {b}, state: {s}, x: {x}, y: {y}");
            return true;
        });
    }

    private void InitUi() {
        /*
        UserInterface.InitUI(_viewport.Width, _viewport.Height);

        _text = new Text(Text.FontSize._16pt, "FPS: 0");
        _text.RelativeTo = Corner.Center;
        _text.Position = new Point(10, 10);
        
        UserInterface.AddElement(_text);
        */
    }

    private void SwapBuffers() {
        Window.SwapBuffers();
    }

    private void DumpFps() {
        //Console.WriteLine($"FPS: {_fps.Sum() / FpsSamples}");
    }
    
    public void Run() {
        // handle events and render the frame
        while (Window.Open) {
            _fps.Tick();

            Window.HandleEvents();
            RelativeMouse.HandleEvents();

            if (!IsElapsedFrame()) {
                continue;
            }
            _fps.Frame();

            UpdateScene();

            _renderer.Clear();
            _blockTexture.Use();

            foreach (var axis in _axis) {
                var renderable = (OpenGLRenderable)axis.Renderable;
                var shader = renderable.Geometry.Program;
                shader.Use();
                shader["Color"].SetValue(axis.Color);

                _renderer.Render(_mainCamera, renderable);
            }

            _renderer.Render(_mainCamera, _chunkMeshes);

            //_renderer.Render(_mainCamera, _cube);

            //UserInterface.Draw();
            SwapBuffers();
        }
    }

    public bool IsElapsedFrame() {
        var currentTime = _stopwatch.ElapsedMilliseconds;
        var delta = currentTime - _lastUpdate;

        if (delta < _frameTime) {
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
        _mainCamera.RotateX(_lookRotation.Y);
        _mainCamera.RotateY(-_lookRotation.X);
        
        _rotation += (1.0f * RoxMath.ToRadians);
        _cube.Model.RotateY(_rotation);
    }
    
    private void DisposeScene() {
        _blockTexture.Dispose();
        _program.Dispose();
        _simple.Dispose();
        _cube.Dispose();
        foreach (var chunkMesh in _chunkMeshes) {
            chunkMesh.Dispose();
        }

        foreach (var renderable in _axis) {
            renderable.Renderable.Dispose();
        }

        //UserInterface.Dispose();
        //BMFont.Dispose();
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
    private OpenGLRenderable NewCubeRenderable() {
        var cubeObject = new RoxObject(new Vector3(0, 0, 0));
        var cubeGeometry = NewCubeWithLightShader();

        return new OpenGLRenderable(cubeObject, cubeGeometry);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private List<ColorRenderable> NewAxisGeometry() {
        _simple = NewAxisShader();

        const float Length = 100.0f;
        const float Width = 0.02f;
        var xAxisVao = Geometry.CreateCube(_simple, new Vector3(-Length, -Width, -Width), new Vector3(Length, Width, Width));
        var yAxisVao = Geometry.CreateCube(_simple, new Vector3(-Width, -Length, -Width), new Vector3(Width, Length, Width));
        var zAxisVao = Geometry.CreateCube(_simple, new Vector3(-Width, -Width, -Length), new Vector3(Width, Width, Length));

        xAxisVao.DisposeChildren = yAxisVao.DisposeChildren = zAxisVao.DisposeChildren = true;

        return new List<ColorRenderable>() {
            new ColorRenderable() {
                Renderable = new OpenGLRenderable(new RoxObject("XAxis"), xAxisVao),
                Color = new Vector3(1.0f, 0.0f, 0.0f)
            },
            new ColorRenderable() {
                Renderable = new OpenGLRenderable(new RoxObject("yAxis"), yAxisVao),
                Color = new Vector3(0.0f, 1.0f, 0.0f)
            },
            new ColorRenderable() {
                Renderable = new OpenGLRenderable(new RoxObject("zAxis"), zAxisVao),
                Color = new Vector3(0.0f, 0.0f, 1.0f)
            }
        };
    }

    /// <summary>
    /// Creates a new cube geometry (VAO) with a light shader.
    /// </summary>
    /// <returns></returns>
    private VAO NewCubeWithLightShader() {
        _program = NewLightShader();
        
        var cubeVao = Geometry.CreateCubeWithNormals(
            _program,
            new Vector3(-1, -1, -1),
            new Vector3(1, 1, 1)); 

        cubeVao.DisposeChildren = true;
        return cubeVao;
    }

    /// <summary>
    /// Creates and sets default values for our light shader.
    /// </summary>
    /// <returns></returns>
    private ShaderProgram NewLightShader() {
        var shader = GLHelper.NewShader("resources/light-vert.glsl", "resources/light-frag.glsl");
        Console.WriteLine("Vertex Shader: {0}", shader.VertexShader.ShaderLog);
        Console.WriteLine("Fragment Shader: {0}", shader.FragmentShader.ShaderLog);

        shader["light.Position"].SetValue(new Vector3(5, 5, -2));
        shader["light.Ambient"].SetValue(new Vector3(0.3f, 0.3f, 0.3f));
        shader["light.Diffuse"].SetValue(Vector3.One);
        //shader["light.Specular"].SetValue(Vector3.One);
        shader["light.Constant"].SetValue(1.0f);
        shader["light.Linear"].SetValue(0.045f);
        shader["light.Quadratic"].SetValue(0.0075f);

        //shader["material.Color"].SetValue(new Vector3(1.0f, 1.0f, 1.0f));
        shader["material.Texture"].SetValue(0);
        return shader;
    }

    private ShaderProgram NewAxisShader() {
        var shader = GLHelper.NewShader("resources/axis-vert.glsl", "resources/axis-frag.glsl");
        Console.WriteLine("Vertex Shader: {0}", shader.VertexShader.ShaderLog);
        Console.WriteLine("Fragment Shader: {0}", shader.FragmentShader.ShaderLog);

        shader["Color"].SetValue(new Vector3(1.0f, 0.0f, 0.0f));
        return shader;
    }
}