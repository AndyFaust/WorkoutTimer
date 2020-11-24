using System;
using System.Collections.Generic;

namespace WorkoutTimerConsole
{
    static class ConsoleHelper
    {
        public static void PressAnyKeyToContinue()
        {
            var message = "Press any key to continue.";
            Console.Write(message);
            Console.ReadKey();
            ClearLine();
        }

        public static void WriteColumns(params string[] entries)
        {
            var columnWidth = Console.WindowWidth / entries.Length;
            var message = "";
            foreach(var entry in entries)
            {
                var numberOfSpaces = Math.Max(columnWidth - entry.Length - 1, 0);

                message += $"{entry}{new string(' ', numberOfSpaces)}";
            }
            Console.WriteLine(message);
        }

        internal static void PrintItems<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

        internal static void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        internal static void WriteAndResetCursor(string message)
        {
            Console.Write(message + new string('\b', message.Length));
        }
    }
}
