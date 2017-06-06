using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using Rox.Core;

namespace Rox.Render.GL {

    public class OpenGLRenderer : IRenderer {

        public string Version {
            get {
                return string.Format(
                    "OpenGL Version: {0}\nGLSL Version: {1}",
                    Gl.GetString(OpenGL.StringName.Version),
                    Gl.GetString(StringName.ShadingLanguageVersion));
            }
        }

        public OpenGLRenderer() {
            Gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.ClearDepthf(1.0f);

            Gl.Enable(EnableCap.Multisample);
            Gl.Enable(EnableCap.DepthTest);
            Gl.DepthFunc(DepthFunction.Lequal);

            Gl.Enable(EnableCap.CullFace);
            Gl.CullFace(CullFaceMode.Back);

            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        public void Clear() {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Render(Camera camera, IRenderable renderable) 
        {
            var viewport = camera.Viewport;
            Gl.Viewport(viewport.X, viewport.Y, viewport.Width, viewport.Height);

            if (camera.IsVisible(renderable)) {
                renderable.Draw(camera);
            }
        }
    }
}
