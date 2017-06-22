using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rox.Util;

namespace OpenGL { 

    /// <summary>
    /// 
    /// </summary>
    public static class MathExtensions {

        public static Vector3 ToEulerAngles(this Quaternion @this) {
            return RoxMath.ToEuler(@this);
        }

        /// <summary>
        /// This returns a more defined hash value for <see cref="Vector3"/> compared
        /// to <see cref="Vector3.GetHashCode"/>.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static int Hash(this Vector3 @this) {
            return @this.X.GetHashCode() ^ @this.Y.GetHashCode() << 2 ^ @this.Z.GetHashCode() >> 2;
        }

        /*
        public static void Set(this Vector3 @this, float x, float y, float z) {
            @this.X = x;
            @this.Y = y;
            @this.Z = z;
        }

        public static void Set(this Vector3 @this, uint x, uint y, uint z) {
            @this.X = x;
            @this.Y = y;
            @this.Z = z;
        }

        public static void Set(this Vector3 @this, int x, int y, int z) {
            @this.X = x;
            @this.Y = y;
            @this.Z = z;
        }
        */
    }
}
