using System.Collections.Generic;
using SDL2;

namespace Rox.Util {

    /// <summary>
    /// Helper method for tracking relative mouse position.
    /// </summary>
    public static class RelativeMouse {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public delegate void OnRelativeMouseMove(int x, int y);

        /// <summary>
        /// Internal enabled state.
        /// </summary>
        private static bool _enabled = false;

        /// <summary>
        /// List of mouse move listeners
        /// </summary>
        private static List<OnRelativeMouseMove> _listeners = new List<OnRelativeMouseMove>();

        /// <summary>
        /// Relative mouse positions.
        /// </summary>
        private static int _relativeX;
        private static int _relativeY;

        /// <summary>
        /// 
        /// </summary>
        public static bool Enabled {
            get {
                return _enabled;
            }
            set {
                if (value == _enabled) {
                    return;
                }
                _enabled = value;
                SDL.SDL_SetRelativeMouseMode(_enabled ? SDL.SDL_bool.SDL_TRUE : SDL.SDL_bool.SDL_FALSE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public static void AddListener(OnRelativeMouseMove listener) {
            _listeners.Add(listener);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public static void RemoveListener(OnRelativeMouseMove listener) {
            _listeners.Remove(listener);
        }

        /// <summary>
        /// This updates the relative mouse position, and dispatches events.
        /// </summary>
        public static void HandleEvents() {
            if (!_enabled) {
                return;
            }

            SDL.SDL_GetRelativeMouseState(out _relativeX, out _relativeY);

            if (0 == _relativeX && 0 == _relativeY) {
                return;
            }

            foreach (var listener in _listeners) {
                listener(_relativeX, _relativeY);
            }
        } 
    }
}
