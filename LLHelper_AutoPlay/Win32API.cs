using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

public class Win32API
{

    public struct Key32
    {
        #region bVk参数 常量定义

        public const byte Key_LButton = 0x1;    // 鼠标左键
        public const byte Key_RButton = 0x2;    // 鼠标右键
        public const byte Key_Cancel = 0x3;     // CANCEL 键
        public const byte Key_MButton = 0x4;    // 鼠标中键
        public const byte Key_Back = 0x8;       // BACKSPACE 键
        public const byte Key_Tab = 0x9;        // TAB 键
        public const byte Key_Clear = 0xC;      // CLEAR 键
        public const byte Key_Return = 0xD;     // ENTER 键
        public const byte Key_Shift = 0x10;     // SHIFT 键
        public const byte Key_Control = 0x11;   // CTRL 键
        public const byte Key_Alt = 18;         // Alt 键  (键码18)
        public const byte Key_Menu = 0x12;      // MENU 键
        public const byte Key_Pause = 0x13;     // PAUSE 键
        public const byte Key_Capital = 0x14;   // CAPS LOCK 键
        public const byte Key_Escape = 0x1B;    // ESC 键
        public const byte Key_Space = 0x20;     // SPACEBAR 键
        public const byte Key_PageUp = 0x21;    // PAGE UP 键
        public const byte Key_End = 0x23;       // End 键
        public const byte Key_Home = 0x24;      // HOME 键
        public const byte Key_Left = 0x25;      // LEFT ARROW 键
        public const byte Key_Up = 0x26;        // UP ARROW 键
        public const byte Key_Right = 0x27;     // RIGHT ARROW 键
        public const byte Key_Down = 0x28;      // DOWN ARROW 键
        public const byte Key_Select = 0x29;    // Select 键
        public const byte Key_Print = 0x2A;     // PRINT SCREEN 键
        public const byte Key_Execute = 0x2B;   // EXECUTE 键
        public const byte Key_Snapshot = 0x2C;  // SNAPSHOT 键
        public const byte Key_Delete = 0x2E;    // Delete 键
        public const byte Key_Help = 0x2F;      // HELP 键
        public const byte Key_Numlock = 0x90;   // NUM LOCK 键

        //常用键 字母键A到Z
        public const byte Key_A = 65;
        public const byte Key_B = 66;
        public const byte Key_C = 67;
        public const byte Key_D = 68;
        public const byte Key_E = 69;
        public const byte Key_F = 70;
        public const byte Key_G = 71;
        public const byte Key_H = 72;
        public const byte Key_I = 73;
        public const byte Key_J = 74;
        public const byte Key_K = 75;
        public const byte Key_L = 76;
        public const byte Key_M = 77;
        public const byte Key_N = 78;
        public const byte Key_O = 79;
        public const byte Key_P = 80;
        public const byte Key_Q = 81;
        public const byte Key_R = 82;
        public const byte Key_S = 83;
        public const byte Key_T = 84;
        public const byte Key_U = 85;
        public const byte Key_V = 86;
        public const byte Key_W = 87;
        public const byte Key_X = 88;
        public const byte Key_Y = 89;
        public const byte Key_Z = 90;

        //数字键盘0到9
        public const byte Key_0 = 48;    // 0 键
        public const byte Key_1 = 49;    // 1 键
        public const byte Key_2 = 50;    // 2 键
        public const byte Key_3 = 51;    // 3 键
        public const byte Key_4 = 52;    // 4 键
        public const byte Key_5 = 53;    // 5 键
        public const byte Key_6 = 54;    // 6 键
        public const byte Key_7 = 55;    // 7 键
        public const byte Key_8 = 56;    // 8 键
        public const byte Key_9 = 57;    // 9 键


        public const byte Key_Numpad0 = 0x60;    //0 键
        public const byte Key_Numpad1 = 0x61;    //1 键
        public const byte Key_Numpad2 = 0x62;    //2 键
        public const byte Key_Numpad3 = 0x63;    //3 键
        public const byte Key_Numpad4 = 0x64;    //4 键
        public const byte Key_Numpad5 = 0x65;    //5 键
        public const byte Key_Numpad6 = 0x66;    //6 键
        public const byte Key_Numpad7 = 0x67;    //7 键
        public const byte Key_Numpad8 = 0x68;    //8 键
        public const byte Key_Numpad9 = 0x69;    //9 键
        public const byte Key_Multiply = 0x6A;   // MULTIPLICATIONSIGN(*)键
        public const byte Key_Add = 0x6B;        // PLUS SIGN(+) 键
        public const byte Key_Separator = 0x6C;  // ENTER 键
        public const byte Key_Subtract = 0x6D;   // MINUS SIGN(-) 键
        public const byte Key_Decimal = 0x6E;    // DECIMAL POINT(.) 键
        public const byte Key_Divide = 0x6F;     // DIVISION SIGN(/) 键


        //F1到F12按键
        public const byte Key_F1 = 0x70;   //F1 键
        public const byte Key_F2 = 0x71;   //F2 键
        public const byte Key_F3 = 0x72;   //F3 键
        public const byte Key_F4 = 0x73;   //F4 键
        public const byte Key_F5 = 0x74;   //F5 键
        public const byte Key_F6 = 0x75;   //F6 键
        public const byte Key_F7 = 0x76;   //F7 键
        public const byte Key_F8 = 0x77;   //F8 键
        public const byte Key_F9 = 0x78;   //F9 键
        public const byte Key_F10 = 0x79;  //F10 键
        public const byte Key_F11 = 0x7A;  //F11 键
        public const byte Key_F12 = 0x7B;  //F12 键

        #endregion
    }

    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    [DllImport("user32.DLL")]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.DLL")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

}
