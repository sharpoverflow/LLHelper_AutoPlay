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
        private ColorPlayForm cpf;
        private LLKeyboardSimulator llks;
        private Setting setting;
        private NetPackCatchJsonLL npcj = new NetPackCatchJsonLL();

        private bool simulateState = false;

        private void MainForm_Load(object sender , EventArgs e)
        {
            setting = Setting.Load();
            llks = new LLKeyboardSimulator(setting);
            ShowSettingInfo();
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

            int l = json.LastIndexOf("\"live_difficulty_id\"");
            if (l >= 0)
            {
                int r = json.IndexOf("," , l);
                string id = "";
                string tem = json.Substring(l , r - l + 1);
                for (int i = 0 ; i < tem.Length ; i++)
                {
                    id += (tem[i] >= 48 && tem[i] <= 57) ? tem[i].ToString() : "";
                }
                int ldi = int.Parse(id);
                bool b = llks.Reset(ldi);
                Log("当前选择歌曲id: " + ldi);
                if (!b) Log("历史记录中查询不到该歌曲,如果未捕获到谱面将会导致打歌失败");
            }

            //
            //bool isCFEvent = true;
            //isCFEvent &= json.Contains("challenge_info");
            //isCFEvent &= json.Contains("round");
            //应继续搜索live_difficulty_id
            //

            bool isLiveList = false;
            isLiveList |= json.Contains("\"notes_speed\"");
            isLiveList |= json.Contains("\"is_random\"");
            isLiveList &= json.Contains("\"timing_sec\"");
            if (!isLiveList) return;

            if (json.Contains("live_list"))
            {
                MusicNotes mn = Utils.Deserialize<MusicNotes>(json);
                if (mn == null) return;

                Live_info li = mn.response_data.live_list[0].live_info;

                llks.Reset(li);
                Log("谱面捕获 id: " + li.live_difficulty_id);
            }
        }

        private void Btn_HookListen_Click(object sender , EventArgs e)
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
            try
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
                        Win32API.keybd_event(Win32API.Key32.Key_A , 0 , 0 , 0);
                        Win32API.keybd_event(Win32API.Key32.Key_A , 0 , 2 , 0);
                        Win32API.keybd_event(Win32API.Key32.Key_A , 0 , 2 , 0);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                Application.Exit();
            }
        }

        private void SimulateOff()
        {
            if (cpf != null && !cpf.IsDisposed)
            {
                cpf.Stop();
            }

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

            llks.Start();
        }

        private void MainForm_FormClosing(object sender , FormClosingEventArgs e)
        {
            npcj.Shutdown();
            Process.GetCurrentProcess().Kill();
            this.Dispose();
        }

        private void Btn_NetCatch_Click(object sender , EventArgs e)
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

        private void Btn_ShowHttpLog_Click(object sender , EventArgs e)
        {
            hl.ClearList();
            hl.Visible = true;
        }

        private void Btn_ShowSetting_Click(object sender , EventArgs e)
        {
            SettingForm sf = new SettingForm(setting);
            sf.onSave += ShowSettingInfo;
            sf.Show();
        }

        private void Btn_About_Click(object sender , EventArgs e)
        {
            new AboutForm().Show();
        }

        private void ShowSettingInfo()
        {
            string s = "按键配置: [";

            for (byte i = 9 ; i >= 1 ; i--)
            {
                s += (char)setting.pos2key[i];
            }
            s += "]\r\n";
            s += "全局按键: [F6开始][F7停止]\r\n";
            s += "  击键偏移[F8前移][F9后移]\r\n";
            s += $"击键偏移量(tick): {setting.trimValue}\r\n";
            s += $"单击模拟时长(s): {setting.oneKeyLoopTime}\r\n";
            s += $"击键后延时(tick): {setting.keyAfterTickOffset}\r\n";
            s += $"长按击键间隔(ms): {setting.longpressInterval}\r\n";
            s += $"{setting.appName}";
            text_Setting.Text = s;
        }

        private void Log(string text)
        {
            text_Log.Text = DateTime.Now.ToLongTimeString() + " - " + text + "\r\n\r\n" + text_Log.Text;
        }

        private void LogClear()
        {
            text_Log.Text = "";
        }

        private void Btn_ShowColorPlayForm_Click(object sender , EventArgs e)
        {
            if (cpf == null || cpf.IsDisposed)
            {
                cpf = new ColorPlayForm(setting);
                cpf.Show();
            }
        }

        private void Btn_LoadListFromFile_Click(object sender , EventArgs e)
        {
            openFileDialog.Title = "选取谱面数据";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string json = Encoding.UTF8.GetString(File.ReadAllBytes(openFileDialog.FileName));

                    Live_info li = Utils.Deserialize<Live_info>(json);
                    llks.Reset(li);
                    Log("手动载入谱面 id: " + li.live_difficulty_id);
                }
                catch
                {
                    Log("载入失败");
                }
            }
        }

        //数据来源 "http://a.llsif.win/"
        //谱面数据-名称表 "http://r.llsif.win/maps.json"
        //谱面数据-数据表 "http://a.llsif.win/live/json/[live_setting_id=1]"
        private void Btn_GetLivelist_Click(object sender , EventArgs e)
        {
            if (MessageBox.Show("是否从 http://llsif.win 下载谱面数据?" , "下载谱面数据" , MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                folderBrowserDialog.Description = "选择谱面保存位置(文件夹),相同名称文件执行覆盖操作";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    bool isThread = true;
                    string message = "";
                    new Thread(() =>
                    {
                        try
                        {
                            string listJson = Utils.GetWebString("http://r.llsif.win/maps.json");
                            List<RD3Map> rd3Maps = Utils.Deserialize<List<RD3Map>>(listJson);
                            string path = folderBrowserDialog.SelectedPath + "/";

                            string mapJson = Utils.Serialize(rd3Maps);
                            File.WriteAllBytes(path + "0_map.txt" , Encoding.UTF8.GetBytes(mapJson));

                            for (int i = 0 ; i < rd3Maps.Count ; i++)
                            {
                                RD3Map rd3 = rd3Maps[i];
                                string noteJson = Utils.GetWebString("http://a.llsif.win/live/json/" + rd3.live_setting_id);
                                Notes_list[] nl = Utils.Deserialize<Notes_list[]>(noteJson);
                                Live_info li = new Live_info
                                {
                                    is_random = false ,
                                    notes_speed = 0.0f ,
                                    live_difficulty_id = rd3.live_setting_id ,
                                    notes_list = nl
                                };
                                string liveJson = Utils.Serialize(li);
                                string liveName = $"{rd3.live_setting_id.ToString("00000000")}-{rd3.name}-{rd3.difficulty_text}";
                                liveName = liveName.ReplaceToSpace("\\" , "/" , ":" , "*" , "?" , "\"" , "<" , ">" , "|");
                                File.WriteAllBytes(path + liveName + ".txt" , Encoding.UTF8.GetBytes(liveJson));

                                message = i + " / " + rd3Maps.Count;
                            }
                            isThread = false;
                            MessageBox.Show("完成");
                            LogClear();
                        }
                        catch (Exception ex)
                        {
                            isThread = false;
                            Thread.Sleep(300);
                            LogClear();
                            MessageBox.Show("网络异常\r\n" + ex.Message + ex.StackTrace);
                        }
                    }).Start();
                    new Thread(() =>
                    {
                        while (isThread)
                        {
                            LogClear();
                            Log(message);
                            Thread.Sleep(500);
                        }
                    }).Start();
                }
            }
        }


    }
}
