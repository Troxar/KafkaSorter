using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileGeneratorConsoleApp.GeneratorLib
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
            
            var tasks = new Task[_config.TasksCount];
            for (int i = 0; i < _config.TasksCount; i++)
            {
                tasks[i] = Task.Run(PopulateFile, token);
            }

            Task.WaitAll(tasks);
        }
        
        private void PopulateFile()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var maxValue = _config.NumberMaxValue + 1;
            var sb = new StringBuilder();
            var fileInfo = new FileInfo(_config.ResultFilePath);

            while (IsLineCanBeAdded(fileInfo))
            {
                for (int i = 0; i < _config.Portion; i++)
                {
                    var line = GenerateLine(random.Next(_config.NumberMinValue, maxValue),
                        _config.Words[random.Next(_config.Words.Length)]);
                    sb.AppendLine(line);
                }

                lock (_lock)
                {
                    if (IsLineCanBeAdded(fileInfo))
                    {
                        File.AppendAllText(_config.ResultFilePath, sb.ToString());
                    }    
                }
                
                sb.Clear();
            }
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