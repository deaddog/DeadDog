using System;
using System.Collections.Generic;
using System.Text;

namespace DeadDog
{
    public partial class StringParser
    {
        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <returns>A <see cref="StringParser"/> equivalent to the substring that begins at startIndex in this instance, or a <see cref="StringParser"/> with its data set to System.String.Empty if startIndex is equal to the length of this instance.</returns>
        public StringParser Substring(int startIndex)
        {
            return new StringParser(html.Substring(startIndex));
        }
        
        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A <see cref="StringParser"/> equivalent to the substring of length length that begins at startIndex in this instance, or a <see cref="StringParser"/> with its data set to System.String.Empty if startIndex is equal to the length of this instance and length is zero.</returns>
        public StringParser Substring(int startIndex, int length)
        {
            return new StringParser(html.Substring(startIndex, length));
        }
    }
}
