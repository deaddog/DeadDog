using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Represents a method that attempts to parse a string to a different type.
    /// </summary>
    /// <typeparam name="T">The type that the string should be parsed to.</typeparam>
    /// <param name="text">The input string to parse.</param>
    /// <param name="value">When the method returns, contains the parsed value; if parsing was succesfull.
    /// If parsing fails, this value should be disregarded.</param>
    /// <returns>A boolean value indicating weather parsing was succesfull.</returns>
    public delegate bool TryParse<T>(string input, out T value);
}
