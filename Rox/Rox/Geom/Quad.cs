using System.Runtime.InteropServices;
using System;
using OpenGL;

namespace Rox.Geom {

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct Quad {

        public UvVertex V0 { get; private set; }

        public UvVertex V1 { get; private set; }

        public UvVertex V2 { get; private set; }

        public UvVertex V3 { get; private set; }

        // TODO: Optimize out if necessary
        public Vector3 Normal { get; private set; }

        public Quad(Vector3 normal, UvVertex v0, UvVertex v1, UvVertex v2, UvVertex v3) {
            Normal = normal;
            V0 = v0;
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public UvVertex At(int index) {
            switch(index) {
                case 0: return V0;
                case 1: return V1;
                case 2: return V2;
                case 3: return V3;
                default: throw new IndexOutOfRangeException($"Index: {index} is out of range.");
            }
        }
    }
}
