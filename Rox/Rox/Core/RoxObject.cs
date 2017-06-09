using System;
using OpenGL;
using Rox.Util;

namespace Rox.Core {

    /// <summary>
    /// Abstract generic Rox object implementation.
    /// </summary>
    public class RoxObject : IRoxObject {

        /// <summary>
        /// Dirty flag enumeration used to track the changed values for
        /// the object.
        /// </summary>
        [Flags]
        private enum DirtyFlag : byte {
            None = 0,
            Rotation = 1,
            Translation = 2
        }

        /// <summary>
        /// RoxObject instance namer.
        /// </summary>
        private static readonly InstanceNamer Namer = new InstanceNamer("RoxObject");

        /// <summary>
        /// Transform Matrix
        /// </summary>
        private Matrix4 _transform = Matrix4.Identity;

        /// <summary>
        /// Rotation Matrix
        /// </summary>
        private Matrix4 _rotationMatrix = Matrix4.Identity;

        /// <summary>
        /// axis rotations
        /// </summary>
        private float _xRotation = 0.0f;
        private float _yRotation = 0.0f;
        private float _zRotation = 0.0f;

        /// <summary>
        /// Dirty flag for rotation and translation
        /// </summary>
        private DirtyFlag _dirty = DirtyFlag.None;

        /// <summary>
        /// Returns <c>true</c> if the rotation is dirty.
        /// </summary>
        private bool IsRotationDirty {
            get { return (_dirty & DirtyFlag.Rotation) == DirtyFlag.Rotation; }
        }

        /// <summary>
        /// Returns <c>true</c> if the translation for the object is dirty.
        /// </summary>
        private bool IsTranslationDirty {
            get { return (_dirty & DirtyFlag.Translation) == DirtyFlag.Translation; }
        }

        /// <summary>
        /// The name of the instance.
        /// </summary>
        public string Name { get; protected set; }
        
        /// <summary>
        /// Current position of the object.
        /// </summary>
        public Vector3 Position { get; protected set; }

        /// <summary>
        /// Rotation of the current object, represented as a Quaternion.
        /// </summary>
        public Quaternion Rotation { get; protected set; }

        /// <summary>
        /// The current transform matrix (translation and rotation) for the object.
        /// </summary>
        public Matrix4 Transform {
            get {
                var isRotationUpdate = UpdateRotation();
                UpdateTransform(isRotationUpdate);

                return _transform;
            }
        }

        /// <summary>
        /// Current forward vector based on the rotation.
        /// </summary>
        public Vector3 Forward {
            get {
                UpdateRotation();

                return (_rotationMatrix * Vector3.UnitZ).Normalize();
            }
        }

        /// <summary>
        /// Current up vector based on the rotation.
        /// </summary>
        public Vector3 Up {
            get {
                UpdateRotation();

                return (_rotationMatrix * Vector3.UnitY).Normalize();
            }
        }

        /// <summary>
        /// Creates a new <c>RoxObject</c> instance.
        /// </summary>
        public RoxObject() 
            : this(Namer.NextName(), Vector3.Zero, Quaternion.Zero)
        { }

        /// <summary>
        /// Creates a new <c>RoxObject</c> with a name.
        /// </summary>
        /// <param name="name"></param>
        public RoxObject(string name)
            : this(name, Vector3.Zero, Quaternion.Zero) 
        { }

        /// <summary>
        /// Creates a new <c>RoxObject</c> with a name nad position.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        public RoxObject(string name, Vector3 position)
            : this(name, position, Quaternion.Zero) 
        { }

        /// <summary>
        /// Creates a <c>RoxObject</c> with a position.
        /// </summary>
        /// <param name="position"></param>
        public RoxObject(Vector3 position) 
            : this(Namer.NextName(), position, Quaternion.Zero) 
        { }

        /// <summary>
        /// Creates a <c>RoxObject</c> with name, position, and rotation.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public RoxObject(string name, Vector3 position, Quaternion rotation) {
            Name = name;
            Position = position;
            Rotation = rotation;

            _dirty = DirtyFlag.Rotation | DirtyFlag.Translation;

            UpdateRotation();
            UpdateTransform(true);
        }

        /// <summary>
        /// Rotates the object instance along the x-axis by the provided amount, in radians.
        /// </summary>
        /// <param name="x">Rotation around the x-axis in radians.</param>
        public void RotateX(float x) {
            _xRotation = x;

            _dirty |= DirtyFlag.Rotation;
        }

        /// <summary>
        /// Rotates the object instance along the y-axis by the provided amount, in radians.
        /// </summary>
        /// <param name="y">Rotation around the y-axis in radians.</param>
        public void RotateY(float y) {
            _yRotation = y;

            _dirty |= DirtyFlag.Rotation;
        }

        /// <summary>
        /// Rotates the object instance along the z-axis by the provided amount, in radians.
        /// </summary>
        /// <param name="z">Rotation around the z-axis in radians.</param>
        public void RotateZ(float z) {
            _zRotation = z;

            _dirty |= DirtyFlag.Rotation;
        }

        /// <summary>
        /// Moves the object by a specific amount.
        /// </summary>
        /// <param name="delta"></param>
        public virtual void Move(Vector3 delta) {
            if (delta.LengthSquared() <= float.Epsilon) {
                return;
            }

            Position += delta;
            _dirty |= DirtyFlag.Translation;
        }

        /// <summary>
        /// Moves the object by a specific amount.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public virtual void Move(float x, float y, float z) {
            Move(new Vector3(x, y, z));
        }

        /// <summary>
        /// Moves the object to a specific position.
        /// </summary>
        /// <param name="position"></param>
        public virtual void MoveTo(Vector3 position) {
            Position = position;

            _dirty |= DirtyFlag.Translation;
        }

        /// <summary>
        /// Moves the object to a specific position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public virtual void MoveTo(float x, float y, float z) {
            MoveTo(new Vector3(x, y, z));
        }

        /// <summary>
        /// Looks at the target <c>Vector3</c> position.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="up"></param>
        public virtual void LookAt(Vector3 target, Vector3 up) {
            _transform = Matrix4.LookAt(Position, target, up);

            Vector3 z = (target - Position).Normalize();
            Vector3 x = Vector3.Cross(up, z).Normalize();
            Vector3 y = Vector3.Cross(z, x).Normalize();

            _rotationMatrix = new Matrix4(
                new Vector4(x.X, y.X, z.X, 0.0f),
                new Vector4(x.Y, y.Y, z.Y, 0.0f),
                new Vector4(x.Z, y.Z, z.Z, 0.0f),
                Vector4.UnitW);

            Rotation = Quaternion.FromRotationMatrix(_rotationMatrix);

            _dirty &= ~(DirtyFlag.Rotation | DirtyFlag.Translation);
        }

        /// <summary>
        /// Looks at the provided <c>IRoxObject</c>. 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="up"></param>
        public virtual void LookAt(IRoxObject obj, Vector3 up) {
            LookAt(obj.Position, up);
        }

        /// <summary>
        /// Updates the rotation, if dirty, for the current object and returns
        /// a status.
        /// </summary>
        /// <returns></returns>
        protected virtual bool UpdateRotation() {
            if (!IsRotationDirty) { 
                return false;
            }

            Rotation = RoxMath.Euler(_xRotation, 0.0f, 0.0f)
                * RoxMath.Euler(0.0f, _yRotation, 0.0f)
                * RoxMath.Euler(0.0f, 0.0f, _zRotation);

            _rotationMatrix = Rotation.Matrix4;
            _dirty &= ~DirtyFlag.Rotation;

            return true;
        }

        /// <summary>
        /// Updates the transform for the current object.
        /// </summary>
        /// <param name="force"></param>
        /// <returns></returns>
        protected virtual bool UpdateTransform(bool force = false) {
            if (!force && !IsTranslationDirty) {
                return false;
            }
            
            LookAt(Position + Forward, Up);

            _dirty &= ~DirtyFlag.Translation;
            return true;
        }
    }
}
