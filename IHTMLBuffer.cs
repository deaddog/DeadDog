using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Defines methods used by an HTML buffer.
    /// </summary>
    public interface IHTMLBuffer
    {
        /// <summary>
        /// Removes a page from the buffer.
        /// </summary>
        /// <param name="page">The page to remove from the buffer.</param>
        /// <returns>A boolean value indicating if the page was removed.</returns>
        bool Remove(HTMLPage page);
    }
}
