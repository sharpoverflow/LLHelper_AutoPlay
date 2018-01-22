using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

namespace LLHelper_AutoPlay
{
    [Serializable]
    public class Setting
    {
        [JsonIgnore]
        const string SettingFile = "Setting.txt";

        [JsonIgnore]
        public Setting defaultSetting;

        public Dictionary<byte, byte> pos2key;
        public long trimValue = 150000;
        public float oneKeyLoopTime = 0.03f;
        public long keyAfterTickOffset = 74;
        public int longpressInterval = 1;
        public int keyupTimes = 4;
        public string appName = "Bluestacks App Player";

        private Setting()
        {
            defaultSetting = new Setting(true);
            SetDefaultValue();
        }

        private Setting(bool isDefault)
        {
            SetDefaultValue();
        }

        public void SetDefaultValue()
        {
            pos2key = new Dictionary<byte, byte>();
            pos2key.Add(1, Win32API.Key32.Key_L);
            pos2key.Add(2, Win32API.Key32.Key_K);
            pos2key.Add(3, Win32API.Key32.Key_J);
            pos2key.Add(4, Win32API.Key32.Key_H);
            pos2key.Add(5, Win32API.Key32.Key_Space);
            pos2key.Add(6, Win32API.Key32.Key_F);
            pos2key.Add(7, Win32API.Key32.Key_D);
            pos2key.Add(8, Win32API.Key32.Key_S);
            pos2key.Add(9, Win32API.Key32.Key_A);
            trimValue = 150000;
            oneKeyLoopTime = 0.03f;
            keyAfterTickOffset = 74;
            longpressInterval = 1;
            keyupTimes = 4;
            appName = "Bluestacks App Player";
        }

        static public Setting Load()
        {
            try
            {
                string json = File.ReadAllText(SettingFile);
                Setting s = JsonConvert.DeserializeObject<Setting>(json);
                return s;
            }
            catch { }
            return new Setting();
        }

        static public void Save(Setting s)
        {
            string json = JsonConvert.SerializeObject(s, Formatting.Indented);
            File.WriteAllText(SettingFile, json);
        }

    }

}
