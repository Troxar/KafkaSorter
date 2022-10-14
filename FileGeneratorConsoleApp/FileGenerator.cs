using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileGeneratorConsoleApp
{
    internal class FileGenerator
    {
        private readonly FileGeneratorConfig _config;
        private readonly object _lock = new object();

        internal FileGenerator(FileGeneratorConfig config)
        {
            _config = config;
        }

        internal void Generate(CancellationToken token)
        {
            if (_config.Words is null || _config.Words.Length == 0)
            {
                throw new NoWordsException("No words to generate file");
            }
            
            var tasks = new Task<int>[_config.TasksCount];
            for (int i = 0; i < _config.TasksCount; i++)
            {
                tasks[i] = Task.Run(PopulateFile, token);
            }

            Task.WaitAll();
            
            for (int i = 0; i < _config.TasksCount; i++)
            {
                Console.WriteLine($"Task[{i}] generated {tasks[i].Result} lines");
            }
        }
        
        private int PopulateFile()
        {
            var random = new Random();
            var maxValue = _config.NumberMaxValue + 1;
            var sb = new StringBuilder();
            var fileInfo = new FileInfo(_config.ResultFilePath);
            int genCount = 0;

            while (IsLineCanBeAdded(fileInfo))
            {
                for (int i = 0; i < _config.Portion; i++)
                {
                    var line = GenerateLine(random.Next(_config.NumberMinValue, maxValue),
                        _config.Words[random.Next(_config.Words.Length)]);
                    sb.AppendLine(line);
                    genCount++;
                }

                lock (_lock)
                {
                    if (IsLineCanBeAdded(fileInfo))
                    {
                        File.AppendAllText(_config.ResultFilePath, sb.ToString());
                    }    
                }
            }

            return genCount;
        }

        private bool IsLineCanBeAdded(FileInfo fileInfo)
        {
            fileInfo.Refresh();
            return !fileInfo.Exists || fileInfo.Length < _config.ResultFileSize;
        }
        
        private string GenerateLine(int number, string word)
        {
            return $"{number}. {word}";
        }
    }
}