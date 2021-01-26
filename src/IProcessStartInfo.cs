namespace BetterProcess
{
    public interface IProcessStartInfo
    {
        string FileName { get; set; }
        string Arguments { get; set; }
    }
}