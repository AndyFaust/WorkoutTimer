using System;
using System.Threading.Tasks;

namespace WorkoutTimer.Shared
{
    public interface ISound
    {
        TimeSpan Duration { get; }
        Task PlayAsync();
    }
}
