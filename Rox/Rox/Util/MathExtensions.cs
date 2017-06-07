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
    }
}
