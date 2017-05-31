using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace Rox.Core {

    /// <summary>
    /// Abstract generic Rox object implementation.
    /// </summary>
    public class RoxObject : IRoxObject {

        #region Variables

        private Matrix4 _transform;
        private Matrix4 _rotationMatrix;

        private bool _dirty = false;

        #endregion

        public Vector3 Forward {
            get {
                UpdateRotation();

                return (_rotationMatrix * Vector3.UnitZ).Normalize();
            }
        }

        public Vector3 Up {
            get {
                UpdateRotation();

                return (_rotationMatrix * Vector3.UnitY).Normalize();
            }
        }

        public Matrix4 Transform {
            get {
                UpdateTransform(true);

                return _transform;
            }
        }

        public Quaternion Rotation { get; protected set; }

        public Vector3 Position { get; protected set; }

        public RoxObject() 
            : this(Vector3.Zero, Quaternion.Zero)
        { }

        public RoxObject(Vector3 position) 
            : this(position, Quaternion.Zero) 
        { }

        public RoxObject(Vector3 position, Quaternion rotation) {
            Position = position;
            Rotation = rotation;
            _transform = Matrix4.Identity;

            UpdateRotation(true);
            UpdateTransform(true);
        }
        
        public void Move(Vector3 delta) {
            Position += delta;
            _dirty = true;
        }

        public void Move(float x, float y, float z) {
            Move(new Vector3(x, y, z));
        }

        public void MoveTo(Vector3 position) {
            Position = position;
            _dirty = true;
        }

        public void MoveTo(float x, float y, float z) {
            MoveTo(new Vector3(x, y, z));
        }

        public void Rotate(Vector3 axis, float angle) {
            Rotation = Quaternion.FromAngleAxis(angle, axis);
            _dirty = true;
        }

        public void LookAt(Vector3 target, Vector3 up) {
            _transform = Matrix4.LookAt(Position, target, up);
            _dirty = false;
        }

        protected void UpdateRotation(bool force = false) {
            if (!force && !_dirty) {
                return;
            }

            _rotationMatrix = Rotation.Matrix4;
            _dirty = false;
        }

        protected void UpdateTransform(bool force = false) {
            if (!force && !_dirty) {
                return;
            }

            LookAt(Position + Forward, Up);
            _dirty = false;
        }
    }
}
