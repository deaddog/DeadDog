﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog.Console
{
    /// <summary>
    /// Represents the baseclass for a menu displayed by the <see cref="Console"/> type.
    /// </summary>
    /// <typeparam name="ActionType">The type of actions (delegates) associated with each entry in the menu.</typeparam>
    public abstract class MenuBase<ActionType> where ActionType : class
    {
        private int menuWidth = 60;
        private string title;

        private List<string> texts;
        private List<ActionType> actions;
        private List<ConsoleColor> colors;

        private string cancelText = null;
        private ActionType cancelAction = null;
        private ConsoleColor cancelColor = ConsoleColor.Gray;

        private char paddingChar = '#';

        private MenuLabeling labels = MenuLabeling.Numbers;
        /// <summary>
        /// Gets or sets the type of labeling used when displaying this menu.
        /// </summary>
        public MenuLabeling Labels
        {
            get { return labels; }
            set { labels = value; }
        }

        /// <summary>
        /// Gets or sets the char used to pad the menu.
        /// </summary>
        public char PaddingChar
        {
            get { return paddingChar; }
            set
            {
                if (char.IsControl(value))
                    throw new ArgumentException("Cannot use control-character for padding.");
                paddingChar = value;
            }
        }
        /// <summary>
        /// Gets or sets the title of the menu.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string description = string.Empty;
        /// <summary>
        /// Gets or sets a description-text displayed at the top of the menu.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets a boolean value indicating whether or not the menu has a "cancel" option.
        /// </summary>
        public bool CanCancel
        {
            get { return cancelText != null; }
        }

        private bool? cancelled = null;
        /// <summary>
        /// Gets a boolean value indicating whether or not the cancel option was selected the last time this menu was displayed.
        /// </summary>
        /// <exception cref="InvalidOperationException">Menu has not been displayed.</exception>
        public bool WasCancelled
        {
            get
            {
                if (!cancelled.HasValue)
                    throw new InvalidOperationException("WasCancelled can not be read when menu has not been displayed.");
                return cancelled.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuBase{ActionType}"/> class with a specified title.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        public MenuBase(string title)
        {
            this.title = title;

            this.texts = new List<string>();
            this.actions = new List<ActionType>();
            this.colors = new List<ConsoleColor>();
        }

        /// <summary>
        /// Adds a new option to the menu.
        /// </summary>
        /// <param name="text">The text displayed for the new option.</param>
        /// <param name="action">The action associated with the new option.</param>
        /// <param name="color">The color of the text displayed for the new option.</param>
#if NET3
        public void Add(string text, ActionType action, ConsoleColor color)
#else
        public void Add(string text, ActionType action, ConsoleColor color = ConsoleColor.Gray)
#endif
        {
            this.Add(action, text, color);
        }
        /// <summary>
        /// Adds a new option to the menu.
        /// </summary>
        /// <param name="action">The action associated with the new option.</param>
        /// <param name="text">The text displayed for the new option.</param>
        /// <param name="color">The color of the text displayed for the new option.</param>
#if NET3
        public void Add(ActionType action, string text, ConsoleColor color)
#else
        public void Add(ActionType action, string text, ConsoleColor color = ConsoleColor.Gray)
#endif
        {
            this.texts.Add(text);
            this.actions.Add(action);
            this.colors.Add(color);
        }

        /// <summary>
        /// Sets the cancel option for the menu.
        /// </summary>
        /// <param name="text">The text displayed for the cancel option.</param>
        /// <param name="action">The action associated with the cancel option.</param>
        /// <param name="color">The color of the text displayed for the cancel option.</param>
#if NET3
        public void SetCancel(string text, ActionType action, ConsoleColor color)
#else
        public void SetCancel(string text, ActionType action, ConsoleColor color = ConsoleColor.Gray)
#endif
        {
            SetCancel(action, text, color);
        }
        /// <summary>
        /// Sets the cancel option for the menu.
        /// </summary>
        /// <param name="action">The action associated with the cancel option.</param>
        /// <param name="text">The text displayed for the cancel option.</param>
        /// <param name="color">The color of the text displayed for the cancel option.</param>
#if NET3
        public void SetCancel(ActionType action, string text, ConsoleColor color)
#else
        public void SetCancel(ActionType action, string text, ConsoleColor color = ConsoleColor.Gray)
#endif
        {
            this.cancelText = text;
            this.cancelAction = action;
            this.cancelColor = color;
        }

        /// <summary>
        /// Gets the number of options available in the menu (excluding the cancel option).
        /// </summary>
        public int Count
        {
            get { return texts.Count; }
        }

        /// <summary>
        /// Gets the set of action and text at a specified index.
        /// </summary>
        /// <param name="index">The index at which the action and text is retrieved. If index == <see cref="MenuBase{ActionType}.Count"/>, the cancel option is returned.</param>
        /// <returns>A tuple containing the action and text at the specified index.</returns>
        protected Tuple<ActionType, string> this[int index]
        {
            get
            {
                if (index == actions.Count)
                    return new Tuple<ActionType, string>(cancelAction, cancelText);
                else
                    return new Tuple<ActionType, string>(actions[index], texts[index]);
            }
        }
        /// <summary>
        /// Displays the menu and returns the selected index.
        /// </summary>
        /// <returns>The index of the selected option.</returns>
        protected int ShowAndSelectIndex()
        {
            System.Console.Clear();

            int padL = menuWidth - title.Length;
            int padR = padL;
            padL /= 2;
            padR -= padL;

            int width = menuWidth - 6 - title.Length;
            System.Console.WriteLine(CenterString(title, menuWidth, paddingChar) + "\n");
            WriteLines(description, "  ", menuWidth);

            int zeroPosition = System.Console.CursorTop;
            int cursorPosition = System.Console.CursorTop;
            for (int i = 0; i < texts.Count; i++)
            {
                System.Console.ForegroundColor = colors[i];
                char prefix = prefixFromIndex(i);
                if (prefix == ' ')
                    System.Console.WriteLine("     {1}", prefix, texts[i]);
                else
                    System.Console.WriteLine("  {0}: {1}", prefix, texts[i]);
            }

            if (CanCancel)
            {
                System.Console.ForegroundColor = cancelColor;
                System.Console.WriteLine("  0: " + cancelText);
            }
            System.Console.ResetColor();

            System.Console.WriteLine("\n".PadRight(menuWidth + 1, '#'));


            int restPosition = System.Console.CursorTop;
            System.Console.SetCursorPosition(0, cursorPosition);
            System.Console.Write('>');
            System.Console.SetCursorPosition(0, restPosition);

            int selected = -1;
            while (selected == -1)
            {
                ConsoleKeyInfo key = System.Console.ReadKey(true);
                int keyIndex = indexFromKey(key.KeyChar);
                if (keyIndex < texts.Count)
                {
                    selected = keyIndex;
                    if (selected == -1 && CanCancel)
                        selected = texts.Count;
                }
                else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.UpArrow)
                {
                    int nextPos = key.Key == ConsoleKey.DownArrow ? cursorPosition + 1 : cursorPosition - 1;
                    int lastPos = CanCancel ? texts.Count + zeroPosition : texts.Count + zeroPosition - 1;

                    if (nextPos - zeroPosition < 0)
                        nextPos = lastPos;
                    else if (nextPos > lastPos)
                        nextPos = zeroPosition;
                    System.Console.SetCursorPosition(0, cursorPosition);
                    System.Console.Write(' ');
                    System.Console.SetCursorPosition(0, nextPos);
                    System.Console.Write('>');
                    System.Console.SetCursorPosition(0, restPosition);
                    cursorPosition = nextPos;
                }
                else if (key.Key == ConsoleKey.Enter)
                    selected = cursorPosition - zeroPosition;
                else if (key.Key == ConsoleKey.Escape && CanCancel)
                    selected = texts.Count;
            }

            System.Console.Clear();

            this.cancelled = selected == texts.Count;
            return selected;
        }


        private void WriteLines(string text, string prefix, int width)
        {
            if (text == null || text.Length == 0)
                return;

            text = text.Trim().Replace("\r\n", "\n");
            width = width - prefix.Length;

            List<string> lines = new List<string>(text.Split('\n', '\r'));
            if (lines.Count == 0)
                return;

            int i = 0;
            while (i < lines.Count)
            {
                lines[i] = lines[i].Trim();
                if (lines[i].Length > width)
                {
                    int wid = lines[i].LastIndexOf(' ', width) >= 0 ? lines[i].LastIndexOf(' ', width) : width;

                    lines.Insert(i + 1, lines[i].Substring(wid));
                    lines[i] = lines[i].Substring(0, wid);
                }
                System.Console.WriteLine(prefix + lines[i]);
                i++;
            }
            System.Console.WriteLine();
        }
        private void WriteLine(int index)
        {

        }
        private void WriteCancel()
        {
        }

        private static string CenterString(string text, int width, char pad)
        {
            int padL, padR;
            padR = padL = width - text.Length;
            padL /= 2;
            padR -= padL;

            return "  ".PadLeft(padL, pad) + text + "  ".PadRight(padR, pad);
        }

        private char prefixFromIndex(int index)
        {
            if (index < 0)
                return ' ';

            if (index == Count)
                return '0';

            switch (labels)
            {
                case MenuLabeling.None:
                    return ' ';
                case MenuLabeling.Numbers:
                    return index < 9 ? (char)('1' + index) : ' ';
                case MenuLabeling.Letters:
                    return (index + 'a') <= 'z' ? (char)(index + 'a') : ' ';
                case MenuLabeling.NumbersAndLetters:
                    return index < 9 ? (char)('1' + index) :
                        (index - 9 + 'a') <= 'z' ? (char)(index - 9 + 'a') : ' ';
                default:
                    return ' ';
            }
        }

        private int indexFromKey(char keyChar)
        {
            if (keyChar == '0')
                return -1;

            if(char.IsUpper(keyChar))
                keyChar = char.ToLower(keyChar);

            switch (labels)
            {
                case MenuLabeling.None:
                    return int.MaxValue;
                case MenuLabeling.Numbers:
                    return char.IsNumber(keyChar) ? int.Parse(keyChar.ToString()) - 1 : int.MaxValue;
                case MenuLabeling.Letters:
                    return char.IsLetter(keyChar) ? keyChar - 'a' : int.MaxValue;
                case MenuLabeling.NumbersAndLetters:
                    return char.IsNumber(keyChar) ? int.Parse(keyChar.ToString()) - 1 :
                        char.IsLetter(keyChar) ? keyChar - 'a' + 9 : int.MaxValue;
                default:
                    return int.MaxValue;
            }
        }

#if NET3
        public class Tuple<T1, T2>
        {
            public Tuple(T1 item1, T2 item2)
            {

            }

            public T1 Item1 { get; set; }
            public T2 Item2 { get; set; }
        }
#endif
    }
}
