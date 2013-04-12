using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog.Console
{   
    /// <summary>
    /// Represents a method that attempts to parse a string to a different type.
    /// </summary>
    /// <typeparam name="T">The type that the string should be parsed to.</typeparam>
    /// <param name="text">The input string to parse.</param>
    /// <param name="value">When the method returns, contains the parsed value; if parsing was succesfull.
    /// If parsing fails, this value should be disregarded.</param>
    /// <returns>A boolean value indicating weather parsing was succesfull.</returns>
    public delegate bool TryParse<T>(string text, out T value);

    /// <summary>
    /// Defines method enabling simple input of values directly from <see cref="Console"/>.    
    /// </summary>
    public static class Parsing
    {
        private static bool booleanTryParse(string text, out bool value)
        {
            if (text.Equals("yes", StringComparison.CurrentCultureIgnoreCase) || text.Equals("ja", StringComparison.CurrentCultureIgnoreCase))
            {
                value = true;
                return true;
            }
            else if (text.Equals("no", StringComparison.CurrentCultureIgnoreCase) || text.Equals("nej", StringComparison.CurrentCultureIgnoreCase))
            {
                value = false;
                return true;
            }
            else
            {
                value = true;
                return false;
            }
        }
        
        /// <summary>
        /// Parses a <see cref="string"/> into a boolean value.
        /// "yes" and "ja" returns true; "no" and "nej" returns false.
        /// The method will exit only when input could be parsed (see the GetValue methods).
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <returns>The parsed boolean value.</returns>
        public static bool GetBoolean(this string text)
        {
            return Read<bool>(text, booleanTryParse);
        }
        
        /// <summary>
        /// Parses a <see cref="string"/> into an integer value.
        /// The method will exit only when input could be parsed (see the GetValue methods).
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <returns>The parsed integer value.</returns>
        public static int GetInt32(this string text)
        {
            return Read<int>(text, int.TryParse);
        }
        /// <summary>
        /// Parses a <see cref="string"/> into an integer value.
        /// The method will exit only when input could be parsed (see the GetValue methods).
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <param name="predicate">A method that determines if the parsed integer is valid in the current context.</param>
        /// <returns>The parsed integer value.</returns>
        public static int GetInt32(this string text, Func<int, bool> predicate)
        {
            return Read(text, int.TryParse, predicate);
        }

        private static bool doubleTryParse(string text, out double value)
        {
            return double.TryParse(text.Replace('.', ','), out value);
        }
        /// <summary>
        /// Parses a <see cref="string"/> into an double value.
        /// The method will exit only when input could be parsed (see the GetValue methods).
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <returns>The parsed double value.</returns>
        public static double GetDouble(this string text)
        {
            return Read<double>(text, doubleTryParse);
        }
        /// <summary>
        /// Parses a <see cref="string"/> into an double value.
        /// The method will exit only when input could be parsed (see the GetValue methods).
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <param name="predicate">A method that determines if the parsed double is valid in the current context.</param>
        /// <returns>The parsed double value.</returns>
        public static double GetDouble(this string text, Func<double, bool> predicate)
        {
            return Read(text, doubleTryParse, predicate);
        }

        /// <summary>
        /// Parses a <see cref="string"/> into an <see cref="DateTime"/> value.
        /// The method will exit only when input could be parsed (see the GetValue methods).
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <returns>The parsed <see cref="DateTime"/> value.</returns>
        public static DateTime GetDateTime(this string text)
        {
            return Read<DateTime>(text, DateTime.TryParse);
        }
        /// <summary>
        /// Parses a <see cref="string"/> into an <see cref="DateTime"/> value.
        /// The method will exit only when input could be parsed (see the GetValue methods).
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <param name="predicate">A method that determines if the parsed <see cref="DateTime"/> is valid in the current context.</param>
        /// <returns>The parsed <see cref="DateTime"/> value.</returns>
        public static DateTime GetDateTime(this string text, Func<DateTime, bool> predicate)
        {
            return Read<DateTime>(text, DateTime.TryParse, predicate);
        }

        private static bool stringTryParse(string input, out string output)
        {
            output = input;
            return true;
        }
        /// <summary>
        /// Returns a string from user-input (based on GetValue, but uses no "parsing" from string to string).
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <returns>The string read by the console.</returns>
        public static string GetString(this string text)
        {
            return Read<string>(text, stringTryParse);
        }
        /// <summary>
        /// Returns a string from user-input (based on GetValue, but uses no "parsing" from string to string).
        /// The method will exit only when a predicate is matched.
        /// </summary>
        /// <param name="text">The text to display in <see cref="Console"/> declaring the type of info required.</param>
        /// <param name="predicate">A method that determines if the input string is valid in the current context.</param>
        /// <returns>A string meeting the constraints defined by <paramref name="predicate"/>.</returns>
        public static string GetString(this string text, Func<string, bool> predicate)
        {
            return Read<string>(text, stringTryParse, predicate);
        }

        /// <summary>
        /// Parses a <see cref="string"/> into an object of type <typeparamref name="T"/>.
        /// The method will display <paramref name="text"/> in the console and prompt the user for input.
        /// The method will exit only when input could be parsed.
        /// </summary>
        /// <typeparam name="T">The type that the input should be parsed to.</typeparam>
        /// <param name="text">The text to display in <see cref="Console"/> explaining which type of input is required.</param>
        /// <param name="tryparse">A method capable of parsing console input to an object of type <typeparamref name="T"/>.</param>
        /// <returns>The first item of type <typeparamref name="T"/> that could be parsed.</returns>
        public static T GetValue<T>(this string text, TryParse<T> tryparse)
        {
            return Read<T>(text, tryparse);
        }
        /// <summary>
        /// Parses a <see cref="string"/> into an object of type <typeparamref name="T"/>.
        /// The method will display <paramref name="text"/> in the console and prompt the user for input.
        /// The method will exit only when input could be parsed and the specified predicate is met.
        /// </summary>
        /// <typeparam name="T">The type that the input should be parsed to.</typeparam>
        /// <param name="text">The text to display in <see cref="Console"/> explaining which type of input is required.</param>
        /// <param name="tryparse">A method capable of parsing console input to an object of type <typeparamref name="T"/>.</param>
        /// <param name="predicate">A method determining if the parsed info is valid. If not, that parsing is considered to have failed.</param>
        /// <returns>The first item of type <typeparamref name="T"/> that could be parsed.</returns>
        public static T GetValue<T>(this string text, TryParse<T> tryparse, Func<T, bool> predicate)
        {
            return Read(text, tryparse, predicate);
        }

        private static T Read<T>(string text, TryParse<T> tryparse)
        {
            return Read(text, tryparse, x => true);
        }
        private static T Read<T>(string text, Func<string, T> parse)
        {
            TryParse<T> translated = delegate(string inp, out T value)
            {
                bool success = true;
                try { value = parse(inp); }
                catch { value = default(T); success = false; }
                return success;
            };

            return Read<T>(text, translated);
        }
        private static T Read<T>(string text, TryParse<T> tryparse, Func<T, bool> predicate)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            text = text.Trim();

            System.Console.Write(text);
            char lastSymbol = text[text.Length - 1];
            if (lastSymbol != ':' && lastSymbol != '?')
                System.Console.Write(':');

            System.Console.Write(' ');

            int l = System.Console.CursorLeft, t = System.Console.CursorTop;
            string input = " ";
            T result = default(T);
            bool parsed = false;

            while (!parsed)
            {
                System.Console.SetCursorPosition(l, t);
                System.Console.Write("".PadRight(input.Length, ' '));
                System.Console.SetCursorPosition(l, t);
                input = System.Console.ReadLine();
                parsed = tryparse(input, out result);
                if (parsed)
                    parsed = predicate(result);
            }

            return result;
        }
    }
}
