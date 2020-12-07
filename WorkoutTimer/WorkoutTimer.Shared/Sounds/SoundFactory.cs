using System;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.Sounds
{
    internal class SoundFactory : ISoundFactory
    {
        public ISound GetSoundFromFile(IFile file)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));

            return new NaudioSound(file);
        }
    }
}
