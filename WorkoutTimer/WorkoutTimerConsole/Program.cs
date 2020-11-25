using System;
using WorkoutTimer.Shared;

namespace WorkoutTimerConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var gui = new ConsoleGui();

            try
            {
                var workoutTimer = new WorkoutTimerLogic(gui);

                var scriptPath = gui.GetScriptFilePath();

                var commands = workoutTimer.ReadScript(scriptPath);

                gui.DisplayScript(commands);

                gui.AskToContinue();

                workoutTimer.RunCommands(commands);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {

                    gui.DisplayException(e);
                }
            }
            catch (Exception e)
            {
                gui.DisplayException(e);
            }

            Console.ReadLine();
        }
    }
}
