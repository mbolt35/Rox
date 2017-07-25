using OpenGL;

namespace Rox.Voxel {

    /// <summary>
    /// View class which manages which <see cref="Chunk"/> instances should be loaded 
    /// and unloaded based on player position.
    /// </summary>
    public class ChunkView {

        private int _radius;
        private Vector3 _center;
        private Vector3 _minVisible;
        private Vector3 _maxVisible;

        public readonly Chunk[,,] _chunks;

        public int ViewWidth => _chunks.GetLength(0);

        public int ViewHeight => _chunks.GetLength(1);

        public int ViewDepth => _chunks.GetLength(2);

        public Vector3 Center => _center;
        
        public ChunkView(int radius) {
            var diameter = radius * 2;

            _radius = radius;
            _chunks = new Chunk[diameter, diameter, diameter];
            _center = new Vector3();

            _minVisible = new Vector3(
                (Chunk.Width * _radius) / 2.0f,
                (Chunk.Height * _radius) / 2.0f,
                (Chunk.Depth * _radius) / 2.0f);

            _maxVisible = _minVisible + new Vector3(Chunk.Width, Chunk.Height, Chunk.Depth);
            
        }

        public void Update(Vector3 position) {
            var diameter = _radius * 2;

            var chunkX = (int) position.X / Chunk.Width;
            var chunkY = (int) position.Y / Chunk.Height;
            var chunkZ = (int) position.Z / Chunk.Depth;

            var maxRadius = _radius + 1;

            var startX = chunkX - maxRadius;
            var startY = chunkY - maxRadius;
            var startZ = chunkZ - maxRadius;

            var endX = chunkX + maxRadius;
            var endY = chunkY + maxRadius;
            var endZ = chunkZ + maxRadius;

            for (var x = 0; x < ViewWidth; ++x) {
                for (var y = 0; y < ViewHeight; ++y) {
                    for (var z = 0; z < ViewDepth; ++z) {
                        var chunk = _chunks[x, y, z];
                        

                    }
                }
            }

            /*
            for (var x = 0; x < ViewWidth; ++x) {
                for (var y = 0; y < ViewHeight; ++y) {
                    for (var z = 0; z < ViewDepth; ++z) {
                        var chunk = _chunks[x, y, z];
                        if (null == chunk) {
                            continue;
                        }

                        var v = chunk.World + Chunk.HalfChunk;
                        Vector3 toCheck = (v - position);


                    }
                }
            }
            */
        }
    }
}