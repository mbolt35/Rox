using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rox.Terrain.Noise {

    /// <summary>
    /// OpenSimplexNoise implementation of <see cref="INoiseFactory"/>
    /// </summary>
    public class OpenSimpleNoiseFactory : INoiseFactory {
        public INoise NewNoise() {
            return new OpenSimplexNoise(DateTime.Now.Ticks);
        }

        public INoise NewNoise(long seed) {
            return new OpenSimplexNoise(seed);
        }
    }
}
