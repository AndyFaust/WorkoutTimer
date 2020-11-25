using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimerConsole
{
    class ConsoleGui : IGui
    {
        private int WindowWidth => Console.WindowWidth - 4;

        public Task AskToContinue()
        {
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            ClearLine();
            return Task.CompletedTask;
        }

        public Task DisplayScript(IEnumerable<IWorkoutCommand> commands)
        {
            Console.WriteLine("Script");
            Console.WriteLine("------");
            PrintItems(commands);
            Console.WriteLine();
            return Task.CompletedTask;
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

        public Task UpdateNowAndNext(IWorkoutCommand now, IWorkoutCommand next)
        {
            WriteColumns($"Now: {now}", $"Next: {next}");
            return Task.CompletedTask;
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

        public Task UpdateTimer(int seconds)
        {
            var secondsStr = seconds.ToString() + new string(' ', 10);
            Console.Write(secondsStr + new string('\b', secondsStr.Length));
            return Task.CompletedTask;
        }

        public string GetScriptFilePath()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "Scripts|*.txt;*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    return openFileDialog.FileName;
                }
                else
                {
                    throw new Exception("No file selected.");
                }
            }
        }
    }
}
