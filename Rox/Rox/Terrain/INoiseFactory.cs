using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rox.Terrain {
    
    /// <summary>
    /// 
    /// </summary>
    public interface INoiseFactory {

        /// <summary>
        /// Creates a new <see cref="INoise"/> implementation using a date-based seed.
        /// </summary>
        /// <returns></returns>
        INoise NewNoise();

        /// <summary>
        /// Creates a new <see cref="INoise"/> implementation using the provided seed.
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        INoise NewNoise(long seed);
    }
}
