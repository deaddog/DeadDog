using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Represents a the position of a substring.
    /// </summary>
    public struct StringIndex : IEquatable<StringIndex>
    {
        private int first;
        private int last;

        //The boolean value is only used to allow a private constructor
        private static StringIndex empty = new StringIndex(-1, -1, false);
        /// <summary>
        /// Gets an empty <see cref="StringIndex"/> where the first and last index is set to -1.
        /// </summary>
        public static StringIndex Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Gets a boolean value indicating if this <see cref="StringIndex"/> is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return this == empty; }
        }

        private StringIndex(int first, int last, bool Nothing)
        {
            this.first = first;
            this.last = last;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringIndex"/> struct from the position of a character.
        /// </summary>
        /// <param name="index">The first and last index of a character in the substring.</param>
        public StringIndex(int index)
            : this(index, index)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StringIndex"/> struct from the index of the first and the last character in the substring.
        /// </summary>
        /// <param name="first">The first index of a character in the substring.</param>
        /// <param name="last">The last index of a character in the substring.</param>
        public StringIndex(int first, int last)
        {
            if (first < 0 && last >= 0)
                throw new ArgumentOutOfRangeException("last", "When the first index is less than zero, the last must be as well.");
            else if (last < 0 && first >= 0)
                throw new ArgumentOutOfRangeException("first", "When the last index is less than zero, the first must be as well.");
            else if (first > last)
                throw new ArgumentOutOfRangeException("last", "The last index must be greater than or equal to the first.");

            if (first < 0)
            {
                first = -1;
                last = -1;
            }

            this.first = first;
            this.last = last;
        }

        /// <summary>
        /// Gets the zero-based index of the first character.
        /// </summary>
        public int First
        {
            get { return first; }
        }
        /// <summary>
        /// Gets the zero-based index of the last character.
        /// </summary>
        public int Last
        {
            get { return last; }
        }

        /// <summary>
        /// Gets the length of the substring.
        /// </summary>
        public int Length
        {
            get { return last - first + 1; }
        }

        /// <summary>
        /// Compares two <see cref="StringIndex"/> objects for equality.
        /// </summary>
        /// <param name="a">A <see cref="StringIndex"/> to compare.</param>
        /// <param name="b">A <see cref="StringIndex"/> to compare.</param>
        /// <returns>true, if <paramref name="a"/> and <paramref name="b"/> are equal; otherwise, false.</returns>
        public static bool operator ==(StringIndex a, StringIndex b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// Compares two <see cref="StringIndex"/> objects for inequality.
        /// </summary>
        /// <param name="a">A <see cref="StringIndex"/> to compare.</param>
        /// <param name="b">A <see cref="StringIndex"/> to compare.</param>
        /// <returns>false, if <paramref name="a"/> and <paramref name="b"/> are equal; otherwise, true.</returns>
        public static bool operator !=(StringIndex a, StringIndex b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Specifies whether this <see cref="StringIndex"/> contains the same indices as the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to test.</param>
        /// <returns>true, if <paramref name="obj"/> is a <see cref="StringIndex"/> and has the same indices as this <see cref="StringIndex"/>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj.GetType() != typeof(StringIndex))
                return false;

            return Equals((StringIndex)obj);
        }
        /// <summary>
        /// Specifies whether this <see cref="StringIndex"/> contains the same indices as the specified <see cref="StringIndex"/>.
        /// </summary>
        /// <param name="other">The <see cref="StringIndex"/> to test.</param>
        /// <returns>true, if <paramref name="other"/> has the same indices as this <see cref="StringIndex"/>.</returns>
        public bool Equals(StringIndex other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.first == other.first
                && this.last == other.last;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="StringIndex"/>.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this <see cref="StringIndex"/>.</returns>
        public override int GetHashCode()
        {
            return first ^ last;
        }
        /// <summary>
        /// Converts this <see cref="StringIndex"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this <see cref="StringIndex"/>.</returns>
        public override string ToString()
        {
            return "{first = " + first + " last = " + last + "}";
        }
    }
}
