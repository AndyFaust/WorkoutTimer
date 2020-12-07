using System.Collections.Generic;

namespace WorkoutTimer.Shared.Interfaces
{
    public interface IFile
    {
        string Path { get; }
        string Directory { get; }
        IEnumerable<string> ReadLines();
    }
}
