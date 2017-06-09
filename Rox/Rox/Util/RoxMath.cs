using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace Rox.Util {

    /// <summary>
    /// Rox math helper.
    /// </summary>
    public static class RoxMath {

        /// <summary>
        /// Float Pi constant
        /// </summary>
        public static readonly float Pi = (float)Math.PI;

        /// <summary>
        /// 2.0 * Pi constant
        /// </summary>
        public static readonly float TwoPi = 2.0f * Pi;

        /// <summary>
        /// Pi/2.0 constant
        /// </summary>
        public static readonly float PiOverTwo = Pi / 2.0f;

        /// <summary>
        /// Radian conversion constant
        /// </summary>
        public static readonly float ToRadians = Pi / 180.0f;

        /// <summary>
        /// Degrees conversion constant
        /// </summary>
        public static readonly float ToDegrees = 180.0f / Pi;

        /// <summary>
        /// Creates a Quaternion product using rotations for X, Y, Z respectively.
        /// </summary>
        /// <param name="angles"></param>
        /// <returns></returns>
        public static Quaternion Euler(Vector3 angles) {
            return Euler(angles.X, 0.0f, 0.0f)
                * Euler(0.0f, angles.Y, 0.0f)
                * Euler(0.0f, 0.0f, angles.Z);
        }

        /// <summary>
        /// Euler angle to Quaternion.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static Quaternion Euler(float xRot, float yRot, float zRot) { 
            float phi = xRot * 0.5f;
            float theta = yRot * 0.5f;
            float psi = zRot * 0.5f;

            float sinPhi = (float)Math.Sin(phi);
            float sinTheta = (float)Math.Sin(theta);
            float sinPsi = (float)Math.Sin(psi);

            float cosPhi = (float)Math.Cos(phi);
            float cosTheta = (float)Math.Cos(theta);
            float cosPsi = (float)Math.Cos(psi);

            float x = (sinPhi * cosTheta * cosPsi) - (cosPhi * sinTheta * sinPsi);
            float y = (cosPhi * sinTheta * cosPsi) + (sinPhi * cosTheta * sinPsi);
            float z = (cosPhi * cosTheta * sinPsi) - (sinPhi * sinTheta * cosPsi);
            float w = (cosPhi * cosTheta * cosPsi) + (sinPhi * sinTheta * sinPsi);

            return new Quaternion(x, y, z, w);
        }

        /// <summary>
        /// Converts a quaternion into euler angles.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Vector3 ToEuler(Quaternion q) {
            float x = q.X;
            float y = q.Y;
            float z = q.Z;
            float w = q.W;

            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float w2 = w * w;

            float t1 = 2.0f * (y * z + w * x);
            float t2 = w2 - x2 - y2 + z2;

            float t3 = -2.0f * (x * z - w * y);

            float t4 = 2.0f * (x * y + w * z);
            float t5 = w2 + x2 - y2 - z2;

            float pitch = (float)Math.Atan2(t1, t2);
            float yaw = (float)Math.Asin(t3);
            float roll = (float)Math.Atan2(t4, t5);

            return new Vector3(pitch, yaw, roll);
        }
    }
}
