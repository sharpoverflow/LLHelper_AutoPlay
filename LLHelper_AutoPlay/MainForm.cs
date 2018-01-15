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
        private Setting setting;

        private bool simulateState = false;

        private NetPackCatchJsonLL npcj = new NetPackCatchJsonLL();

        private void MainForm_Load(object sender, EventArgs e)
        {
            setting = Setting.Load();

            npcj.onNewJsonAdd += OnNewJsonAdd;
            npcj.onNewHttpAdd += hl.OnNewHttpAdd;
            npcj.onNewHttpLLAdd += hl.OnNewHttpLLAdd;
            llks.onOver += OnPlayOver;
        }

        private void OnPlayOver()
        {
            LogClear();
            Log("结束");
            SimulateOff();
        }

        private void OnNewJsonAdd(string json)
        {

            int l = json.IndexOf("\"live_difficulty_id\"");
            if (l >= 0)
            {
                int r = json.IndexOf(",", l);
                string id = "";
                string tem = json.Substring(l, r - l + 1);
                for (int i = 0; i < tem.Length; i++)
                {
                    id += (tem[i] >= 48 && tem[i] <= 57) ? tem[i].ToString() : "";
                }
                int ldi = int.Parse(id);
                bool b = llks.Reset(ldi);
                Log("当前选择歌曲id: " + ldi);
                if (!b) Log("历史记录中查询不到该歌曲,如果未捕获到谱面将会导致打歌失败");
            }

            if (json.Contains("live_list"))
            {
                MusicNotes mn = Utils.Deserialize<MusicNotes>(json);
                if (mn == null) return;

                Live_info li = mn.response_data.live_list[0].live_info;

                llks.Reset(li);
                Log("谱面捕获 id: " + li.live_difficulty_id);
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

        private void Btn_ShowHttpLog_Click(object sender, EventArgs e)
        {
            hl.ClearList();
            hl.Visible = true;
        }

        private void Btn_Setting_Click(object sender, EventArgs e)
        {
            new SettingForm(setting).Show();
        }

        private void Btn_About_Click(object sender, EventArgs e)
        {
            new AboutForm().Show();
        }

        private void ShowSettingInfo()
        {
            text_Setting.Text = "";//adsad
        }

        private void Log(string text)
        {
            text_Log.Text = DateTime.Now.ToLongTimeString() + " - " + text + "\r\n\r\n" + text_Log.Text;
        }

        private void LogClear()
        {
            text_Log.Text = "";
        }

    }
}
