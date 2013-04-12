using System;
using System.Collections.Generic;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Represents a description of an attempt to parse a string and the parsed data.
    /// </summary>
    /// <typeparam name="T">The type of data parsed to, in this instance.</typeparam>
    public class ParsedInfo<T>
    {
        private bool parsed;
        private T value;
        private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedInfo{T}"/> class from an instance of <typeparamref name="T"/>. No parsing is done.
        /// </summary>
        /// <param name="value">The data associated with the <see cref="ParsedInfo{T}"/>.</param>
        public ParsedInfo(T value)
        {
            parsed = true;
            this.value = value;
            this.exception = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedInfo{T}"/> class from a string and a parsing method.
        /// </summary>
        /// <param name="text">The string that should be parsed.</param>
        /// <param name="parser">The method used for parsing the string.</param>
        public ParsedInfo(string text, TryParse<T> parser)
        {
            this.exception = null;
            try
            {
                parsed = parser(text, out value);
            }
            catch (Exception e)
            {
                this.exception = e;
                this.parsed = false;
                this.value = default(T);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedInfo{T}"/> class from a string and a parsing method.
        /// </summary>
        /// <param name="text">The string that should be parsed.</param>
        /// <param name="parser">The method used for parsing the string. Any exceptions trown by the parser will result in a parsing error.</param>
        public ParsedInfo(string text, Func<string, T> parser)
        {
            try
            {
                this.exception = null;
                this.value = parser(text);
                this.parsed = true;
            }
            catch (Exception e)
            {
                this.exception = e;
                this.value = default(T);
                this.parsed = false;
            }
        }

        /// <summary>
        /// Gets the exception (if any) that was thrown when attempting to parse a string.
        /// If no exception was thrown, this returns null.
        /// </summary>
        public Exception Error
        {
            get { return exception; }
        }
        /// <summary>
        /// Gets a boolean value indicating if parsing was succesfull.
        /// </summary>
        public bool Succes
        {
            get { return parsed; }
        }
        /// <summary>
        /// Gets the parsed data.
        /// If parsing fails, this should be disregarded.
        /// </summary>
        public T Data
        {
            get { return value; }
        }

        /// <summary>
        /// Implicitly converts a <see cref="ParsedInfo{T}"/> to <typeparamref name="T"/> by returning it's enclosed data.
        /// </summary>
        /// <param name="item">The <see cref="ParsedInfo{T}"/> to convert.</param>
        /// <returns>null, if parsing failed; otherwise <see cref="ParsedInfo{T}.Data"/>.</returns>
        public static implicit operator T(ParsedInfo<T> item)
        {
            return item.value;
        }

        /// <summary>
        /// Returns a string representing the result of parsing this instance.
        /// </summary>
        /// <returns>[OK] and value, if parsing was succesfull; [Error] and the exception message, if an exception was thrown; otherwise [N/A]</returns>
        public override string ToString()
        {
            return parsed ? "[OK] " + value.ToString() :
                (this.exception == null ? "[N/A]" : "[Error] " + exception.Message);
        }
    }
}
