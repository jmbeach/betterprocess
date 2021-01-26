using BetterProcess;
using NUnit.Framework;

namespace BetterProcessTests
{
    public class StartProcessTests
    {
        [SetUp]
        public void Setup()
        {
        }
        
        private static string GetCurrentFile()
        {
            var dir = System.AppDomain.CurrentDomain.BaseDirectory;
            var dirInfo = new System.IO.DirectoryInfo(dir);
            // ../.. from bin/debug
            var parent = dirInfo.Parent.Parent;
            var file = System.IO.Path.Join(parent.FullName, "/StartProcessTests.cs");
            return file;
        }

        [Test]
        public void StartProcessNormalNoArguments()
        {
            BetterProcessStartInfo info = new BetterProcessStartInfo("notepad.exe");
            var p = new BetterProcess.BetterProcess(info);
            p.Start();
            Assert.Fail();
            // TODO: Assert that the notepad window is present
        }

        [Test]
        public void StartProcessNormalWithArguments()
        {
            var info = new BetterProcessStartInfo("notepad.exe", GetCurrentFile());
            var p = new BetterProcess.BetterProcess(info);
            p.Start();
            Assert.Fail();
            // TODO: Assert that the notepad window is present and has file open
        }

        [Test]
        public void StartProcessInactiveNoArguments()
        {
            BetterProcessStartInfo info = new BetterProcessStartInfo("notepad.exe");
            // TODO: Assert that the notepad window is present
            Assert.Fail();
        }

        public void StartProcessInactiveWithArguments()
        {
            // TODO: Assert that the notepad window is present and has file open
            Assert.Fail();
        }
    }
}