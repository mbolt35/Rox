using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using Rox.Core;

namespace Rox.Render {

    public class Camera : RoxObject {

        private Viewport _viewport;
        private float _fieldOfView;
        private float _nearZ;
        private float _farZ;

        private Matrix4 _projection;

        private readonly Frustum _viewFrustum;

        public Viewport Viewport {
            get {
                return _viewport;
            }
            set {
                _viewport = value;

                UpdateViewport();
            }
        }

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
            
            _viewFrustum = new Frustum();

            UpdateViewport();
        }

        /// <summary>
        /// Determines if the <c>IRenderable</c> implementation is visible from the 
        /// camera.
        /// </summary>
        /// <param name="renderable"></param>
        /// <returns></returns>
        public bool IsVisible(IRenderable renderable) {
            // TODO: Implement AxisAlignedBoundingBox IRenderable::Bounds()
            //_viewFrustum.Intersects(renderable.Bounds);
            return true;
        }

        private void UpdateViewport() {
            _projection = Matrix4.CreatePerspectiveFieldOfView(
                _fieldOfView,
                _viewport.AspectRatio,
                _nearZ,
                _farZ);
            _viewFrustum.UpdateFrustum(_projection, Transform);
        }
        
        protected override bool UpdateTransform(bool force = false) {
            var didUpdate = base.UpdateTransform(force);

            if (didUpdate && null != _viewFrustum) {
                _viewFrustum.UpdateFrustum(_projection, _transform);
            }

            return didUpdate;
        }

    }
}
