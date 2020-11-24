using System;
using System.Threading.Tasks;

namespace WorkoutTimerConsole
{
    interface ISound
    {
        TimeSpan Duration { get; }
        Task PlayAsync();
    }
}
