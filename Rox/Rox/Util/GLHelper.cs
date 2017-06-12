using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace Rox.Util {

    public static class GLHelper {
        
        public static string DumpComputeShaderCounts() {
            int[] maxCounts = GetMaxWorkGroupCounts();
            int[] maxSizes = GetMaxWorkGroupSizes();
            int maxInvocations = GetMaxWorkGroupInvocations();

            return $"=== Compute Work Group Maxes ===\n" +
                $"Max Work Group Counts: [{TripletToXYZ(maxCounts)}]\n" +
                $"Max Work Group Sizes: [{TripletToXYZ(maxSizes)}]\n" +
                $"Max Invocations: {maxInvocations}\n" +
                $"================================";
        }

        public static Shader NewShader(string fileName, ShaderType type) {
            try {
                string shaderSource = File.ReadAllText(fileName);
                return new Shader(shaderSource, type);
            }
            catch(Exception e) {
                Console.WriteLine($"Exception loading shader: {e.Message}");
                return null;
            }
        }

        public static ShaderProgram NewShader(string vertShaderFile, string fragShaderFile) {
            var vertShader = NewShader(vertShaderFile, ShaderType.VertexShader);
            var fragShader = NewShader(fragShaderFile, ShaderType.FragmentShader);

            if (null == vertShader || null == fragShader) {
                Console.WriteLine("Failed to create new shader program.");
                return null;
            }

            return new ShaderProgram(vertShader, fragShader) {
                DisposeChildren = true
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxGroupCounts"></param>
        public static int[] GetMaxWorkGroupCounts() {
            int[] maxCounts;
            GetPNameTriplet(GetPName.MaxComputeWorkGroupCount, out maxCounts);
            return maxCounts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxGroupCounts"></param>
        public static int[] GetMaxWorkGroupSizes() {
            int[] maxSizes;
            GetPNameTriplet(GetPName.MaxComputeWorkGroupSize, out maxSizes);
            return maxSizes;
        }

        public static int GetMaxWorkGroupInvocations() {
            return Gl.GetInteger(GetPName.MaxComputeWorkGroupInvocations);
        }

        /// <summary>
        /// Helper function for grabbing integer triplets and pushing to a single int array.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="triplet"></param>
        private static void GetPNameTriplet(GetPName name, out int[] triplet) {
            int[] tmp = new int[1];

            triplet = new int[3];
            for (uint i = 0; i < 3; ++i) {
                Gl.GetIntegeri_v(name, i, tmp);
                triplet[i] = tmp[0];
            }
        }

        private static string TripletToXYZ(int[] triplet) {
            return $"X: {triplet[0]}, Y: {triplet[1]}, Z: {triplet[2]}";
        }
    }
}
