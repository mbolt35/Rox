using System;
using OpenGL;
using Rox.Render;

namespace Rox.Voxel.Render {

    /// <summary>
    /// 
    /// </summary>
    public class ChunkMesh : IRenderable {

        /// <summary>
        /// The chunk represented by this mesh.
        /// </summary>
        private readonly Chunk _chunk;

        /// <summary>
        /// The geometry for the chunk
        /// </summary>
        private VAO _geometry;

        /// <summary>
        /// Whether or not the chunk mesh can be culled by the camera.
        /// </summary>
        public bool CullingEnabled { get; set; } = true;

        /// <summary>
        /// The AABB for this chunk.
        /// </summary>
        public AxisAlignedBoundingBox Bounds { get; }

        /// <summary>
        /// Creates a new <c>ChunkMesh</c> renderable.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="geometry"></param>
        public ChunkMesh(Chunk chunk, VAO geometry) {
            _chunk = chunk;
            _geometry = geometry;

            Bounds = new AxisAlignedBoundingBox(_chunk.World, _chunk.World + Chunk.Dimensions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        public void Draw(Camera camera) {
            var shader = _geometry.Program;
            shader.Use();
            shader["ProjectionMatrix"].SetValue(camera.Projection);
            shader["ViewMatrix"].SetValue(camera.Transform);
            shader["ModelMatrix"].SetValue(Matrix4.Identity);

            _geometry.Draw();
        }

        public void Draw() {
            var shader = _geometry.Program;
            shader.Use();
            shader["ModelMatrix"].SetValue(Matrix4.Identity);

            _geometry.Draw();
        }

        public void Dispose() {
            _geometry.Dispose();
        }
    }
}
