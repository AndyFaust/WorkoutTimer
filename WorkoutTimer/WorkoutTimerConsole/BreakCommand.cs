using System;

namespace WorkoutTimerConsole
{
    class BreakCommand : ICommand
    {
        public void Run()
        {
            Console.WriteLine("Break. Press space to continue.");
            var key = Console.ReadKey();
            while (!string.IsNullOrWhiteSpace(key.ToString()))
            {
                key = Console.ReadKey();
            }
        }
    }
}
