using BetterProcess;
using NUnit.Framework;

namespace BetterProcessTests
{
    public class StartProcessTests
    {
        private BetterProcessInfo _info;

        [SetUp]
        public void Setup()
        {
            _info = null;
        }

        [TearDown]
        public void Cleanup()
        {
            if (_info != null && _info.Id > 0)
            {
                System.Diagnostics.Process.GetProcessById(_info.Id).Kill();
            }
        }

        private static void Wait(System.Func<bool> condition, System.Action onFailure = null)
        {
            var startTime = System.DateTime.Now;
            while (!condition() && (System.DateTime.Now - startTime).TotalSeconds < 30)
            {
                System.Threading.Thread.Sleep(250);
                onFailure?.Invoke();
            }
        }
        
        private static string GetCurrentFile()
        {
            var dir = System.AppDomain.CurrentDomain.BaseDirectory;
            var dirInfo = new System.IO.DirectoryInfo(dir);
            // ../.. from bin/debug/netcoreapp
            var parent = dirInfo.Parent.Parent.Parent;
            var file = System.IO.Path.Join(parent.FullName, "/StartProcessTests.cs");
            return file;
        }

        private static void AssertActiveWindow(int pid)
        {
            var foregroundWindow = PInvoke.User32.GetForegroundWindow();
            PInvoke.User32.GetWindowThreadProcessId(foregroundWindow, out int foregroundPid);
            Assert.AreEqual(foregroundPid, pid);
        }

        private static void AssertNotActiveWindow(int pid)
        {
            var foregroundWindow = PInvoke.User32.GetForegroundWindow();
            PInvoke.User32.GetWindowThreadProcessId(foregroundWindow, out int foregroundPid);
            Assert.AreNotEqual(foregroundPid, pid);
        }

        [Test]
        public void StartProcessNormalNoArguments()
        {
            BetterProcessStartInfo info = new BetterProcessStartInfo("notepad.exe");
            var p = new BetterProcess.BetterProcess(info);
            _info = p.Start();
            var process = System.Diagnostics.Process.GetProcessById(_info.Id);
            Assert.AreEqual("notepad", process.ProcessName);
            Wait(() => !string.IsNullOrEmpty(process.MainWindowTitle), () => process = System.Diagnostics.Process.GetProcessById(_info.Id));
            AssertActiveWindow(_info.Id);
        }

        [Test]
        public void StartProcessNormalWithArguments()
        {
            var fileInfo = new System.IO.FileInfo(GetCurrentFile());
            var info = new BetterProcessStartInfo("notepad.exe", fileInfo.FullName);
            var p = new BetterProcess.BetterProcess(info);
            _info = p.Start();
            var process = System.Diagnostics.Process.GetProcessById(_info.Id);
            Assert.AreEqual("notepad", process.ProcessName);
            var title = $"{fileInfo.Name} - Notepad";
            Wait(() => process.MainWindowTitle == title, () => process = System.Diagnostics.Process.GetProcessById(_info.Id));
            Assert.AreEqual(title, process.MainWindowTitle);
            AssertActiveWindow(_info.Id);
        }

        [Test]
        public void StartProcessInactiveNoArguments()
        {
            BetterProcessStartInfo info = new BetterProcessStartInfo("notepad.exe")
            {
                WindowStyle = BetterProcessWindowStyle.Inactive
            };
            var p = new BetterProcess.BetterProcess(info);
            _info = p.Start();
            var process = System.Diagnostics.Process.GetProcessById(_info.Id);
            Wait(() => !string.IsNullOrEmpty(process.MainWindowTitle), () => process = System.Diagnostics.Process.GetProcessById(_info.Id));
            Assert.AreEqual("notepad", process.ProcessName);
            AssertNotActiveWindow(_info.Id);
        }

        [Test]
        public void StartProcessInactiveWithArguments()
        {
            var fileInfo = new System.IO.FileInfo(GetCurrentFile());
            var info = new BetterProcessStartInfo("notepad.exe", fileInfo.FullName)
            {
                WindowStyle = BetterProcessWindowStyle.Inactive
            };
            var p = new BetterProcess.BetterProcess(info);
            _info = p.Start();
            var process = System.Diagnostics.Process.GetProcessById(_info.Id);
            Assert.AreEqual("notepad", process.ProcessName);
            var title = $"{fileInfo.Name} - Notepad";
            Wait(() => process.MainWindowTitle == title, () => process = System.Diagnostics.Process.GetProcessById(_info.Id));
            Assert.AreEqual(title, process.MainWindowTitle);
            AssertNotActiveWindow(_info.Id);
        }
    }
}