using System.Threading;

namespace Rox.Util {

    /// <summary>
    /// Utility class which generates default names based on a prefix and 
    /// incrementing identifier.
    /// </summary>
    public sealed class InstanceNamer {

        private long _ids = 0;
        private readonly string _prefix;

        /// <summary>
        /// Creates a new <c>InstanceNamer</c> using the provided prefix.
        /// </summary>
        /// <param name="prefix"></param>
        public InstanceNamer(string prefix) {
            _prefix = prefix;
        }

        /// <summary>
        /// Creates the next string name, and returns it.
        /// </summary>
        /// <returns></returns>
        public string NextName() {
            return $"{_prefix}-{Interlocked.Increment(ref _ids)}";
        }
    }
}
