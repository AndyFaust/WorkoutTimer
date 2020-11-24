using System;

namespace WorkoutTimerConsole
{
    class BreakCommand : ICommand
    {
        public void Run()
        {
            ConsoleHelper.PressAnyKeyToContinue();
        }

        public override string ToString()
        {
            return $"Break";
        }
    }
}
