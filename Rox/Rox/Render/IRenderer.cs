using OpenGL;
using Rox.Core;

namespace Rox.Render {

    /// <summary>
    /// 
    /// </summary>
    public interface IRenderer {

        /// <summary>
        /// A formatted version string representing the renderer version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Clears the render
        /// </summary>
        void Clear();

        /// TODO: Render entire scene.
        // void Render(Camera camera, IRoxScene scene);

        /// <summary>
        /// Renders a single <c>IRenderable</c> to the screen. 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="renderable"></param>
        void Render(Camera camera, IRenderable renderable);
    }
}
