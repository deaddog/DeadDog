using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable 1591
namespace DeadDog
{
    public partial class StringParser
    {
        public StringParser CutToFirst(string search, CutDirection dir, bool CutSearch)
        {
            return CutToFirst(search, dir, CutSearch, 0);
        }
        public StringParser CutToFirst(string search, CutDirection dir, bool CutSearch, int add)
        {
            if (dir == CutDirection.Left)
            {
                if (CutSearch)
                    return new StringParser(this.html.Substring(this.html.IndexOf(search) + search.Length + add));
                else
                    return new StringParser(this.html.Substring(this.html.IndexOf(search) + add));
            }
            else
            {
                if (CutSearch)
                    return new StringParser(this.html.Substring(0, this.html.IndexOf(search) - add));
                else
                    return new StringParser(this.html.Substring(0, this.html.IndexOf(search) + search.Length - add));
            }
        }
        public StringParser CutToFirst(char search, CutDirection dir, bool CutSearch)
        {
            return CutToFirst(search, dir, CutSearch, 0);
        }
        public StringParser CutToFirst(char search, CutDirection dir, bool CutSearch, int add)
        {
            if (dir == CutDirection.Left)
            {
                if (CutSearch)
                    return new StringParser(this.html.Substring(this.html.IndexOf(search) + 1 + add));
                else
                    return new StringParser(this.html.Substring(this.html.IndexOf(search) + add));
            }
            else
            {
                int a = this.html.IndexOf("</a>");
                if (CutSearch)
                    return new StringParser(this.html.Substring(0, this.html.IndexOf(search) - add));
                else
                    return new StringParser(this.html.Substring(0, this.html.IndexOf(search) + 1 - add));
            }
        }

        public StringParser CutToLast(string search, CutDirection dir, bool CutSearch)
        {
            return CutToLast(search, dir, CutSearch, 0);
        }
        public StringParser CutToLast(string search, CutDirection dir, bool CutSearch, int add)
        {
            if (dir == CutDirection.Left)
            {
                if (CutSearch)
                    return new StringParser(this.html.Substring(this.html.LastIndexOf(search) + search.Length + add));
                else
                    return new StringParser(this.html.Substring(this.html.LastIndexOf(search) + add));
            }
            else
            {
                if (CutSearch)
                    return new StringParser(this.html.Substring(0, this.html.LastIndexOf(search) - add));
                else
                    return new StringParser(this.html.Substring(0, this.html.LastIndexOf(search) + search.Length - add));
            }
        }
        public StringParser CutToLast(char search, CutDirection dir, bool CutSearch)
        {
            return CutToLast(search, dir, CutSearch, 0);
        }
        public StringParser CutToLast(char search, CutDirection dir, bool CutSearch, int add)
        {
            if (dir == CutDirection.Left)
            {
                if (CutSearch)
                    return new StringParser(this.html.Substring(this.html.LastIndexOf(search) + 1 + add));
                else
                    return new StringParser(this.html.Substring(this.html.LastIndexOf(search) + add));
            }
            else
            {
                if (CutSearch)
                    return new StringParser(this.html.Substring(0, this.html.LastIndexOf(search) - add));
                else
                    return new StringParser(this.html.Substring(0, this.html.LastIndexOf(search) + 1 - add));
            }
        }

        public StringParser CutToSection(string start, string end, bool CutSearch)
        {
            if (CutSearch)
            {
                int a = this.html.IndexOf(start) + start.Length;
                return new StringParser(this.html.Substring(a, this.html.IndexOf(end, a) - a));
            }
            else
            {
                int a = this.html.IndexOf(start);
                return new StringParser(this.html.Substring(a, this.html.IndexOf(end, a + start.Length) + end.Length - a));
            }

            //string s = CutToFirst(this.html, start, CutDirection.Left, CutSearch, 0);
            //return CutToFirst(s, end, CutDirection.Right, CutSearch, 0);
        }

        public StringParser CutToTag(string tag, bool removeTag)
        {
            string startTag1 = "<" + tag + " ";
            string startTag2 = "<" + tag + ">";
            string endTag = "</" + tag + ">";

            int start = innerIndexOf(this.html, startTag1, startTag2);
            int offset = start + 1;
            int tagsOpen = 1;

            int fStart = -1, fEnd = -1;

            if (start == -1 || tag.Length == 0)
                return new StringParser(string.Empty);

            while (tagsOpen > 0)
            {
                fStart = innerIndexOf(this.html, offset, startTag1, startTag2);
                fEnd = this.html.IndexOf(endTag, offset);

                if (fStart == -1 && fEnd == -1)
                    return new StringParser(string.Empty);

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
                return new StringParser(html.Substring(startTagEnd + 1, fEnd - startTagEnd - 1));
            else
                return new StringParser(html.Substring(start, fEnd - start + endTag.Length));
        }

        public StringParser CutToBrackets(Brackets brackets)
        {
            return CutToBrackets(brackets, false);
        }
        public StringParser CutToBrackets(Brackets brackets, bool CutBrackets)
        {
            switch (brackets)
            {
                case Brackets.Round:
                    return CutToBrackets('(', ')', CutBrackets);
                case Brackets.Square:
                    return CutToBrackets('[', ']', CutBrackets);
                case Brackets.Curly:
                    return CutToBrackets('{', '}', CutBrackets);
                case Brackets.Angle:
                    return CutToBrackets('\u3008', '\u3009', CutBrackets);
                case Brackets.Inequality:
                    return CutToBrackets('(', ')', CutBrackets);
                default:
                    throw new ArgumentOutOfRangeException("brackets", "Unknown value '" + brackets.ToString() + "' for parameter 'brackets'.");
            }
        }
        public StringParser CutToBrackets(char first, char last)
        {
            return CutToBrackets(first, last, false);
        }
        public StringParser CutToBrackets(char first, char last, bool CutBrackets)
        {
            StringIndex index = FindBrackets(first, last, !CutBrackets);
            if (index.IsEmpty)
                return new StringParser(string.Empty);
            else
                return new StringParser(this.html.Substring(index.First, index.Length));
            /*
            int start = this.html.IndexOf(first);
            int offset = start + 1;
            int tagsOpen = 1;

            int fStart = -1, fEnd = -1;

            if (start == -1)
                return new StringParser(string.Empty);

            while (tagsOpen > 0)
            {
                fStart = this.html.IndexOf(first, offset);
                fEnd = this.html.IndexOf(last, offset);

                if (fStart == -1 && fEnd == -1)
                    return new StringParser(string.Empty);

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
            if (CutBrackets)
                return new StringParser(html.Substring(start + 1, fEnd - 1));
            else
                return new StringParser(html.Substring(start, fEnd - start + 1));*/
        }

        public StringIndex FirdBrackets(Brackets brackets)
        {
            return FindBrackets(brackets, true);
        }
        public StringIndex FindBrackets(Brackets brackets, bool includeBrackets)
        {
            switch (brackets)
            {
                case Brackets.Round:
                    return FindBrackets('(', ')', includeBrackets);
                case Brackets.Square:
                    return FindBrackets('[', ']', includeBrackets);
                case Brackets.Curly:
                    return FindBrackets('{', '}', includeBrackets);
                case Brackets.Angle:
                    return FindBrackets('\u3008', '\u3009', includeBrackets);
                case Brackets.Inequality:
                    return FindBrackets('(', ')', includeBrackets);
                default:
                    throw new ArgumentOutOfRangeException("brackets", "Unknown value '" + brackets.ToString() + "' for parameter 'brackets'.");
            }
        }
        public StringIndex FindBrackets(char first, char last)
        {
            return FindBrackets(first, last, true);
        }
        public StringIndex FindBrackets(char first, char last, bool includeBrackets)
        {
            int start = this.html.IndexOf(first);
            int offset = start + 1;
            int tagsOpen = 1;

            int fStart = -1, fEnd = -1;

            if (start == -1)
                return StringIndex.Empty;

            while (tagsOpen > 0)
            {
                fStart = this.html.IndexOf(first, offset);
                fEnd = this.html.IndexOf(last, offset);

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

        private int smallestIndex(int a, int b)
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
        private int innerIndexOf(string instring, int offset, string text1, string text2)
        {
            int a = instring.IndexOf(text1, offset);
            int b = instring.IndexOf(text2, offset);
            return smallestIndex(a, b);
        }
        private int innerIndexOf(string instring, string text1, string text2)
        {
            int a = instring.IndexOf(text1);
            int b = instring.IndexOf(text2);
            return smallestIndex(a, b);
        }
    }
}
#pragma warning restore 1591
