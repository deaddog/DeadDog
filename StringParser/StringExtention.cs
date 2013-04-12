using System;
using System.Collections.Generic;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Defines additional methods, as extension methods to mutate strings (by constructing new string).
    /// In the documentation of these methods the string on which the operation is performed is referenced as "this instance".
    /// </summary>
    public static class StringExtention
    {
        /// <summary>
        /// Searches for the first occurrence of a substring in this instance and removes everything on the right or left of that search-string.
        /// </summary>
        /// <param name="html">The string in which the substring should be located.</param>
        /// <param name="search">The substring to locate.</param>
        /// <param name="dir">Determines if the left or right part of the string should be removed.</param>
        /// <param name="CutSearch">true, if <paramref name="search"/> should be removed from the result; false, otherwise.</param>
        /// <returns>A copy of <paramref name="html"/> reduced to a substring starting from or ending at the first occurrence of <paramref name="search"/>.</returns>
        public static string CutToFirst(this string html, string search, CutDirection dir, bool CutSearch)
        {
            return CutToFirst(html, search, dir, CutSearch, 0);
        }
        /// <summary>
        /// Searches for the first occurrence of a substring in this instance and removes everything on the right or left of that search-string.
        /// </summary>
        /// <param name="html">The string in which the substring should be located.</param>
        /// <param name="search">The substring to locate.</param>
        /// <param name="dir">Determines if the left or right part of the string should be removed.</param>
        /// <param name="CutSearch">true, if <paramref name="search"/> should be removed from the result; false, otherwise.</param>
        /// <param name="add">Cuts an additional number of characters from the resulting string. These are removed from the end where text is cut.</param>
        /// <returns>A copy of <paramref name="html"/> reduced to a substring starting from or ending at the first occurrence of <paramref name="search"/>.</returns>
        public static string CutToFirst(this string html, string search, CutDirection dir, bool CutSearch, int add)
        {
            int index = html.IndexOf(search);
            if (index == -1)
                throw new ArgumentOutOfRangeException("The provided string \"search\" could not be located.");

            if (dir == CutDirection.Left)
            {
                index += (CutSearch ? search.Length : 0) + add;
                return html.Substring(index);
            }
            else
            {
                index += (CutSearch ? 0 : search.Length) - add;
                return html.Substring(0, index);
            }
        }
        /// <summary>
        /// Searches for the first occurrence of a character in this instance and removes everything on the right or left of that character.
        /// </summary>
        /// <param name="html">The string in which the character should be located.</param>
        /// <param name="search">The character to locate.</param>
        /// <param name="dir">Determines if the left or right part of the string should be removed.</param>
        /// <param name="CutSearch">true, if <paramref name="search"/> should be removed from the result; false, otherwise.</param>
        /// <returns>A copy of <paramref name="html"/> reduced to a substring starting from or ending at the first occurrence of <paramref name="search"/>.</returns>
        public static string CutToFirst(this string html, char search, CutDirection dir, bool CutSearch)
        {
            return CutToFirst(html, search, dir, CutSearch, 0);
        }
        /// <summary>
        /// Searches for the first occurrence of a character in this instance and removes everything on the right or left of that character.
        /// </summary>
        /// <param name="html">The string in which the character should be located.</param>
        /// <param name="search">The character to locate.</param>
        /// <param name="dir">Determines if the left or right part of the string should be removed.</param>
        /// <param name="CutSearch">true, if <paramref name="search"/> should be removed from the result; false, otherwise.</param>
        /// <param name="add">Cuts an additional number of characters from the resulting string. These are removed from the end where text is cut.</param>
        /// <returns>A copy of <paramref name="html"/> reduced to a substring starting from or ending at the first occurrence of <paramref name="search"/>.</returns>
        public static string CutToFirst(this string html, char search, CutDirection dir, bool CutSearch, int add)
        {
            int index = html.IndexOf(search);
            if (index == -1)
                throw new ArgumentOutOfRangeException("The provided string \"search\" could not be located.");

            if (dir == CutDirection.Left)
            {
                index += (CutSearch ? 1 : 0) + add;
                return html.Substring(index);
            }
            else
            {
                index += (CutSearch ? 0 : 1) - add;
                return html.Substring(0, index);
            }
        }

        /// <summary>
        /// Searches for the last occurrence of a substring in this instance and removes everything on the right or left of that search-string.
        /// </summary>
        /// <param name="html">The string in which the substring should be located.</param>
        /// <param name="search">The substring to locate.</param>
        /// <param name="dir">Determines if the left or right part of the string should be removed.</param>
        /// <param name="CutSearch">true, if <paramref name="search"/> should be removed from the result; false, otherwise.</param>
        /// <returns>A copy of <paramref name="html"/> reduced to a substring starting from or ending at the last occurrence of <paramref name="search"/>.</returns>
        public static string CutToLast(this string html, string search, CutDirection dir, bool CutSearch)
        {
            return CutToLast(html, search, dir, CutSearch, 0);
        }
        /// <summary>
        /// Searches for the last occurrence of a substring in this instance and removes everything on the right or left of that search-string.
        /// </summary>
        /// <param name="html">The string in which the substring should be located.</param>
        /// <param name="search">The substring to locate.</param>
        /// <param name="dir">Determines if the left or right part of the string should be removed.</param>
        /// <param name="CutSearch">true, if <paramref name="search"/> should be removed from the result; false, otherwise.</param>
        /// <param name="add">Cuts an additional number of characters from the resulting string. These are removed from the end where text is cut.</param>
        /// <returns>A copy of <paramref name="html"/> reduced to a substring starting from or ending at the last occurrence of <paramref name="search"/>.</returns>
        public static string CutToLast(this string html, string search, CutDirection dir, bool CutSearch, int add)
        {
            int index = html.LastIndexOf(search);
            if (index == -1)
                throw new ArgumentOutOfRangeException("The provided string \"search\" could not be located.");

            if (dir == CutDirection.Left)
            {
                index += (CutSearch ? search.Length : 0) + add;
                return html.Substring(index);
            }
            else
            {
                index += (CutSearch ? 0 : search.Length) - add;
                return html.Substring(0, index);
            }
        }
        /// <summary>
        /// Searches for the last occurrence of a character in this instance and removes everything on the right or left of that character.
        /// </summary>
        /// <param name="html">The string in which the character should be located.</param>
        /// <param name="search">The character to locate.</param>
        /// <param name="dir">Determines if the left or right part of the string should be removed.</param>
        /// <param name="CutSearch">true, if <paramref name="search"/> should be removed from the result; false, otherwise.</param>
        /// <returns>A copy of <paramref name="html"/> reduced to a substring starting from or ending at the last occurrence of <paramref name="search"/>.</returns>
        public static string CutToLast(this string html, char search, CutDirection dir, bool CutSearch)
        {
            return CutToLast(html, search, dir, CutSearch, 0);
        }
        /// <summary>
        /// Searches for the last occurrence of a character in this instance and removes everything on the right or left of that character.
        /// </summary>
        /// <param name="html">The string in which the character should be located.</param>
        /// <param name="search">The character to locate.</param>
        /// <param name="dir">Determines if the left or right part of the string should be removed.</param>
        /// <param name="CutSearch">true, if <paramref name="search"/> should be removed from the result; false, otherwise.</param>
        /// <param name="add">Cuts an additional number of characters from the resulting string. These are removed from the end where text is cut.</param>
        /// <returns>A copy of <paramref name="html"/> reduced to a substring starting from or ending at the last occurrence of <paramref name="search"/>.</returns>
        public static string CutToLast(this string html, char search, CutDirection dir, bool CutSearch, int add)
        {
            int index = html.LastIndexOf(search);
            if (index == -1)
                throw new ArgumentOutOfRangeException("The provided string \"search\" could not be located.");

            if (dir == CutDirection.Left)
            {
                index += (CutSearch ? 1 : 0) + add;
                return html.Substring(index);
            }
            else
            {
                index += (CutSearch ? 0 : 1) - add;
                return html.Substring(0, index);
            }
        }

        /// <summary>
        /// Removes a specified substring from this instance.
        /// </summary>
        /// <param name="html">The string from which the substring should be removed.</param>
        /// <param name="value">The substring to remove.</param>
        /// <returns>A copy of <paramref name="html"/> from which <paramref name="value"/> has been removed.</returns>
        public static string Remove(this string html, string value)
        {
            while (html.Contains(value))
                html = html.Replace(value, "");
            return html;
        }

        /// <summary>
        /// Determines which of two substrings occurs first in a string.
        /// </summary>
        /// <param name="html">The string in which the two substrings should be located.</param>
        /// <param name="a">The first string to locate.</param>
        /// <param name="b">The second string to locate.</param>
        /// <returns>zero, if <paramref name="a"/> comes before (or at the same point as) <paramref name="b"/>; one, if <paramref name="b"/> comes before <paramref name="a"/>; negative one, if neither were found.</returns>
        public static int First(this string html, string a, string b)
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
        /// Determines which of two substrings occurs first in a string.
        /// </summary>
        /// <param name="html">The string in which the two substrings should be located.</param>
        /// <param name="position">When the method returns, contains the index of the first substring if one was contained; otherwise -1.</param>
        /// <param name="a">The first string to locate.</param>
        /// <param name="b">The second string to locate.</param>
        /// <returns>zero, if <paramref name="a"/> comes before (or at the same point as) <paramref name="b"/>; one, if <paramref name="b"/> comes before <paramref name="a"/>; negative one, if neither were found.</returns>
        public static int First(this string html, out int position, string a, string b)
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
        /// Determines which of a range of substrings occurs first in a string.
        /// </summary>
        /// <param name="html">The string in which the substrings should be located.</param>
        /// <param name="strings">The array of strings to locate.</param>
        /// <returns>The index in the array of the string that occurs first, if any; otherwise -1.</returns>
        public static int First(this string html, params string[] strings)
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
        /// Determines which of a range of substrings occurs first in a string.
        /// </summary>
        /// <param name="html">The string in which the substrings should be located.</param>
        /// <param name="position">When the method returns, contains the index of the first substring if one was contained; otherwise -1.</param>
        /// <param name="strings">The array of strings to locate.</param>
        /// <returns>The index in the array of the string that occurs first, if any; otherwise -1</returns>
        public static int First(this string html, out int position, params string[] strings)
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
        /// Searches for the first occurrence of a substring <paramref name="start"/> in this instance. Then cuts from this point to the first occurrence of <paramref name="end"/>.
        /// </summary>
        /// <param name="html">The string in which the substrings should be located.</param>
        /// <param name="start">A string specifying the start of the section that should be returned.</param>
        /// <param name="end">A string specifying the end of the section that should be returned.</param>
        /// <param name="CutSearch">true, if <paramref name="start"/> and <paramref name="end"/> should be removed from the result; false, otherwise.</param>
        /// <returns>The substring from <paramref name="start"/> to <paramref name="end"/> in a string.</returns>
        public static string CutToSection(this string html, string start, string end, bool CutSearch)
        {
            if (CutSearch)
            {
                int a = html.IndexOf(start) + start.Length;
                return html.Substring(a, html.IndexOf(end, a) - a);
            }
            else
            {
                int a = html.IndexOf(start);
                return html.Substring(a, html.IndexOf(end, a + start.Length) + end.Length - a);
            }

            //string s = CutToFirst(html, start, CutDirection.Left, CutSearch, 0);
            //return CutToFirst(s, end, CutDirection.Right, CutSearch, 0);
        }
        /// <summary>
        /// Searches for the first occurrence of a character <paramref name="start"/> in this instance. Then cuts from this point to the first occurrence of <paramref name="end"/>.
        /// </summary>
        /// <param name="html">The string in which the substrings should be located.</param>
        /// <param name="start">A character specifying the start of the section that should be returned.</param>
        /// <param name="end">A character specifying the end of the section that should be returned.</param>
        /// <param name="CutSearch">true, if <paramref name="start"/> and <paramref name="end"/> should be removed from the result; false, otherwise.</param>
        /// <returns>The substring from <paramref name="start"/> to <paramref name="end"/> in a string.</returns>
        public static string CutToSection(this string html, char start, char end, bool CutSearch)
        {
            int a = html.IndexOf(start) + (CutSearch ? 1 : 0);
            if (CutSearch)
                return html.Substring(a, html.IndexOf(end, a) - a);
            else
                return html.Substring(a, html.IndexOf(end, a + 1) + 1 - a);
        }

        // You should consider a SectionIndex type for findsection, findtag and findbrackets.

        //public static string CutToSection(this string html, 

        //public static StringIndex FindSection(this string html, string start, string end, bool CutSearch)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Gets the first instance of a tag (such as &lt;div blah blah &gt;&lt;/div&gt;) in a string. 
        /// The tag can contain other tags.
        /// </summary>
        /// <param name="html">The string in which the tag should be located.</param>
        /// <param name="tag">The name of the tag to locate (eg. "div" for &lt;div&gt;&lt;/div&gt;)</param>
        /// <param name="removeTag">true, if tag start/end should be removed from the return value; otherwise, false.</param>
        /// <returns>A <see cref="String"/> containing the tag and its contents.</returns>
        public static string CutToTag(this string html, string tag, bool removeTag)
        {
            StringIndex index = FindTag(html, tag, removeTag);
            if (index == StringIndex.Empty)
                throw new ArgumentOutOfRangeException("The provided tag could not be found.");
            else
                return html.Substring(index.First, index.Length);
        }
        /// <summary>
        /// Locates the first tag (such as &lt;div blah blah &gt;&lt;/div&gt;) in a string. 
        /// The located tag can contain other tags.
        /// </summary>
        /// <param name="html">The string in which the tag should be located.</param>
        /// <param name="tag">The name of the tag to locate (eg. "div" for &lt;div&gt;&lt;/div&gt;)</param>
        /// <param name="removeTag">true, if tag start/end should be removed from the return value; otherwise, false.</param>
        /// <returns>A <see cref="StringIndex"/> determining the position and length of the tag within the string.</returns>
        public static StringIndex FindTag(this string html, string tag, bool removeTag)
        {
            string startTag1 = "<" + tag + " ";
            string startTag2 = "<" + tag + ">";
            string endTag = "</" + tag + ">";

            int start = innerIndexOf(html, startTag1, startTag2);
            int offset = start + 1;
            int tagsOpen = 1;

            int fStart = -1, fEnd = -1;

            if (start == -1 || tag.Length == 0)
                return StringIndex.Empty;

            while (tagsOpen > 0)
            {
                fStart = innerIndexOf(html, offset, startTag1, startTag2);
                fEnd = html.IndexOf(endTag, offset);

                if (fStart == -1 && fEnd == -1)
                    return StringIndex.Empty;

                if (smallestIndex(fStart, fEnd) == fStart)
                {
                    offset = fStart + 1;
                    tagsOpen++;
                }
                else
                {
                    offset = fEnd + 1;
                    tagsOpen--;
                }
            }
            int startTagEnd = html.IndexOf('>', start);
            if (removeTag)
                return new StringIndex(startTagEnd + 1, fEnd - 1);
            else
                return new StringIndex(start, fEnd - 1 + endTag.Length);
        }

        /// <summary>
        /// Gets the first instance of a set of brackets in a string. 
        /// Searching for '(' and ')' in "(hello (code) world)" would return "hello (code) world".
        /// </summary>
        /// <param name="html">The string in which the brackets should be located.</param>
        /// <param name="brackets">The type of brackets to look for.</param>
        /// <returns>A <see cref="String"/> containing the content inside the brackets.</returns>
        public static string CutToBrackets(this string html, Brackets brackets)
        {
            return CutToBrackets(html, brackets, false);
        }
        /// <summary>
        /// Gets the first instance of a set of brackets in a string.
        /// </summary>
        /// <param name="html">The string in which the brackets should be located.</param>
        /// <param name="brackets">The type of brackets to look for.</param>
        /// <param name="CutBrackets">true, if the brackets should be removed from the return value; otherwise, false.</param>
        /// <returns>A <see cref="String"/> containing the brackets and their content.</returns>
        public static string CutToBrackets(this string html, Brackets brackets, bool CutBrackets)
        {
            switch (brackets)
            {
                case Brackets.Round:
                    return cutToBrackets(html, '(', ')', CutBrackets);
                case Brackets.Square:
                    return cutToBrackets(html, '[', ']', CutBrackets);
                case Brackets.Curly:
                    return cutToBrackets(html, '{', '}', CutBrackets);
                case Brackets.Angle:
                    return cutToBrackets(html, '\u3008', '\u3009', CutBrackets);
                case Brackets.Inequality:
                    return cutToBrackets(html, '(', ')', CutBrackets);
                default:
                    throw new ArgumentOutOfRangeException("brackets", "Unknown value '" + brackets.ToString() + "' for parameter 'brackets'.");
            }
        }
        private static string cutToBrackets(this string html, char first, char last, bool CutBrackets)
        {
            StringIndex index = findBrackets(html, first, last, !CutBrackets);
            if (index.IsEmpty)
                return string.Empty;
            else
                return html.Substring(index.First, index.Length);
        }

        /// <summary>
        /// Locates the first instance of a set of brackets in a string. 
        /// Searching for '(' and ')' in "(hello (code) world)" would return "hello (code) world".
        /// </summary>
        /// <param name="html">The string in which the brackets should be located.</param>
        /// <param name="brackets">The type of brackets to look for.</param>
        /// <returns>A <see cref="StringIndex"/> determining the position of the first and last bracket within the string.</returns>
        public static StringIndex FindBrackets(this string html, Brackets brackets)
        {
            return FindBrackets(html, brackets, true);
        }
        /// <summary>
        /// Locates the first instance of a set of brackets in a string.
        /// </summary>
        /// <param name="html">The string in which the brackets should be located.</param>
        /// <param name="brackets">The type of brackets to look for.</param>
        /// <param name="includeBrackets">true, if the brackets should be included in the return value; otherwise, false.</param>
        /// <returns>A <see cref="StringIndex"/> determining the position of the first and last bracket within the string.</returns>
        public static StringIndex FindBrackets(this string html, Brackets brackets, bool includeBrackets)
        {
            switch (brackets)
            {
                case Brackets.Round:
                    return findBrackets(html, '(', ')', includeBrackets);
                case Brackets.Square:
                    return findBrackets(html, '[', ']', includeBrackets);
                case Brackets.Curly:
                    return findBrackets(html, '{', '}', includeBrackets);
                case Brackets.Angle:
                    return findBrackets(html, '\u3008', '\u3009', includeBrackets);
                case Brackets.Inequality:
                    return findBrackets(html, '(', ')', includeBrackets);
                default:
                    throw new ArgumentOutOfRangeException("brackets", "Unknown value '" + brackets.ToString() + "' for parameter 'brackets'.");
            }
        }
        private static StringIndex findBrackets(this string html, char first, char last, bool includeBrackets)
        {
            if (first == last)
                throw new InvalidOperationException("First and Last bracket cannot be equal.");
            int start = html.IndexOf(first);
            int offset = start + 1;
            int tagsOpen = 1;

            int fStart = -1, fEnd = -1;

            if (start == -1)
                return StringIndex.Empty;

            while (tagsOpen > 0)
            {
                fStart = html.IndexOf(first, offset);
                fEnd = html.IndexOf(last, offset);

                if (fStart == -1 && fEnd == -1)
                    return StringIndex.Empty;

                if (smallestIndex(fStart, fEnd) == fStart)
                {
                    offset = fStart + 1;
                    tagsOpen++;
                }
                else
                {
                    offset = fEnd + 1;
                    tagsOpen--;
                }
            }
            if (includeBrackets)
                return new StringIndex(start, fEnd);
            else
                return new StringIndex(start + 1, fEnd - 1);
        }

        private static int smallestIndex(int a, int b)
        {
            if (a == b)
                return a;
            else if (a == -1)
                return b;
            else if (b == -1)
                return a;
            else
                return a < b ? a : b;
        }
        private static int innerIndexOf(string instring, int offset, string text1, string text2)
        {
            int a = instring.IndexOf(text1, offset);
            int b = instring.IndexOf(text2, offset);
            return smallestIndex(a, b);
        }
        private static int innerIndexOf(string instring, string text1, string text2)
        {
            int a = instring.IndexOf(text1);
            int b = instring.IndexOf(text2);
            return smallestIndex(a, b);
        }
    }
}
