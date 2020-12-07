using NAudio.Wave;
using System;
using System.Threading.Tasks;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.Sounds
{
    class NaudioSound : ISound
    {
        private readonly IFile file;

        public NaudioSound(IFile file)
        {
            this.file = file ?? throw new ArgumentNullException(nameof(file));

            Duration = GetDuration();
        }

        public TimeSpan Duration { get; }

        public async Task PlayAsync()
        {
            using (var audioFile = new AudioFileReader(file.Path))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(100);
                }
            }
        }
        private TimeSpan GetDuration()
        {
            using (var reader = new AudioFileReader(file.Path))
            {
                return reader.TotalTime;
            }
        }
    }
}
