using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace Rox.Geom {

    /// <summary>
    /// This static utility provides helper methods for working with <see cref="Quad"/>
    /// data structures for building chunk geometry.
    /// </summary>
    public static class Quads {
        
        /// <summary>
        /// Templated Quad that represents the top of a 1x1x1 cube.
        /// </summary>
        public static readonly Quad Top = new Quad(
            Vector3.UnitY,
            new UvVertex(1, 1, 0, 1, 1),
            new UvVertex(0, 1, 0, 0, 1),
            new UvVertex(0, 1, 1, 0, 0),
            new UvVertex(1, 1, 1, 1, 0));

        /// <summary>
        /// Templated Quad that represents the bottom of a 1x1x1 cube.
        /// </summary>
        public static readonly Quad Bottom = new Quad(
            -Vector3.UnitY,
            new UvVertex(1, 0, 1, 0, 0),
            new UvVertex(0, 0, 1, 1, 0),
            new UvVertex(0, 0, 0, 1, 1),
            new UvVertex(1, 0, 0, 0, 1));

        /// <summary>
        /// Templated Quad that represents the front of a 1x1x1 cube.
        /// </summary>
        public static readonly Quad Front = new Quad(
            Vector3.UnitZ,
            new UvVertex(1, 1, 1, 1, 1),
            new UvVertex(0, 1, 1, 0, 1),
            new UvVertex(0, 0, 1, 0, 0),
            new UvVertex(1, 0, 1, 1, 0));

        /// <summary>
        /// Templated Quad that represents the back of a 1x1x1 cube.
        /// </summary>
        public static readonly Quad Back = new Quad(
            -Vector3.UnitZ,
            new UvVertex(1, 0, 0, 0, 0),
            new UvVertex(0, 0, 0, 1, 0),
            new UvVertex(0, 1, 0, 1, 1),
            new UvVertex(1, 1, 0, 0, 1));

        /// <summary>
        /// Templated Quad that represents the right of a 1x1x1 cube.
        /// </summary>
        public static readonly Quad Right = new Quad(
            Vector3.UnitX,
            new UvVertex(1, 1, 0, 1, 1),
            new UvVertex(1, 1, 1, 0, 1),
            new UvVertex(1, 0, 1, 0, 0),
            new UvVertex(1, 0, 0, 1, 0));

        /// <summary>
        /// Templated Quad that represents the left of a 1x1x1 cube.
        /// </summary>
        public static readonly Quad Left = new Quad(
            -Vector3.UnitX,
            new UvVertex(0, 1, 1, 1, 1),
            new UvVertex(0, 1, 0, 0, 1),
            new UvVertex(0, 0, 0, 0, 0),
            new UvVertex(0, 0, 1, 1, 0));

        /// <summary>
        /// Array of all templated block quads, ordered by <see cref="Side"/>.
        /// </summary>
        private static readonly Quad[] _sideToQuad = {
            Top,
            Bottom,
            Right,
            Left,
            Front,
            Back
        };

        /// <summary>
        /// Returns a <see cref="Quad"/> representation of the <see cref="Side"/> provided.
        /// </summary>
        /// <param name="side">The side which to lookup the <see cref="Quad"/> representation for.</param>
        /// <returns></returns>
        public static Quad QuadFor(Side side) {
            return _sideToQuad[(int)side];
        }
    }
}
