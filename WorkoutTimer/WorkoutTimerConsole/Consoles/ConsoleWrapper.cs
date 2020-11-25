using System;
using System.Collections.Generic;

namespace WorkoutTimerConsole.Consoles
{
    class ConsoleWrapper : IConsole
    {
        private int WindowWidth => Console.WindowWidth - 4;

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void PressAnyKeyToContinue()
        {
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            ClearLine();
        }

        public void PrintItems<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                WriteLine(item.ToString());
            }
        }

        private void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public void WriteColumns(params string[] entries)
        {
            var columnWidth = WindowWidth / entries.Length;
            var message = "";
            foreach (var entry in entries)
            {
                if (columnWidth > entry.Length)
                    message += $"{entry}{new string(' ', columnWidth - entry.Length)}";
                else
                    message += entry.Substring(0, columnWidth);
            }
            WriteLine(message);
        }

        public void WriteAndResetCursor(string message)
        {
            Console.Write(message + new string('\b', message.Length));
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
