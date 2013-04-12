using System;
using System.Collections.Generic;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Defines a set of methods used to parse a string and thus retriving data.
    /// </summary>
    [Obsolete("All methods defined by StringParser are also defined as extension methods.")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public partial class StringParser //String : IComparable, ICloneable, IConvertible, IComparable<string>, IEnumerable<char>, IEnumerable, IEquatable<string>
    {
        private string html;

        /// <summary>
        /// Initializes a new <see cref="StringParser"/> from an existing <see cref="StringParser"/> by cloning it.
        /// </summary>
        /// <param name="parser">The existing StringParser that should be cloned.</param>
        [Obsolete("All methods defined by StringParser are also defined as extension methods.")]
        public StringParser(StringParser parser)
            : this(parser.html)
        {
        }
        /// <summary>
        /// Initializes a new <see cref="StringParser"/> by downloading the html from a <see cref="URL"/> path.
        /// </summary>
        /// <param name="url">The url from where html data will be downloaded.</param>
        [Obsolete("All methods defined by StringParser are also defined as extension methods.")]
        public StringParser(URL url)
            : this(url.GetHTML())
        {
        }
        /// <summary>
        /// Initializes a new <see cref="StringParser"/> by downloading the html from a <see cref="URL"/> path.
        /// </summary>
        /// <param name="url">The url from where html data will be downloaded.</param>
        /// <param name="encoding">Specifies the encoding that should be used when reading the html data.</param>
        [Obsolete("All methods defined by StringParser are also defined as extension methods.")]
        public StringParser(URL url, Encoding encoding)
            : this(url.GetHTML(encoding))
        {
        }
        /// <summary>
        /// Initializes a new <see cref="StringParser"/> from a string.
        /// </summary>
        /// <param name="html">The string that is passed to the <see cref="StringParser"/> for parsing.</param>
        [Obsolete("All methods defined by StringParser are also defined as extension methods.")]
        public StringParser(string html)
        {
            this.html = html;
        }

        /// <summary>
        /// Returns a value indicating whether the specified <see cref="String"/> object occurs within this <see cref="StringParser"/>.
        /// </summary>
        /// <param name="value">The <see cref="String"/> object to seek.</param>
        /// <returns>true if the value parameter occurs within this <see cref="StringParser"/>, or if value is the empty string (""); otherwise, false.</returns>
        public bool Contains(string value)
        {
            return html.Contains(value);
        }
        /// <summary>
        /// Checks which of to strings "comes first" in this <see cref="StringParser"/>.
        /// </summary>
        /// <param name="a">The first string to check.</param>
        /// <param name="b">The second string to check.</param>
        /// <returns>The index of the first string (0 for a, 1 for b). If both are at the same index, 0 is returned. If neither was found, returns -1.</returns>
        public int First(string a, string b)
        {
            int ia = html.IndexOf(a);
            int ib = html.IndexOf(b);
            if (ia == -1 && ib == -1)
                return -1;
            else if (ia == ib)
                return 0;
            else if ((ia < ib && ia != -1) || ib == -1)//a first
                return 0;
            else
                return 1;
        }
        /// <summary>
        /// Checks which of to strings "comes first" in this <see cref="StringParser"/> and outputs its position.
        /// </summary>
        /// <param name="position">Is set to the position of the first occuring string. Set to -1 is neither where found.</param>
        /// <param name="a">The first string to check.</param>
        /// <param name="b">The second string to check.</param>
        /// <returns>The index of the first string (0 for a, 1 for b). If both are at the same index, 0 is returned. If neither was found, returns -1.</returns>
        public int First(out int position, string a, string b)
        {
            int ia = html.IndexOf(a);
            int ib = html.IndexOf(b);
            if (ia == -1 && ib == -1)
            {
                position = -1;
                return -1;
            }
            else if (ia == ib)
            {
                position = ia;
                return 0;
            }
            else if ((ia < ib && ia != -1) || ib == -1)//a first
            {
                position = ia;
                return 0;
            }
            else
            {
                position = ib;
                return 1;
            }
        }
        /// <summary>
        /// Checks which of an array of strings "comes first" in this <see cref="StringParser"/>.
        /// </summary>
        /// <param name="strings">The strings to check.</param>
        /// <returns>Returns the index of the first string. If none where found, -1 is returned. If <paramref name="strings"/> is empty, -1 is returned.</returns>
        public int First(params string[] strings)
        {
            if (strings.Length == 0)
                return -1;
            int index = -1;
            int min = int.MaxValue;
            for (int i = 0; i < strings.Length; i++)
            {
                int dist = html.IndexOf(strings[i]);
                if (dist < min && dist != -1)
                {
                    min = dist;
                    index = i;
                }
            }
            return index;
        }
        /// <summary>
        /// Checks which of an array of strings "comes first" in this <see cref="StringParser"/> and outputs its position.
        /// </summary>
        /// <param name="position">Is set to the position of the first occuring string. Set to -1 is none of the strings where found.</param>
        /// <param name="strings">The strings to check.</param>
        /// <returns>Returns the index of the first string. If none where found, -1 is returned. If <paramref name="strings"/> is empty, -1 is returned.</returns>
        public int First(out int position, params string[] strings)
        {
            if (strings.Length == 0)
            {
                position = -1;
                return -1;
            }
            int index = -1;
            int min = int.MaxValue;
            for (int i = 0; i < strings.Length; i++)
            {
                int dist = html.IndexOf(strings[i]);
                if (dist < min && dist != -1)
                {
                    min = dist;
                    index = i;
                }
            }
            if (min == int.MaxValue)
                min = -1;
            position = min;
            return index;
        }

        /// <summary>
        /// Gets the character at a specified character position in the current <see cref="StringParser"/> object.
        /// </summary>
        /// <param name="index">A character position in the current parser.</param>
        /// <returns>A Unicode character.</returns>
        public char this[int index]
        {
            get { return this.html[index]; }
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the current <see cref="StringParser"/> object.
        /// </summary>
        /// <returns>The string that remains after all white-space characters are removed from the start and end of the current <see cref="StringParser"/> object.</returns>
        public StringParser Trim()
        {
            return new StringParser(this.html.Trim());
        }
        /// <summary>
        /// Removes all leading and trailing occurrences of a set of characters specified in an array from the current <see cref="StringParser"/> object.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove or null.</param>
        /// <returns>The string that remains after all occurrences of the characters in the trimChars parameter are removed from the start and end of the current <see cref="StringParser"/> object.
        /// If trimChars is null, white-space characters are removed instead.</returns>
        public StringParser Trim(params char[] trimChars)
        {
            return new StringParser(this.html.Trim(trimChars));
        }
        /// <summary>
        /// Removes all trailing occurrences of a set of characters specified in an array from the current <see cref="StringParser"/> object.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove or null.</param>
        /// <returns>The string that remains after all occurrences of the characters in the trimChars parameter are removed from the end of the current <see cref="StringParser"/> object.
        /// If trimChars is null, white-space characters are removed instead.</returns>
        public StringParser TrimEnd(params char[] trimChars)
        {
            return new StringParser(this.html.TrimEnd(trimChars));
        }
        /// <summary>
        /// Removes all leading occurrences of a set of characters specified in an array from the current <see cref="StringParser"/> object.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove or null.</param>
        /// <returns>The string that remains after all occurrences of characters in the trimChars parameter are removed from the start of the current <see cref="StringParser"/> object.
        /// If trimChars is null, white-space characters are removed instead.</returns>
        public StringParser TrimStart(params char[] trimChars)
        {
            return new StringParser(this.html.TrimStart(trimChars));
        }

        /// <summary>
        /// Returns the string contained by this <see cref="StringParser"/>.
        /// </summary>
        /// <returns>The string contained by this <see cref="StringParser"/>.</returns>
        public override string ToString()
        {
            return html;
        }
    }
}
