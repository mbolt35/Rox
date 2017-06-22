using Rox.Util;

namespace Rox.Geom {
    
    /// <summary>
    /// Represents the side of a cube/block.
    /// </summary>
    public enum Side : byte {
        Up = 0,
        Down = 1,
        Right,
        Left,
        Front,
        Back
    }

    /// <summary>
    /// Helper utility for <see cref="Side"/> enumeration.
    /// </summary>
    public static class Sides {

        public static readonly Side[] All = Enums.Values<Side>();
    }
}
