using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSorterConsoleApp.SorterLib
{
    internal class Sorter : ISorter
    {
        private readonly SorterConfig _config;

        internal Sorter(SorterConfig config)
        {
            _config = config;
        }

        public void Sort()
        {
            var dict = ReadFile();
            var sorted = SortLines(dict);
            WriteFile(dict, sorted);
        }
        
        private ConcurrentDictionary<string, int> ReadFile()
        {
            var dict = new ConcurrentDictionary<string, int>();
            var lines = new List<string>(_config.BufferSize);

            using (var fStream = File.Open(_config.UnsortedFilePath, FileMode.Open, FileAccess.Read))
            using (var bStream = new BufferedStream(fStream))
            using (var reader = new StreamReader(bStream))
            {
                while (reader.ReadLine() is { } line)
                {
                    lines.Add(line);

                    if (lines.Count == _config.BufferSize)
                    {
                        Parallel.ForEach(lines, (x) => ProcessLine(x, dict));
                        lines = new List<string>(_config.BufferSize);
                    }
                }
            }
            
            Parallel.ForEach(lines, (x) => ProcessLine(x, dict));

            return dict;
        }

        private IOrderedEnumerable<LineInfo> SortLines(ConcurrentDictionary<string, int> dict)
        {
            var bag = new ConcurrentBag<LineInfo>();
            Parallel.ForEach(dict.Keys, (line) => ProcessKey(line, bag));
            
            return bag.OrderBy(x => x.Word)
                .ThenBy(x => x.Number);    
        }
        
        private void WriteFile(ConcurrentDictionary<string, int> dict, IOrderedEnumerable<LineInfo> sorted)
        {
            using (var fStream = File.Open(_config.ResultFilePath, FileMode.Create, FileAccess.Write))
            using (var bStream = new BufferedStream(fStream))
            using (var writer = new StreamWriter(bStream))
            {
                foreach (var lineInfo in sorted)
                {
                    var count = dict[lineInfo.Line];
                    var str = lineInfo.Line + Environment.NewLine;
                    var text = new StringBuilder(str.Length * count)
                        .Insert(0, str, count)
                        .ToString();
                    writer.Write(text);
                }

                writer.Flush();
                writer.Close();
            }
        }
        
        private static void ProcessLine(string line, ConcurrentDictionary<string, int> dict)
        {
            dict.AddOrUpdate(line, 1, (key, oldValue) => oldValue + 1);
        }
        
        private static void ProcessKey(string key, ConcurrentBag<LineInfo> bag)
        {
            var substrings = key.Split(". ");
            var number = int.Parse(substrings[0]);
            var word = substrings[1];
            var lineInfo = new LineInfo(number, word, key);
            bag.Add(lineInfo);
        }
    }
}