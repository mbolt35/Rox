using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace Rox.Core
{
    /// <summary>
    /// Abstract object instance in a rox simulation.
    /// </summary>
    public interface IRoxObject {

        #region Properties 

        /// <summary>
        /// The current forward direction vector.
        /// </summary>
        Vector3 Forward { get; }

        /// <summary>
        /// The current up direction vector.
        /// </summary>
        Vector3 Up { get; }

        /// <summary>
        /// The current transform matrix for the object.
        /// </summary>
        Matrix4 Transform { get; }

        /// <summary>
        /// The current rotation of the object.
        /// </summary>
        Quaternion Rotation { get; }

        /// <summary>
        /// The position of the object.
        /// </summary>
        Vector3 Position { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Rotates the object on the supplies axis by the angle (in radians?).
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        void Rotate(Vector3 axis, float angle);
        
        /// <summary>
        /// Offsets the object by a specific amount.
        /// </summary>
        /// <param name="delta"></param>
        void Move(Vector3 delta);

        /// <summary>
        /// Offsets the object by a specific amount.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void Move(float x, float y, float z);

        /// <summary>
        /// Moves to an exact coordinate.
        /// </summary>
        /// <param name="position"></param>
        void MoveTo(Vector3 position);

        /// <summary>
        /// Moves to an exact coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void MoveTo(float x, float y, float z);

        /// <summary>
        /// Rotates the object to look at a specific target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="up"></param>
        void LookAt(Vector3 target, Vector3 up);

        #endregion
    }
}
