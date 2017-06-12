using OpenGL;
using Rox.Core;

namespace Rox.Render.GL {

    /// <summary>
    /// OpenGL implementation of <c>IRenderable</c>.
    /// </summary>
    public class OpenGLRenderable : IRenderable {

        /// <summary>
        /// The object representation of the renderable instance
        /// </summary>
        private readonly IRoxObject _model;

        /// <summary>
        /// The renderable geometry for the instance.
        /// </summary>
        private readonly VAO _geometry;

        /// <summary>
        /// The geometric representation of our renderable instance.
        /// </summary>
        public VAO Geometry {
            get { return _geometry; }
        }

        /// <summary>
        /// The model representation of our renderable instance.
        /// </summary>
        public IRoxObject Model { 
            get { return _model; }
        }

        public OpenGLRenderable(IRoxObject model, VAO geometry) {
            _model = model;
            _geometry = geometry;
        }

        /// <summary>
        /// This uses the <c>Camera</c> and <c>IRoxObject</c> inputs to apply and render
        /// current state.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="obj"></param>
        public void Draw(Camera camera) {
            var shader = _geometry.Program;
            shader.Use();
            shader["ProjectionMatrix"].SetValue(camera.Projection);
            shader["ViewMatrix"].SetValue(camera.Transform);
            shader["ModelMatrix"].SetValue(_model.Transform);

            _geometry.Draw();
        }

        /// <summary>
        /// Draws the internal geometrical representation.
        /// </summary>
        public void Draw() {
            var shader = _geometry.Program;
            shader.Use();
            shader["ModelMatrix"].SetValue(_model.Transform);

            _geometry.Draw();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            _geometry.Dispose();
        }
    }
}
