namespace FileSorterConsoleApp.SorterLib
{
    internal class SorterConfig
    {
        internal string UnsortedFilePath { get; }
        internal string ResultFilePath { get; }
        internal int BufferSize { get; }

        internal SorterConfig(string unsortedFilePath, string resultFilePath, int bufferSize)
        {
            UnsortedFilePath = unsortedFilePath;
            ResultFilePath = resultFilePath;
            BufferSize = bufferSize;
        }
    }
}