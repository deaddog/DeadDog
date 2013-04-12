using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable 1591
namespace DeadDog
{
    public partial class StringParser
    {
        public StringParser Replace(char oldChar, char newChar)
        {
            return new StringParser(this.html.Replace(oldChar, newChar));
        }
        public StringParser Replace(string oldValue, string newValue)
        {
            return new StringParser(this.html.Replace(oldValue, newValue));
        }
    }
}
#pragma warning restore 1591
