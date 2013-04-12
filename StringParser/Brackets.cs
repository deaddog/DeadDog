using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Specifies different types of brackets.
    /// </summary>
    public enum Brackets
    {
        /// <summary>
        /// Specifies rounded brackets: '(' and ')'
        /// </summary>
        Round,
        /// <summary>
        /// Specifies square brackets: '[' and ']'
        /// </summary>
        Square,
        /// <summary>
        /// Specifies curly brackets: '{' and '}'
        /// </summary>
        Curly,
        /// <summary>
        /// Specifies angled brackets: '&lt;' and '&gt;'
        /// </summary>
        Angle,
        /// <summary>
        /// Currently specifies ( and ), unknown why :S
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        Inequality
    }
}
