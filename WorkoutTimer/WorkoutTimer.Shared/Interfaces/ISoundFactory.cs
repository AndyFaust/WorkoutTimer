namespace WorkoutTimer.Shared.Interfaces
{
    public interface ISoundFactory
    {
        ISound GetSoundFromFile(IFile file);
    }
}
