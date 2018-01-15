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
        const string SettingFile = "setting.txt";

        [JsonIgnore]
        public Setting defaultSetting;

        public Dictionary<byte, byte> pos2key;

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
            string json = JsonConvert.SerializeObject(s);
            File.WriteAllText(SettingFile, json);
        }

    }

}
