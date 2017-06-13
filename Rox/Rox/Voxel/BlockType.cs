using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rox.Voxel {
    
    /// <summary>
    /// The type of block/voxel. 
    /// 
    /// NOTE: Limited to 255 types (byte). Expansion may include sub-types. Be
    /// NOTE: sure to accomodate support for backwards compatibility in file
    /// NOTE: format.
    /// </summary>
    public enum BlockType : byte {
        Air = 0,
        Dirt,
        Wood,
        Stone,
        // 
    }
}
