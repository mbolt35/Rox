using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using Rox.Core;

namespace Rox.Render {

    public class Camera : RoxObject {

        private readonly Viewport _viewport;
        private float _fieldOfView;
        private float _nearZ;
        private float _farZ;

        private readonly Matrix4 _projection;

        private readonly Frustum _viewFrustum;

        public Matrix4 Projection {
            get { return _projection; }
        }

        public Frustum View {
            get { return _viewFrustum; }
        }

        public Camera( string name, 
                       Viewport viewport, 
                       float fov = 0.45f, 
                       float nearZ = 0.1f, 
                       float farZ = 1000.0f ) 
            : base(name)
        {
            _viewport = viewport;
            _fieldOfView = fov;
            _nearZ = nearZ;
            _farZ = farZ;

            _projection = Matrix4.CreatePerspectiveFieldOfView(
                _fieldOfView, 
                _viewport.AspectRatio, 
                _nearZ, 
                _farZ);

            _viewFrustum = new Frustum();
            _viewFrustum.UpdateFrustum(_projection, Transform);
        }
        
        protected override bool UpdateTransform(bool force = false) {
            var didUpdate = base.UpdateTransform(force);

            if (didUpdate && null != _viewFrustum) {
                _viewFrustum.UpdateFrustum(_projection, Transform);
            }

            return didUpdate;
        }

    }
}
