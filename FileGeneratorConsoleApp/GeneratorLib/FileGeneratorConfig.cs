namespace FileGeneratorConsoleApp.GeneratorLib
{
    internal class FileGeneratorConfig
    {
        internal int NumberMinValue { get; }
        internal int NumberMaxValue { get; }
        internal string[] Words { get; }
        internal int Portion { get; }
        internal int TasksCount { get; }
        internal string ResultFilePath { get; }
        internal long ResultFileSize { get; }

        internal FileGeneratorConfig(int numberMinValue, int numberMaxValue, string[] words, 
            int portion, int tasksCount, string resultFilePath, long resultFileSize)
        {
            NumberMinValue = numberMinValue;
            NumberMaxValue = numberMaxValue;
            Words = words;
            Portion = portion;
            TasksCount = tasksCount;
            ResultFilePath = resultFilePath;
            ResultFileSize = resultFileSize;
        }
    }
}