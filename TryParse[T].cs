using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Defines a method that will attempt to parse a string into type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type into which the string should be parsed.</typeparam>
    /// <param name="input">The string to parse.</param>
    /// <param name="value">The result of parsing. If parsing fails, this value should be disregarded.</param>
    /// <returns>True, if parsing was succesfull; otherwise false.</returns>
    public delegate bool TryParse<T>(string input, out T value);
}
