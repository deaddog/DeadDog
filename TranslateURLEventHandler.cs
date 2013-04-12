using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Represents a method that will handle the <see cref="HTMLBuffer{T}.TranslateURL"/> event of a <see cref="HTMLBuffer{T}"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="TranslateURLEventArgs"/> that contains the event data.</param>
    public delegate void TranslateURLEventHandler(object sender, TranslateURLEventArgs e);
}
