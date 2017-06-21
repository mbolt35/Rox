using OpenGL;

namespace Rox.Util {

    /// <summary>
    /// Contains helper extensions for the <c>OpenGL</c> library.
    /// </summary>
    public static class GLExtensions {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="unit"></param>
        public static void Use(this Texture @this, int unit = 0) {
            Gl.ActiveTexture(unit);
            Gl.BindTexture(@this);
        }
    }
}