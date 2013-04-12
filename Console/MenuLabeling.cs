using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog.Console
{
    /// <summary>
    /// Defines the labels used when displaying menues.
    /// </summary>
    [Flags]
    public enum MenuLabeling
    {
        /// <summary>
        /// No labeling is used.
        /// </summary>
        None = 0,
        /// <summary>
        /// Numbers (0-9) are used.
        /// </summary>
        Numbers = 1,
        /// <summary>
        /// Letters (a-z) are used.
        /// </summary>
        Letters = 2,
        /// <summary>
        /// Numbers (0-9) and then letters (a-z) are used.
        /// </summary>
        NumbersAndLetters = 3
    }
}
