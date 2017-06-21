using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using Rox.Voxel;

namespace Rox.Terrain {

    public class TerrainGenerator {

        private readonly INoise _noise;
        private readonly uint _octaves;
        private readonly float _frequency;
        private readonly float _amplitude;

        public TerrainGenerator( INoise noise,
                                 uint octaves = 6, 
                                 float frequency = 0.25f, 
                                 float amplitude = 0.5f ) 
        {
            _noise = noise;
            _octaves = octaves;
            _frequency = frequency;
            _amplitude = amplitude;
        }

        public Chunk Generate(Vector3 position) {
            const float MaxHeight = 14.0f;

            var chunk = new Chunk(position);

            float scaleX = 0.5f;
            float scaleZ = 0.5f;

            float sx = 1.0f / (Chunk.Width * scaleX);
            float sz = 1.0f / (Chunk.Depth * scaleZ);
            
            for (uint x = 0; x < Chunk.Width; ++x) {
                for (uint z = 0; z < Chunk.Depth; ++z) {
                    float amplitude = _amplitude;
                    float frequency = _frequency;
                    float height = 0.0f;

                    for (uint octave = 0; octave < _octaves; ++octave) {
                        float mx = (position.X + x) * frequency;
                        float mz = (position.Z + z) * frequency;

                        var result = _noise.Evaluate(mx * sx, mz * sz);

                        height += (float)Math.Max(Math.Min(result, 1.0), -1.0) * amplitude;

                        frequency *= 2.0f;
                        amplitude /= 2.0f;
                    }
                    height = (height + 1.0f) / 2.0f;

                    float hy = (float)Math.Round(height * MaxHeight);
                    for (uint y = 0; y < Chunk.Height; ++y) {
                        float my = (position.Y + y);

                        if (my <= hy) {
                            chunk.Set(x, y, z, BlockType.Dirt);
                        } else {
                            chunk.Set(x, y, z, BlockType.Air);
                        }
                    }
                }
            }

            return chunk;
        }

        //

        // User Passes Seed
        // Generate a bunch of chunk noise in Format X
        // Instead of Generating Noise at chunk creation, read from Format X

        // User Passes Seed
        // While generating world, if OnDisk(), read from disk. If Not, generate, save to disk

        /*
        public static void TestNoise(string path, int w = 512, int h = 512) {
            int W = w;
            int H = h;

            var noise = new OpenSimplexNoise(1234567L);
            var bm = new Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            uint octaves = 6;

            float scaleX = 0.25f;
            float scaleY = 0.25f;

            float sx = 1.0f / (W * scaleX);
            float sy = 1.0f / (H * scaleY);

            float maxValue = 0.0f;

            for (int x = 0; x < W; ++x) {
                for (int y = 0; y < H; ++y) {
                    float amplitude = 0.5f;
                    float frequency = 0.25f;
                    float total = 0.0f;

                    for (uint octave = 0; octave < octaves; ++octave) {
                        float mx = x * frequency;
                        float my = y * frequency;

                        var result = noise.Evaluate(mx * sx, my * sy);

                        total += (float)Math.Max(Math.Min(result, 1.0), -1.0) * amplitude;

                        frequency *= 2.0f;
                        amplitude /= 2.0f;
                    }
                    total = (total + 1.0f) / 2.0f;

                    int color = (int)Math.Round(total * 255.0f);
                    bm.SetPixel(x, y, Color.FromArgb(color, color, color));

                    maxValue = Math.Max(maxValue, total);
                }
            }

            if (File.Exists(path)) {
                File.Delete(path);
            }

            bm.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }
        */
    }
}
