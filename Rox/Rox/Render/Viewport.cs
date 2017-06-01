using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rox.Render {

    /// <summary>
    /// Viewport class uses to determine aspect ratio. 
    /// </summary>
    public struct Viewport {
        
        /// <summary>
        /// X-Coordinate of the viewport
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y-Coordinate of the viewport
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Width of the viewport.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height of the viewport
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The aspect ratio of the viewport.
        /// </summary>
        public float AspectRatio {
            get {
                return (float)Width / Height;
            }
        }
        
        public Viewport(int width, int height)
            : this(0, 0, width, height) { }

        public Viewport(int x, int y, int width, int height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }


    }
}
