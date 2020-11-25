using System;
using System.Threading.Tasks;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.Sounds
{
    class NullSound : ISound
    {
        public TimeSpan Duration => new TimeSpan();

        public Task PlayAsync() => Task.CompletedTask;
    }
}
