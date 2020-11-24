using System;

namespace WorkoutTimerConsole
{
    class BreakCommand : ICommand
    {
        public void Run()
        {
            Console.WriteLine("Press enter to continue.");
            Console.ReadLine();
        }

        public override string ToString()
        {
            return $"Break";
        }
    }
}
