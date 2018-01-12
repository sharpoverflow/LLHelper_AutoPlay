using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


public class Hook
{
    [StructLayout(LayoutKind.Sequential)]
    struct KeyBoardHookStruct
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    [DllImport("user32.dll")]
    static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    static extern bool UnhookWindowsHookEx(int idHook);
    [DllImport("user32.dll")]
    static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
    [DllImport("kernel32.dll")]
    static extern int GetCurrentThreadId();
    [DllImport("kernel32.dll")]
    static extern IntPtr GetModuleHandle(string name);

    public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
    static HookProc KeyBoardHookProcedure;
    public delegate void HookHandle(Keys k);
    static HookHandle hookHandle;
 
    const int WH_KEYBOARD_LL = 13;   

    static int hHook = 0;

    static public bool hookState = false;

    static public void HookRestart(HookHandle hh)
    {
        HookClear();
        hookHandle = hh;
        hookState = true;
        if (hHook == 0)
        {
            KeyBoardHookProcedure = KeyBoardHookProc;
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyBoardHookProcedure,
                    GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
            if (hHook == 0)
            {
                HookClear();
            }
        }
    }

    static public void HookClear()
    {
        bool retKeyboard = true;
        hookState = false;
        if (hHook != 0)
        {
            retKeyboard = UnhookWindowsHookEx(hHook);
            hHook = 0;
        }
    }

    static int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
            Keys k = (Keys)Enum.Parse(typeof(Keys), kbh.vkCode.ToString());
            hookHandle?.Invoke(k);
        }
        return 0;
    }

}