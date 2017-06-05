using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using Rox.Core;

namespace Rox.Render {

    /// <summary>
    /// 
    /// </summary>
    public interface IRenderer {

        string Version { get; }

        void Clear();

        // void Render(Camera camera, IRoxScene scene);
        void Render(Camera camera, IRoxObject obj, VAO geometry);
    }
}
