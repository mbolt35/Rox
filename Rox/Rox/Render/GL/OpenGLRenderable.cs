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
        /// AABB representing the bounds of our renderable object
        /// </summary>
        private readonly AxisAlignedBoundingBox _bounds;

        /// <summary>
        /// The bounding box for the renderable object.
        /// </summary>
        public AxisAlignedBoundingBox Bounds {
            get {
                UpdateBounds();

                return _bounds;
            }
        }

        /// <summary>
        /// Whether or not culling is enabled for this renderable.
        /// </summary>
        public bool CullingEnabled { get; set; } = false;

        /// <summary>
        /// The geometric representation of our renderable instance.
        /// </summary>
        public VAO Geometry => _geometry;

        /// <summary>
        /// The model representation of our renderable instance.
        /// </summary>
        public IRoxObject Model => _model;

        /// <summary>
        /// Creates a new <c>OpenGLRenderable</c> instance.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="geometry"></param>
        public OpenGLRenderable(IRoxObject model, VAO geometry) {
            _model = model;
            _geometry = geometry;
            _bounds = new AxisAlignedBoundingBox();
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
        private void UpdateBounds() {
            _bounds.SetExtents(_model.Position, _model.Position + Vector3.One);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            _geometry.Dispose();
        }
    }
}
