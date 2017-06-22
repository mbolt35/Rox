using System;

namespace Rox.Util {

    /// <summary>
    /// Static helper utility for enumerations.
    /// </summary>
    public static class Enums {

        /// <summary>
        /// Returns an array of all the enumeration values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] Values<T>() where T : struct, IConvertible {
            if (!typeof(T).IsEnum) {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}