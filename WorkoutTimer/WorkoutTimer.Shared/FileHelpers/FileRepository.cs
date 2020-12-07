using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.FileHelpers
{
    internal class FileRepository : IFileRepository
    {
        public IFile GetFile(string filePath)
        {
            return new FileInfoFile(filePath);
        }
    }
}
