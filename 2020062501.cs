// PCのプロパティ（Windows 10では「コンピューターの基本的な情報の表示」）を表示する。
//

using System;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            var pidlComputer = default(IntPtr);
            try
            {
                pidlComputer = NativeMethods.SHGetKnownFolderIDList(
                    FOLDERID_ComputerFolder,
                    KF_FLAG_DEFAULT,
                    IntPtr.Zero);

                var sei = new SHELLEXECUTEINFOW
                {
                    cbSize = (uint)Marshal.SizeOf<SHELLEXECUTEINFOW>(),
                    fMask = SEE_MASK_INVOKEIDLIST,
                    lpVerb = "properties",
                    lpIDList = pidlComputer
                };
                NativeMethods.ShellExecuteExW(ref sei);
            }
            finally
            {
                Marshal.FreeCoTaskMem(pidlComputer);
            }
        }

        private static class NativeMethods
        {
            [DllImport("shell32.dll", ExactSpelling = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ShellExecuteExW(
                ref SHELLEXECUTEINFOW pExecInfo);

            [DllImport("shell32.dll", ExactSpelling = true, PreserveSig = false)]
            public static extern IntPtr SHGetKnownFolderIDList(
                [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
                uint dwFlags,
                IntPtr hToken);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHELLEXECUTEINFOW
        {
            public uint cbSize;
            public uint fMask;
            public IntPtr hwnd;
            public string lpVerb;
            public string lpFile;
            public string lpParameters;
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            public IntPtr lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIconOrMonitor;
            public IntPtr hProcess;
        }

        private static readonly Guid FOLDERID_ComputerFolder = Guid.Parse(
            "{0AC0837C-BBF8-452A-850D-79D08E667CA7}");

        private const uint KF_FLAG_DEFAULT = 0;

        private const uint SEE_MASK_INVOKEIDLIST = 0x0000000C;
    }
}

