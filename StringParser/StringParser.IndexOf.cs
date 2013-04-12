using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable 1591
namespace DeadDog
{
    public partial class StringParser
    {
        public int IndexOf(char value)
        {
            return html.IndexOf(value);
        }
        public int IndexOf(string value)
        {
            return html.IndexOf(value);
        }
        public int IndexOf(char value, int startIndex)
        {
            return html.IndexOf(value, startIndex);
        }
        public int IndexOf(string value, int startIndex)
        {
            return html.IndexOf(value, startIndex);
        }
        public int IndexOf(string value, StringComparison comparisonType)
        {
            return html.IndexOf(value, comparisonType);
        }
        public int IndexOf(char value, int startIndex, int count)
        {
            return html.IndexOf(value, startIndex, count);
        }
        public int IndexOf(string value, int startIndex, int count)
        {
            return html.IndexOf(value, startIndex, count);
        }
        public int IndexOf(string value, int startIndex, StringComparison comparisonType)
        {
            return html.IndexOf(value, startIndex, comparisonType);
        }
        public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
        {
            return html.IndexOf(value, startIndex, count, comparisonType);
        }

        public int IndexOfTag(string tag, bool removeTag)
        {
            string startTag1 = "<" + tag + " ";
            string startTag2 = "<" + tag + ">";
            string endTag = "</" + tag + ">";

            int start = innerIndexOf(this.html, startTag1, startTag2);
            int offset = start + 1;
            int tagsOpen = 1;

            int fStart = -1, fEnd = -1;

            if (start == -1 || tag.Length == 0)
                return -1;

            while (tagsOpen > 0)
            {
                fStart = innerIndexOf(this.html, offset, startTag1, startTag2);
                fEnd = this.html.IndexOf(endTag, offset);

                if (fStart == -1 && fEnd == -1)
                    return -1;

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
                return startTagEnd + 1;
            else
                return start;
        }
    }
}
#pragma warning restore 1591
