namespace WorkoutTimer.Shared.Interfaces
{
    public interface IFileRepository
    {
        IFile GetFile(string filePath);
    }
}
