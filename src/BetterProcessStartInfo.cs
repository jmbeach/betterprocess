namespace BetterProcess
{
    public class BetterProcessStartInfo: IProcessStartInfo
    {
        public BetterProcessStartInfo()
        {
        }

        public BetterProcessStartInfo(string fileName)
        {
            FileName = fileName;
        }

        public BetterProcessStartInfo(string fileName, string arguments)
        {
            FileName = fileName;
            Arguments = arguments;
        }

        public string FileName { get; set; }
        public string Arguments { get; set; }
        public BetterProcessWindowStyle WindowStyle { get; set; } = BetterProcessWindowStyle.Normal;
    }
}
