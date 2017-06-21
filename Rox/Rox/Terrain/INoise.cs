using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rox.Terrain {

    /// <summary>
    /// An implementation prototype for an object capable of creating noise values based on 
    /// 2D and 3D coordinates.
    /// </summary>
    public interface INoise {

        /// <summary>
        /// 
        /// </summary>
        long Seed { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        double Evaluate(double x, double y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        double Evaluate(double x, double y, double z);
    }
}
