using System.IO;
using System.Linq;

namespace FileGeneratorConsoleApp.GeneratorLib
{
    internal class DictionariesReader
    {
        private readonly string _folderPath;

        internal DictionariesReader(string folderPath)
        {
            _folderPath = folderPath;
        }
        
        internal string[] Read()
        {
            var files = Directory.EnumerateFiles(_folderPath).ToArray();
            var lines = files.AsParallel()
                .Select(File.ReadLines)
                .SelectMany(x => x)
                .ToArray();
            return lines;
        }
    }
}