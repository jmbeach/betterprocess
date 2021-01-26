using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PInvoke;

namespace BetterProcess
{
    public class BetterProcess
    {
        private readonly BetterProcessStartInfo _info;

        private readonly Dictionary<BetterProcessWindowStyle, ushort> windowStyleMap =
            new Dictionary<BetterProcessWindowStyle, ushort>
            {
                { BetterProcessWindowStyle.Normal, 1 },
                { BetterProcessWindowStyle.Inactive, 4 }
            }; 
        public BetterProcess(BetterProcessStartInfo info)
        {
            _info = info;
        }

        public void Start()
        {
            // Inspired by https://stackoverflow.com/a/19049930/1834329
            var info = new Kernel32.STARTUPINFO();
            info.cb = Marshal.SizeOf(info);
            
            // TODO: Is this needed?
            info.dwFlags = Kernel32.StartupInfoFlags.STARTF_USESHOWWINDOW;

            info.wShowWindow = windowStyleMap[_info.WindowStyle];
            var arguments = _info.Arguments != null ? $" {_info.Arguments}" : string.Empty;
            Kernel32.CreateProcess(null,
                $"{_info.FileName}{arguments}",
                IntPtr.Zero,
                IntPtr.Zero,
                true,
                0,
                IntPtr.Zero,
                null,
                ref info,
                out var processInfo);
            Kernel32.CloseHandle(processInfo.hProcess);
            Kernel32.CloseHandle(processInfo.hThread);
        }
    }
}