using System;

namespace Rox.Collections {

    /// <summary>
    /// A generic buffer that must be pre-allocated, and allows copy and reset.
    /// </summary>
    public class Buffer<T> {

        /// <summary>
        /// The buffer storage.
        /// </summary>
        private readonly T[] _buffer;

        /// <summary>
        /// The index of the next location to insert a value, or alternatively
        /// the current number of elements added.
        /// </summary>
        private int _index;

        /// <summary>
        /// The current number of elements pushed into the buffer.
        /// </summary>
        public int Count {
            get { return _index; }
        }

        /// <summary>
        /// The maximum capacity of this buffer. Free to <see cref="Add"/> new elements while 
        /// <see cref="Count"/> is less than this value.
        /// </summary>
        public int MaxCapacity {
            get { return _buffer.Length; }
        }

        /// <summary>
        /// Creates a new <c>Buffer</c> instance of the provided size.
        /// </summary>
        /// <param name="size">The max size of this data structure. This is a <i>fixed</i> size.</param>
        public Buffer(int size) {
            _buffer = new T[size];
        }

        /// <summary>
        /// Adds a new element to the buffer, as long as our <see cref="Count"/> remains less than our 
        /// <see cref="MaxCapacity"/>.
        /// </summary>
        /// <param name="element">The new element to add to our buffer.</param>
        public void Add(T element) {
            _buffer[_index++] = element;
        }

        /// <summary>
        /// This method resets the index pointer.
        /// </summary>
        public void Clear() {
            _index = 0;
        }

        /// <summary>
        /// Creates a new array constrained to the <see cref="Count"/>, containing all added data.
        /// </summary>
        public T[] ToArray() { 
            T[] destination = new T[_index];
            Array.Copy(_buffer, destination, _index);
            return destination;
        }
    }
}
