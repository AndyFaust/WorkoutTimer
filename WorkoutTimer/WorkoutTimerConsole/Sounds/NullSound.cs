using System;
using System.Threading.Tasks;

namespace WorkoutTimerConsole.Sounds
{
    class NullSound : ISound
    {
        public TimeSpan Duration => new TimeSpan();

        public Task PlayAsync() => Task.CompletedTask;
    }
}
