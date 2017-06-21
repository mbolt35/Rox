using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using Rox.Collections;

namespace Rox.Geom {

    /// <summary>
    /// 
    /// </summary>
    public class GeometryPool {

        /// <summary>
        /// 
        /// </summary>
        public const int MaxVertices = 98304;

        /// <summary>
        /// 
        /// </summary>
        public const int MaxTriangles = 262144;

        /// <summary>
        /// Vertex buffer storage
        /// </summary>
        private Buffer<Vector3> _vertices = new Buffer<Vector3>(MaxVertices);

        /// <summary>
        /// UV buffer storage
        /// </summary>
        private Buffer<Vector2> _uvs = new Buffer<Vector2>(MaxVertices);

        /// <summary>
        /// Normal buffer storage
        /// </summary>
        private Buffer<Vector3> _normals = new Buffer<Vector3>(MaxVertices);

        /// <summary>
        /// Triangle Buffer
        /// </summary>
        private Buffer<int> _triangles = new Buffer<int>(MaxTriangles);
        
        /// <summary>
        /// Triangle index tally.
        /// </summary>
        private int _triangleCount = 0;

        /// <summary>
        /// Creates a new <c>GeometryPool</c> instance.
        /// </summary>
        public GeometryPool() {

        }

        /// <summary>
        /// Adds the quad face for a block at the position provided, using the uv coordinates and offset.
        /// </summary>
        /// <param name="quad"></param>
        /// <param name="position"></param>
        /// <param name="uv"></param>
        /// <param name="uvOffset"></param>
        public void AddFace(Quad quad, Vector3 position, Vector2 uv, Vector2 uvOffset) {
            Vector3 normal = quad.Normal;
            Vector2 vertUv;

            for (int i = 0; i < 4; ++i) {
                UvVertex quadVert = quad.At(i);
                Vector3 vertPosition = quadVert.Vertex + position;

                ApplyOffset(uv, quadVert.UV, uvOffset, out vertUv);
                AddVertex(vertPosition, vertUv, normal);
            }

            _triangles.Add(_triangleCount + 0);
            _triangles.Add(_triangleCount + 1);
            _triangles.Add(_triangleCount + 2);
            _triangles.Add(_triangleCount + 2);
            _triangles.Add(_triangleCount + 3);
            _triangles.Add(_triangleCount + 0);

            _triangleCount += 4;
        }

        public VAO ToMesh(ShaderProgram shader) {
            Vector3[] verts = _vertices.ToArray();
            Vector2[] uvs = _uvs.ToArray();
            Vector3[] normals = _normals.ToArray();
            int[] triangles = _triangles.ToArray();
            
            var vao = new VAO(
                shader,
                new VBO<Vector3>(verts),
                new VBO<Vector3>(normals),
                new VBO<Vector2>(uvs),
                new VBO<int>(triangles, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
            vao.DisposeChildren = true;
            return vao;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset() {
            _vertices.Clear();
            _uvs.Clear();
            _normals.Clear();
            _triangles.Clear();
            _triangleCount = 0;
        }

        private void AddVertex(Vector3 vertex, Vector2 uv, Vector3 normal) {
            _vertices.Add(vertex);
            _uvs.Add(uv);
            _normals.Add(normal);
        }

        private void ApplyOffset(Vector2 uv, Vector2 baseUv, Vector2 uvOffset, out Vector2 target) {
            target.X = baseUv.X == 0.0f ? uv.X : uv.X + uvOffset.X;
            target.Y = baseUv.Y == 0.0f ? uv.Y : uv.Y + uvOffset.Y;
        }

        
    }
}
