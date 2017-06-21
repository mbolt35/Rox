using OpenGL;

namespace Rox.Geom {

    /// <summary>
    /// This static utility class provides methods for finding <see cref="Vector3"/> directions based
    /// on the <see cref="Side"/> enumeration.
    /// </summary>
    public static class Directions {

        /// <summary>
        /// All direction vectors ordered by <see cref="Side"/>
        /// </summary>
        private static readonly Vector3[] _sideToDirection = {
            Vector3.UnitY,
            -Vector3.UnitY,
            Vector3.UnitX,
            -Vector3.UnitX,
            Vector3.UnitZ,
            -Vector3.UnitZ
        };

        /// <summary>
        /// Returns a <see cref="Vector3"/> direction vector given the <see cref="Side"/>.
        /// </summary>
        /// <param name="side">The side enumeration used to lookup the direction.</param>
        /// <returns></returns>
        public static Vector3 DirectionFor(Side side) {
            return _sideToDirection[(int)side];
        }
        
    }
}
