using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rox.Util {

    /// <summary>
    /// Special multidimensional array 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Array3<T> where T : struct {
        private readonly uint _width;
        private readonly uint _height;
        private readonly uint _depth;

        private readonly T[] _data;

        public Array3(uint width, uint height, uint depth) {
            _width = width;
            _height = height;
            _depth = depth;

            _data = new T[width * height * depth];
        }

        public T this[uint x, uint y, uint z] {
            get {
                return _data[x + (y * _width) + (z * _width * _height)];
            }
            set {
                _data[x + (y * _width) + (z * _width * _height)] = value;
            }
        }
    }
}
