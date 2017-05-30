using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using OpenGL.Platform;
using System.Diagnostics;

namespace Rox {

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
            #version 440

            uniform mat4 projection_matrix;
            uniform mat4 model_matrix;
            uniform mat4 view_matrix;

            uniform vec3 lightPosition;

            in vec3 in_position;
            in vec3 in_normal;

            out float cosTheta;
            out float lightIntensity;

            void main(void)
            {
                mat4 mv = view_matrix * model_matrix;
                vec4 vertex_t = mv * vec4(in_position, 1);
                vec4 normal_t = mv * vec4(in_normal, 1);

                vec3 n = normalize(normal_t.xyz);
                vec3 l = normalize(lightPosition - vertex_t.xyz);

                cosTheta = clamp(dot(n, l), 0.0f, 1.0f);

                lightIntensity = distance(lightPosition, vertex_t.xyz) / 20.0f;
                
                gl_Position = projection_matrix * vertex_t;
            }";

        /// <summary>
        /// Fragment Shader
        /// </summary>
        public static string fragmentShader2Source = @"
            #version 440
            uniform vec3 color;

            in float cosTheta;
            in float lightIntensity;

            void main(void)
            {
                vec3 lightColor = cosTheta * lightIntensity * color; 
                gl_FragColor = vec4(lightColor, 1);
            }";

        // Window Dimensions
        private int _width;
        private int _height;

        private float _rotation = 0.0f;
        private Vector3 _rotationAxis = new Vector3(0, -1, 0);
        private Vector3 _translation;
        private Matrix4 _viewMatrix;
        private Vector3 _lightPosition;

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
            _width = width;
            _height = height;

            Init();
        }

        /// <summary>
        /// Initialize GL Window, Shader, and Cube VAO
        /// </summary>
        private void Init() {
            // create an OpenGL window
            Window.CreateWindow("OpenGL", _width, _height);
            OpenGLVersions();

            Gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.ClearDepthf(1.0f);

            Gl.Enable(EnableCap.Multisample);
            Gl.Enable(EnableCap.DepthTest);
            Gl.DepthFunc(DepthFunction.Lequal);

            Gl.Enable(EnableCap.CullFace);
            Gl.CullFace(CullFaceMode.Back);

            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // create the shader program
            _shader = new ShaderProgram(vertexShader2Source, fragmentShader2Source);
            
            // Projection variables
            float fieldOfView = 0.45f;
            float aspectRatio = (float)_width / _height;
            float zNearPlane = 0.1f;
            float zFarPlane = 1000.0f;
            
            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, zNearPlane, zFarPlane);

            _translation = new Vector3(2, 5, -10);
            _position = new Vector3(0, 0, 0);

            _viewMatrix = Matrix4.LookAt(_translation, _position, Vector3.UnitY);

            _lightPosition = new Vector3(10, 15, -20);

            // set up some defaults for the shader program project and modelview matrices
            _shader["projection_matrix"].SetValue(projectionMatrix);
            _shader["view_matrix"].SetValue(_viewMatrix);
            _shader["model_matrix"].SetValue(Matrix4.CreateTranslation(_position));
            _shader["lightPosition"].SetValue(_lightPosition);

            // set the color to blue
            _shader["color"].SetValue(new Vector3(0, 0, 1));

            // create a cube
            _cubeVao = Geometry.CreateCubeWithNormals(_shader, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
            _cubeVao.DisposeChildren = true;
            _cubeVao.DisposeElementArray = true;
        }

        private long _frameTime = 16;
        private long _lastUpdate = 0;
        private bool _forward = true;
        private Vector3 _position;

        private Stopwatch _stopwatch = Stopwatch.StartNew();

        public void Run() {
            // handle events and render the frame
            while (Window.Open) {
                Window.HandleEvents();
                var currentTime = _stopwatch.ElapsedMilliseconds;
                if (currentTime - _lastUpdate < _frameTime)
                {
                    continue;
                }

                _lastUpdate = currentTime;
                
                _rotation += 0.05f;
                
                if (_forward)
                {
                    _position.Y += 0.05f;
                } else
                {
                    _position.Y -= 0.05f;
                }
                
                if (_forward && _position.Y > 3.0f)
                {
                    _forward = false;
                } 
                else if (!_forward && _position.Y < -3.0f)
                {
                    _forward = true;
                }

                //_viewMatrix = Matrix4.LookAt(_translation, Vector3.Zero, -Vector3.UnitY);

                Matrix4 cubeRotation = Matrix4.CreateRotation(_rotationAxis, _rotation);
                Matrix4 modelMatrix = cubeRotation * Matrix4.CreateTranslation(_position);

                _shader["view_matrix"].SetValue(_viewMatrix);
                _shader["model_matrix"].SetValue(modelMatrix);

                OnRenderFrame();
            }

            _shader.Dispose();
            _cubeVao.Dispose();
        }

        private void OnRenderFrame() {
            Gl.Viewport(0, 0, _width, _height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _cubeVao.Program.Use();
            _cubeVao.Draw();

            Window.SwapBuffers();
        }

        private static void OpenGLVersions()
        {
            Console.WriteLine("OpenGL Version: {0}", Gl.GetString(OpenGL.StringName.Version));
            Console.WriteLine("GLSL Version: {0}", Gl.GetString(StringName.ShadingLanguageVersion));
        }
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