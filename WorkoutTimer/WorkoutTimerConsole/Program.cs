using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using WorkoutTimerConsole.Commands;
using WorkoutTimerConsole.Consoles;
using WorkoutTimer.Shared;

namespace WorkoutTimerConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var console = new ConsoleGui();

            try
            {
                var scriptPath = PromptUserForFilePath();

                var commandFactory = new CommandFactory(console);
                var commands = commandFactory.GetCommands(scriptPath).ToList();

                console.DisplayScript(commands);

                console.AskToContinue();

                RunCommands(console, commands);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {

                    console.DisplayException(e);
                }
            }
            catch (Exception e)
            {
                console.DisplayException(e);
            }

            Console.ReadLine();
        }

        static void RunCommands(IGui gui, IEnumerable<IWorkoutCommand> commands)
        {
            var queue = new Queue<IWorkoutCommand>(commands);
            while (queue.Count > 0)
            {
                var command = queue.Dequeue();
                var nextCommand = queue.Count > 0 ? queue.Peek() : new NullCommand();
                gui.UpdateNowAndNext(command, nextCommand);
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
