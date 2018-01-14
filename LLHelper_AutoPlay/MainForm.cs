using Newtonsoft.Json;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LLHelper_AutoPlay
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private HttpLogForm hl = new HttpLogForm();
        private LLKeyboardSimulator llks = new LLKeyboardSimulator();

        private bool simulateState = false;

        private NetPackCatchJsonLL npcj = new NetPackCatchJsonLL();

        private void MainForm_Load(object sender, EventArgs e)
        {
            npcj.onNewJsonAdd += OnNewJsonAdd;
            npcj.onNewHttpAdd += hl.OnNewHttpAdd;
            npcj.onNewHttpLLAdd += hl.OnNewHttpLLAdd;
            llks.onOver += OnPlayOver;
        }

        private void OnPlayOver()
        {
            text_Log.Text = "";
            SimulateOff();
        }

        private void OnNewJsonAdd(string json)
        {
            if (json.Contains("live_list"))
            {
                MusicNotes mn = Utils.Deserialize<MusicNotes>(json);
                if (mn == null) return;

                string s = "歌曲谱面";
                text_Log.Text = "";
                text_Log.Text += DateTime.Now.ToLongTimeString() + " - " + s + "\r\n";
                
                llks.Reset(mn);
            }
        }

        private void Btn_HookListen_Click(object sender, EventArgs e)
        {
            if (btn_HookListen.Text == "停止监听")
            {
                Hook.HookClear();
                btn_HookListen.Text = "监听按键";
                lab_State_HookListen.Text = "未监听";
                lab_State_HookListen.ForeColor = Color.Red;
            }
            else
            {
                Hook.HookRestart(ListenNumpad);
                btn_HookListen.Text = "停止监听";
                lab_State_HookListen.Text = "监听中";
                lab_State_HookListen.ForeColor = Color.Green;
            }
        }

        private void ListenNumpad(Keys k)
        {
            switch (k)
            {
                case Keys.F7:
                    SimulateOff();
                    break;
                case Keys.F6:
                    SimulateOn();
                    break;
                case Keys.F8:
                    llks.TrimForward();
                    break;
                case Keys.F9:
                    llks.TrimBackward();
                    break;
                case Keys.F3:
                    Win32API.keybd_event(Win32API.Key32.Key_A, 0, 0, 0);
                    Win32API.keybd_event(Win32API.Key32.Key_A, 0, 2, 0);
                    Win32API.keybd_event(Win32API.Key32.Key_A, 0, 2, 0);
                    break;
            }
        }

        private void SimulateOff()
        {
            if (!simulateState) return;
            simulateState = false;

            lab_State_Running.Text = "未运行";
            lab_State_Running.ForeColor = Color.Red;

            llks.Stop();
        }

        private void SimulateOn()
        {
            if (simulateState) return;
            simulateState = true;

            lab_State_Running.Text = "运行中";
            lab_State_Running.ForeColor = Color.Green;

            llks.Init(null);
            llks.Start();
        }



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            npcj.Shutdown();
            Process.GetCurrentProcess().Kill();
            this.Dispose();
        }

        private void Btn_NetCatch_Click(object sender, EventArgs e)
        {
            if (btn_NetCatch.Text == "停止抓包")
            {
                npcj.Shutdown();
                btn_NetCatch.Text = "抓包";

                lab_State_NetCatch.Text = "未抓包";
                lab_State_NetCatch.ForeColor = Color.Red;
            }
            else
            {
                npcj.ShowSelectDeviceWindow();
                npcj.deviceListForm.OnItemSelected += index =>
                {
                    btn_NetCatch.Text = "停止抓包";

                    lab_State_NetCatch.Text = "抓包中";
                    lab_State_NetCatch.ForeColor = Color.Green;
                };
            }
        }

        //mn = Utils.Deserialize<MusicNotes>(textBox3.Text);
        //File.WriteAllText(@"E:\test\lovelive\note list\" 
        //    + "[" + mn.response_data.live_list[0].live_info.live_difficulty_id 
        //    + "][" + DateTime.Now.ToString("yyyyMMddHHmmss") 
        //    + "][" + (mn.response_data.live_list[0].live_info.is_random ? 1 : 0).ToString() 
        //    + "].txt", textBox3.Text);

        //textBox3.Text = "\r\n反序列化成功\r\n\r\n" + textBox3.Text;
        //SimulateOff();


        private void Btn_ShowHttpLog_Click(object sender, EventArgs e)
        {
            hl.ClearList();
            hl.Visible = true;
        }

        private void Btn_Setting_Click(object sender, EventArgs e)
        {

        }

        private void Btn_About_Click(object sender, EventArgs e)
        {

        }
    }
}
