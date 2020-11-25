using System.Collections.Generic;

namespace WorkoutTimerConsole.Consoles
{
    interface IConsole
    {
        void WriteLine();
        void WriteLine(string message);
        string ReadLine();
        void PressAnyKeyToContinue();
        void PrintItems<T>(IEnumerable<T> items);
        void WriteColumns(params string[] entries);
        void WriteAndResetCursor(string message);
    }
}
