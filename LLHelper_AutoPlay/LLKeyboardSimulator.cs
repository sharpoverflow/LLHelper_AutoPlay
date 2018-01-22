using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LLHelper_AutoPlay
{
    public class LLKeyboardSimulator
    {
        const string LiveDifficultyPath = "LiveDifficulty";

        const double e7 = 10000000.000;

        public delegate void OnOver();
        public OnOver onOver;

        private Live_info li;

        private bool isRun = false;
        private long startTick;

        private Setting setting;

        public LLKeyboardSimulator(Setting setting)
        {
            this.setting = setting;
        }

        private void TimeLine(object obj)
        {
            Notes_list[] nl = obj as Notes_list[];
            for (int i = 0; i < nl.Length; i++)
            {
                nl[i].notes_attribute = 1;
            }
            startTick = DateTime.Now.Ticks;
            int t = 0;
            while (isRun)
            {
                for (int i = t; i < nl.Length; i++)
                {
                    if (nl[i].notes_attribute == 1)
                    {
                        if (nl[i].timing_sec <= (DateTime.Now.Ticks - startTick) / e7)
                        {
                            startTick += setting.keyAfterTickOffset;
                            nl[i].notes_attribute = 0;
                            t++;
                            new Thread(SendForATime).Start(nl[i]);
                        }
                        break;
                    }
                }
                if (nl[nl.Length - 1].notes_attribute == 0)
                {
                    break;
                }
                Thread.Sleep(1);
            }
            isRun = false;
            onOver?.Invoke();
        }

        private void SendForATime(object obj)
        {
            Notes_list nl = obj as Notes_list;
            float lastTime = nl.effect == 3 || nl.effect == 13 ? nl.effect_value : setting.oneKeyLoopTime;

            byte pos = nl.position;

            long last = DateTime.Now.Ticks;
            do
            {
                Thread.Sleep(setting.longpressInterval);
                KeyDown(pos);
            }
            while (isRun && (DateTime.Now.Ticks - last) <= lastTime * e7);

            for (int i = 0; i < setting.keyupTimes; i++)
            {
                KeyUp(pos);
            }
        }

        private void KeyDown(byte pos)
        {
            Win32API.keybd_event(setting.pos2key[pos], 0, 0, 0);
        }

        private void KeyUp(byte pos)
        {
            Win32API.keybd_event(setting.pos2key[pos], 0, 2, 0);
        }

        public void Reset(Live_info li)
        {
            this.li = li;
            isRun = false;
            Save();
        }

        public bool Reset(int live_difficulty_id)
        {
            return Load(live_difficulty_id);
        }

        private void Save()
        {
            try
            {
                if (!Directory.Exists(LiveDifficultyPath))
                {
                    Directory.CreateDirectory(LiveDifficultyPath);
                }
                if (!li.is_random)
                {
                    string json = JsonConvert.SerializeObject(li, Formatting.Indented);
                    File.WriteAllText(LiveDifficultyPath + "/" + li.live_difficulty_id.ToString("00000000") + ".txt", json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private bool Load(int live_difficulty_id)
        {
            try
            {
                string path = LiveDifficultyPath + "/" + live_difficulty_id.ToString("00000000") + ".txt";
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    li = JsonConvert.DeserializeObject<Live_info>(json);
                    return true;
                }  
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public void Start()
        {
            IntPtr h = Win32API.FindWindow(null, setting.appName);
            Win32API.SetForegroundWindow(h);
            Notes_list[] notelist = new List<Notes_list>(li.notes_list).ToArray();
            float timeStart = notelist[0].timing_sec;

            for (int i = 0; i < notelist.Length; i++)
            {
                notelist[i].timing_sec -= timeStart;
            }

            isRun = true;
            new Thread(TimeLine).Start(notelist);
        }

        public void Stop()
        {
            isRun = false;
            onOver?.Invoke();
        }

        public void TrimForward()
        {
            startTick -= setting.trimValue;
        }

        public void TrimBackward()
        {
            startTick += setting.trimValue;
        }

    }
}
