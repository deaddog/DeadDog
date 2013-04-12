using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Provides data for the <see cref="HTMLBuffer{T}.TranslateURL"/> event.
    /// </summary>
    public class TranslateURLEventArgs : EventArgs
    {
        private URL original;
        private URL result;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateURLEventArgs"/> class.
        /// </summary>
        /// <param name="original">The <see cref="URL"/> which is about to be read.</param>
        public TranslateURLEventArgs(URL original)
        {
            this.original = original;
            this.result = original;
        }

        /// <summary>
        /// Gets the <see cref="URL"/> that is being loaded.
        /// </summary>
        public URL Original
        {
            get { return original; }
        }
        /// <summary>
        /// Gets or sets the <see cref="URL"/> that should be considered equivalent to <see cref="Original"/>.
        /// </summary>
        public URL Result
        {
            get { return result; }
            set
            {
                if (value == null)
                    throw new NullReferenceException();
                result = value;
            }
        }
        
        /// <summary>
        /// Converts this <see cref="TranslateURLEventArgs"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this <see cref="TranslateURLEventArgs"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0} Translated into {1}", original.Address, result.Address);
        }
    }
}
