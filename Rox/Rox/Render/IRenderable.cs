using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rox.Core;

namespace Rox.Render {

    /// <summary>
    /// Defines a basic contract for an entity which can be rendered by <c>IRenderer</c>
    /// </summary>
    public interface IRenderable {
        
        /// <summary>
        /// The model representation of the renderable.
        /// </summary>
        IRoxObject Model { get; }

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
