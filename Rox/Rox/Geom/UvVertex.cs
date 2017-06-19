using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace Rox.Geom {

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct UvVertex {

        private readonly Vector3 _vertex;
        private readonly Vector2 _uv;

        public float X { get { return _vertex.X; } }
        public float Y { get { return _vertex.Y; } }
        public float Z { get { return _vertex.Z; } }

        public float U { get { return _uv.X; } }
        public float V { get { return _uv.Y; } }

        public Vector3 Vertex { get { return _vertex; } }
        public Vector2 UV { get { return _uv; } }

        public UvVertex(float x, float y, float z, float u, float v) 
            : this(new Vector3(x, y, z), new Vector2(u, v)) 
        { }

        public UvVertex(Vector3 vertex, Vector2 uv) {
            _vertex = vertex;
            _uv = uv;
        }

        public UvVertex ResolveVertex(Vector3 v) {
            return new UvVertex(_vertex + v, _uv);
        }


    }
}
