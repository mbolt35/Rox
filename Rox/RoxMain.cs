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
            uniform mat4 projection_matrix;
            uniform mat4 model_matrix;
            uniform mat4 view_matrix;

            attribute vec3 in_position;

            void main(void)
            {
                vec4 appliedModel = model_matrix * vec4(in_position, 1);

                gl_Position = projection_matrix * view_matrix * appliedModel;
            }";

        /// <summary>
        /// Fragment Shader
        /// </summary>
        public static string fragmentShader2Source = @"
            uniform vec3 color;

            void main(void)
            {
                gl_FragColor = vec4(color, 1);
            }";

        // Window Dimensions
        private int _width;
        private int _height;

        private float _rotation = 0.0f;
        private Vector3 _rotationAxis = new Vector3(0, -1, 0);

        private Matrix4 _translationMatrix;

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

            // create the shader program
            _shader = new ShaderProgram(vertexShader2Source, fragmentShader2Source);
            
            // Projection variables
            float fieldOfView = 0.45f;
            float aspectRatio = (float)_width / _height;
            float zNearPlane = 0.1f;
            float zFarPlane = 1000.0f;

            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, zNearPlane, zFarPlane);

            Vector3 position = new Vector3(0, 0, -10);

            _translationMatrix = Matrix4.CreateTranslation(position);

            

            // set up some defaults for the shader program project and modelview matrices
            _shader["projection_matrix"].SetValue(projectionMatrix);
            _shader["view_matrix"].SetValue(_translationMatrix);
            _shader["model_matrix"].SetValue(Matrix4.CreateTranslation(new Vector3(0, 0, 0)));

            // set the color to blue
            _shader["color"].SetValue(new Vector3(0, 0, 1));

            // create a cube
            _cubeVao = Geometry.CreateCube(_shader, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
            _cubeVao.DisposeChildren = true;
            _cubeVao.DisposeElementArray = true;
        }

        private long _frameTime = 16;
        private long _lastUpdate = 0;

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

                Matrix4 modelMatrix = Matrix4.CreateTranslation(new Vector3(0, 0, 0)) * Matrix4.CreateRotation(_rotationAxis, _rotation);
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