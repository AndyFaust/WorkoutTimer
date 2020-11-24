using System;
using System.Threading.Tasks;

namespace WorkoutTimerConsole
{
    class NullSound : ISound
    {
        public TimeSpan Duration => new TimeSpan();

        public Task PlayAsync() => Task.CompletedTask;
    }
}
