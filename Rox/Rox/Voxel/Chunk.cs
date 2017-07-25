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
        public const int Width = 32;
        public const int Height = 32;
        public const int Depth = 32;

        public static readonly Vector3 HalfChunk = new Vector3(Width / 2.0f, Height / 2.0f, Depth / 2.0f);

        /// <summary>
        /// Static dimensions for a chunk.
        /// </summary>
        public static readonly Vector3 Dimensions = new Vector3(Width, Height, Depth);

        /// <summary>
        /// Whether or not the chunk runs along a border.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static bool IsChunkBorder(uint x, uint y, uint z) {
            var isBorder = x == 0 || x == Chunk.Width - 1
                || y == 0 || y == Chunk.Height - 1
                || z == 0 || z == Chunk.Depth - 1;

            return isBorder;
        }

        /// <summary>
        /// Creates a bounding box representing the chunk world position + dimensions.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public static AxisAlignedBoundingBox ChunkBoundsFor(Vector3 worldPosition) {
            return new AxisAlignedBoundingBox(worldPosition, worldPosition + Dimensions);
        }

        // Multidimensional Array
        //private readonly Block[,,] _blocks;
        private Array3<Block> _blocks;

        public Vector3 World { get; }
        public AxisAlignedBoundingBox Bounds { get; }

        public Chunk(Vector3 worldPosition) {
            World = worldPosition;
            Bounds = new AxisAlignedBoundingBox(worldPosition, worldPosition + Dimensions);

            _blocks = new Array3<Block>(Width, Height, Depth);
        }

        public Block At(uint x, uint y, uint z) {
            return _blocks[x, y, z];
        }

        public Block At(float x, float y, float z) {
            return At((uint) x, (uint) y, (uint) z);
        }

        public Block At(Vector3 chunkCoordinates) {
            return At(chunkCoordinates.X, chunkCoordinates.Y, chunkCoordinates.Z);
        }

        public void Set(uint x, uint y, uint z, BlockType type) {
            _blocks[x, y, z] = new Block(type) {
                World = World + new Vector3(x, y, z)
            };
        }
    }
}
