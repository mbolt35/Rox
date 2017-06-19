using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace Rox.Geom {

    /// <summary>
    /// 
    /// </summary>
    public static class Quads {

        public static readonly Quad Top = new Quad(
            Vector3.UnitY,
            new UvVertex(0, 1, 0, 0, 0),
            new UvVertex(1, 1, 0, 0, 1),
            new UvVertex(0, 1, 1, 1, 0),
            new UvVertex(1, 1, 1, 1, 1));

        public static readonly Quad Bottom = new Quad(
            -Vector3.UnitY,
            new UvVertex(0, 0, 0, 0, 0),
            new UvVertex(1, 0, 0, 0, 1),
            new UvVertex(0, 0, 1, 1, 0),
            new UvVertex(1, 0, 1, 1, 1));

        public static readonly Quad Right = new Quad(
            Vector3.UnitX,
            new UvVertex(1, 0, 0, 0, 0),
            new UvVertex(1, 0, 1, 0, 1),
            new UvVertex(1, 1, 0, 1, 0),
            new UvVertex(1, 1, 1, 1, 1));

        public static readonly Quad Left = new Quad(
            -Vector3.UnitX,
            new UvVertex(0, 0, 0, 0, 0),
            new UvVertex(0, 0, 1, 0, 1),
            new UvVertex(0, 1, 0, 1, 0),
            new UvVertex(0, 1, 1, 1, 1));

        public static readonly Quad Front = new Quad(
            Vector3.UnitZ,
            new UvVertex(0, 0, 1, 0, 0),
            new UvVertex(0, 1, 1, 0, 1),
            new UvVertex(1, 0, 1, 1, 0),
            new UvVertex(1, 1, 1, 1, 1));

        public static readonly Quad Back = new Quad(
            -Vector3.UnitZ,
            new UvVertex(0, 0, 0, 0, 0),
            new UvVertex(0, 1, 0, 0, 1),
            new UvVertex(1, 0, 0, 1, 0),
            new UvVertex(1, 1, 0, 1, 1));
    }
}
