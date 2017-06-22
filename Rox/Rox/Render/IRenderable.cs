using OpenGL;
using Rox.Core;

namespace Rox.Render {

    /// <summary>
    /// Defines a basic contract for an entity which can be rendered by <c>IRenderer</c>
    /// </summary>
    public interface IRenderable {
        
        /// <summary>
        /// Whether or not culling is enabled for this renderable
        /// </summary>
        bool CullingEnabled { get; set; }

        /// <summary>
        /// The bounding box for the renderably geometry.
        /// </summary>
        AxisAlignedBoundingBox Bounds { get; }

        /// <summary>
        /// Uses camera to draw internal geometrical representation. 
        /// </summary>
        /// <param name="camera"></param>
        void Draw(Camera camera);

        /// <summary>
        /// Draws the internal geometrical representation.
        /// </summary>
        void Draw();

        /// <summary>
        /// Disposes of any unmanaged resources
        /// </summary>
        void Dispose();


    }
}
