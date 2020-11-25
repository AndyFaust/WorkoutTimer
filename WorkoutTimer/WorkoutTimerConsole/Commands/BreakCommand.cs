using System;

namespace WorkoutTimerConsole.Commands
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
