using System;
using System.Collections.Generic;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// A collection of similar parsed data.
    /// </summary>
    /// <typeparam name="T">The type of the parsed data.</typeparam>
    public class ParsedCollection<T> : IEnumerable<T>
    {
        private List<T> result;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">A collection of parsed data.</param>
        public ParsedCollection(IEnumerable<T> collection)
        {
            this.result = new List<T>(collection);
        }

        /// <summary>
        /// Gets the numbers of successfull parsings of data-elements.
        /// </summary>
        public int Count
        {
            get { return result.Count; }
        }

        /// <summary>
        /// Gets the parsed data at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the data to get.</param>
        /// <returns>The parsed element at the specified index.</returns>
        public T this[int index]
        {
            get { return result[index]; }
        }

        /// <summary>
        /// Converts this <see cref="ParsedCollection{T}"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this <see cref="ParsedCollection{T}"/>.</returns>
        public override string ToString()
        {
            return result.Count + " results";
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return result.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return result.GetEnumerator();
        }
    }
}
