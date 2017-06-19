using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using Rox.Util;

namespace Rox.Voxel {

    /// <summary>
    /// This is our basic block container, unit of world generation, and basic building block for
    /// our voxel world. Parent node for block units.
    /// </summary>
    public class Chunk {
        public const int Width = 16;
        public const int Height = 16;
        public const int Depth = 16;

        // Multidimensional Array
        //private readonly Block[,,] _blocks;
        private Array3<Block> _blocks;

        public Vector3 World { get; private set; }

        public Chunk(Vector3 worldPosition) {
            World = worldPosition;

            _blocks = new Array3<Block>(Width, Height, Depth);
        }

        public Block At(uint x, uint y, uint z) {
            return _blocks[x, y, z];
        }

        public void Set(uint x, uint y, uint z, BlockType type) {
            _blocks[x, y, z] = new Block(type) {
                World = World + new Vector3(x, y, z)
            };
        }
    }
}
