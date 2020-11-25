using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using WorkoutTimerConsole.Commands;
using WorkoutTimerConsole.Sounds;
using WorkoutTimerConsole.Consoles;

namespace WorkoutTimerConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var console = new ConsoleWrapper();

            try
            {
                var scriptPath = PromptUserForFilePath();

                var commandFactory = new CommandFactory(console);
                var commands = commandFactory.GetCommands(scriptPath).ToList();

                console.WriteLine("Script");
                console.WriteLine("------");
                console.PrintItems(commands);
                console.WriteLine();
                console.PressAnyKeyToContinue();
                console.WriteColumns("Now", "Next");
                console.WriteColumns("---", "----");

                RunCommands(console, commands);

                console.WriteLine("End");
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                console.WriteLine(e.Message);
            }

            console.ReadLine();
        }

        static void RunCommands(IConsole console, IEnumerable<ICommand> commands)
        {
            var queue = new Queue<ICommand>(commands);
            while (queue.Count > 0)
            {
                var command = queue.Dequeue();
                var nextCommand = queue.Count > 0 ? queue.Peek() : new NullCommand();
                console.WriteColumns(command.ToString(), nextCommand.ToString());
                command.Run();
            }
        }

        static string PromptUserForFilePath()
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
