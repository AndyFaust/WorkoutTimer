using System;
using System.Threading.Tasks;

namespace WorkoutTimerConsole.Sounds
{
    interface ISound
    {
        TimeSpan Duration { get; }
        Task PlayAsync();
    }
}
