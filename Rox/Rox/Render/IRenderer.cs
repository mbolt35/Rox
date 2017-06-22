using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// Render all elements of the enumerable to the screen.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="renderables"></param>
        void Render(Camera camera, IEnumerable<IRenderable> renderables);

        /// <summary>
        /// Renders a single <c>IRenderable</c> to the screen. 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="renderable"></param>
        void Render(Camera camera, IRenderable renderable);
    }
}
