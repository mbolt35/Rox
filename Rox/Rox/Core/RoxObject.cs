using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenGL;

namespace Rox.Core {

    /// <summary>
    /// Abstract generic Rox object implementation.
    /// </summary>
    public class RoxObject : IRoxObject {

        private static long Ids = 1;

        private static string NewDefaultName() {
            return string.Format("RoxObject-{0}", Interlocked.Increment(ref Ids));
        }

        #region Variables

        private Matrix4 _transform;
        private Matrix4 _rotationMatrix;

        private bool _dirty = false;

        #endregion

        public string Name { get; protected set; }

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
                UpdateTransform();

                return _transform;
            }
        }

        public Quaternion Rotation { get; protected set; }

        public Vector3 Position { get; protected set; }

        public RoxObject() 
            : this(NewDefaultName(), Vector3.Zero, Quaternion.Zero)
        { }

        public RoxObject(string name)
            : this(name, Vector3.Zero, Quaternion.Zero) 
        { }

        public RoxObject(string name, Vector3 position)
            : this(name, position, Quaternion.Zero) 
        { }

        public RoxObject(Vector3 position) 
            : this(NewDefaultName(), position, Quaternion.Zero) 
        { }

        public RoxObject(string name, Vector3 position, Quaternion rotation) {
            Name = name;
            Position = position;
            Rotation = rotation;
            _transform = Matrix4.Identity;

            UpdateRotation(true);
            UpdateTransform(true);
        }
        
        public virtual void Move(Vector3 delta) {
            Position += delta;
            _dirty = true;
        }

        public virtual void Move(float x, float y, float z) {
            Move(new Vector3(x, y, z));
        }

        public virtual void MoveTo(Vector3 position) {
            Position = position;
            _dirty = true;
        }

        public virtual void MoveTo(float x, float y, float z) {
            MoveTo(new Vector3(x, y, z));
        }

        public virtual void Rotate(Vector3 axis, float angle) {
            Rotation = Quaternion.FromAngleAxis(angle, axis);
            _dirty = true;
        }

        public virtual void LookAt(Vector3 target, Vector3 up) {
            _transform = Matrix4.LookAt(Position, target, up);
            _dirty = false;
        }

        public virtual void LookAt(IRoxObject obj, Vector3 up) {
            _transform = Matrix4.LookAt(Position, obj.Position, up);
            _dirty = false;
        }

        protected virtual bool UpdateRotation(bool force = false) {
            if (!force && !_dirty) {
                return false;
            }

            _rotationMatrix = Rotation.Matrix4;
            _dirty = false;
            return true;
        }

        protected virtual bool UpdateTransform(bool force = false) {
            if (!force && !_dirty) {
                return false;
            }

            LookAt(Position + Forward, Up);
            _dirty = false;
            return true;
        }
    }
}
