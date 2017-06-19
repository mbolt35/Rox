using OpenGL;

namespace Rox.Geom {

    public static class Directions {

        private static readonly Vector3[] _sideToDirection = {
            Vector3.UnitY,
            -Vector3.UnitY,
            Vector3.UnitX,
            -Vector3.UnitX,
            Vector3.UnitZ,
            -Vector3.UnitZ
        };

        public static Vector3 DirectionFor(Side side) {
            return _sideToDirection[(int)side];
        }

    }
}
