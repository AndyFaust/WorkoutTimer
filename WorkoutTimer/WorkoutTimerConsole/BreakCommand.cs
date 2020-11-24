using System;

namespace WorkoutTimerConsole
{
    class BreakCommand : ICommand
    {
        public void Run()
        {
            Console.WriteLine("Break. Press enter to continue.");
            Console.ReadLine();
        }
    }
}
