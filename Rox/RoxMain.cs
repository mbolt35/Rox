using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using OpenGL.Platform;

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
            uniform mat4 modelview_matrix;

            attribute vec3 in_position;

            void main(void)
            {
                gl_Position = projection_matrix * modelview_matrix * vec4(in_position, 1);
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

            Matrix4 rotation = Matrix4.CreateRotation(new Vector3(1, -1, 0), 0.2f);
            Vector3 position = new Vector3(2, 2, -10);

            Matrix4 translationMatrix = Matrix4.CreateTranslation(position) * rotation;

            // set up some defaults for the shader program project and modelview matrices
            _shader["projection_matrix"].SetValue(projectionMatrix);
            _shader["modelview_matrix"].SetValue(translationMatrix);

            // set the color to blue
            _shader["color"].SetValue(new Vector3(0, 0, 1));

            // create a cube
            _cubeVao = Geometry.CreateCube(_shader, new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
            _cubeVao.DisposeChildren = true;
            _cubeVao.DisposeElementArray = true;
        }

        public void Run() {
            // handle events and render the frame
            while (Window.Open) {
                Window.HandleEvents();
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