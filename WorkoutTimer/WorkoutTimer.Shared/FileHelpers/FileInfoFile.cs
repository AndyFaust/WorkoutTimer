using System;
using System.Collections.Generic;
using System.IO;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Shared.FileHelpers
{
    class FileInfoFile : IFile
    {
        private readonly FileInfo fileInfo;

        public FileInfoFile(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            fileInfo = new FileInfo(path);

            if (fileInfo.Exists == false)
                throw new FileNotFoundException($"Unable to find file '{path}'.");
        }

        public string Path => fileInfo.FullName;

        public string Directory => fileInfo.DirectoryName;

        public IEnumerable<string> ReadLines()
        {
            return File.ReadLines(Path);
        }
    }
}
