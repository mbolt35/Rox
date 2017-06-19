using System.Runtime.InteropServices;
using OpenGL;

namespace Rox.Voxel {
    
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)] 
    public struct Block {

        /// <summary>
        /// World coordinate for the block
        /// </summary>
        public Vector3 World;

        /// <summary>
        /// The type of block 
        /// </summary>
        public BlockType BlockType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockType"></param>
        public Block(BlockType blockType) {
            BlockType = blockType;
            World = new Vector3();
        }

        /// <summary>
        /// Equality for Block type
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public bool Equals(Block block) {
            return block.BlockType == BlockType && block.World == World;
        }

        /// <summary>
        /// Equality for a block is based on block type and world.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            return obj is Block && Equals((Block)obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            unchecked {
                return (World.GetHashCode() * 397) ^ (int)BlockType;
            }
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Block left, Block right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Block left, Block right) {
            return !left.Equals(right);
        }
    }
}
