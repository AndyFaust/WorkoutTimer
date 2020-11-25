using System;
using System.Collections.Generic;
using WorkoutTimer.Shared;

namespace WorkoutTimerConsole.Consoles
{
    class ConsoleGui : IGui
    {
        private int WindowWidth => Console.WindowWidth - 4;

        public void AskToContinue()
        {
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            ClearLine();
        }

        public void DisplayScript(IEnumerable<IWorkoutCommand> commands)
        {
            Console.WriteLine("Script");
            Console.WriteLine("------");
            PrintItems(commands);
            Console.WriteLine();
        }

        private void PrintItems<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public void UpdateNowAndNext(IWorkoutCommand now, IWorkoutCommand next)
        {
            WriteColumns($"Now: {now}", $"Next: {next}");
        }

        private void WriteColumns(params string[] entries)
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
            Console.WriteLine(message);
        }

        public void DisplayException(Exception e)
        {
            Console.WriteLine(e.Message);
        }

        public void UpdateTimer(int seconds)
        {
            var secondsStr = seconds.ToString() + new string(' ', 10);
            Console.Write(secondsStr + new string('\b', secondsStr.Length));
        }
    }
}
