using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Point = System.Drawing.Point;

namespace Touchless.Multitouch.Devices
{
    internal class PointerManager : IDisposable
    {
        private readonly Dictionary<int, DebugCursor> _pointers;
        // The hook procedure
        private static HookProc hookProc;
        // The hook handle
        private static IntPtr hookHandle = IntPtr.Zero;

        public PointerManager()
        {
            _pointers = new Dictionary<int, DebugCursor>();
            hookProc = new HookProc(MouseHookCallback);
            hookHandle = SetWindowsHookEx(WH_MOUSE, hookProc, (IntPtr)0, GetCurrentThreadId());

            if (hookHandle == IntPtr.Zero)
            {
                MessageBox.Show("Unable to set mouse input hook.");
            }
        }

        public bool BlockMouse { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            if (UnhookWindowsHookEx((IntPtr)hookHandle) == false)
            {
                MessageBox.Show("Unable to unset mouse input hook.");
            }
            else
            {
                hookHandle = IntPtr.Zero;
            }
        }

        #endregion

        public void UpdatePointerPosition(int pointerId, Point position)
        {
            if (_pointers.ContainsKey(pointerId))
            {
                _pointers[pointerId].Location = position;
            }
            else
            {
                var deviceStatus = new DebugCursor();
                _pointers.Add(pointerId, deviceStatus);
                deviceStatus.Show(position);
            }
        }

        public void RemovePointer(int pointerId)
        {
            if (_pointers.ContainsKey(pointerId))
            {
                _pointers[pointerId].Close();
                _pointers.Remove(pointerId);
            }
        }

        private int MouseHookCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (BlockMouse && code >= 0 /*&& WM_MOUSEMOVE == (uint)wParam*/)
            {
                return -1;
            }
            return CallNextHookEx(hookHandle, code, wParam, lParam);
        }

        #region WIN32 Interop

        private const int WH_MOUSE = 7;
        private const int WH_MOUSE_LL = 14;
        private const uint WM_MOUSEMOVE = 0x0200;

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        #endregion WIN32 Interop
    }
}