using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LLHelper_AutoPlay
{
    public partial class SettingForm : Form
    {
        public Setting setting;

        private Label[] keyshow;

        public SettingForm(Setting setting)
        {
            InitializeComponent();

            if (setting == null)
            {
                return;
            }
            this.setting = setting;

            keyshow = new Label[9];
            keyshow[0] = lab_key_0;
            keyshow[1] = lab_key_1;
            keyshow[2] = lab_key_2;
            keyshow[3] = lab_key_3;
            keyshow[4] = lab_key_4;
            keyshow[5] = lab_key_5;
            keyshow[6] = lab_key_6;
            keyshow[7] = lab_key_7;
            keyshow[8] = lab_key_8;

            string s = "";
            for (byte i = 9; i >=1; i--)
            {
                s += (char)setting.pos2key[i];
            }
            text_KeyStr.Text = s;

            textBox1.Text = setting.trimValue.ToString();
            textBox2.Text = setting.oneKeyLoopTime.ToString();
            textBox3.Text = setting.keyAfterTickOffset.ToString();
            textBox4.Text = setting.longpressInterval.ToString();
            textBox5.Text = setting.keyupTimes.ToString();
            textBox6.Text = setting.appName;
        }

        public delegate void OnSave();
        public OnSave onSave;

        private void Text_KeyStr_KeyPress(object sender, KeyPressEventArgs e)
        {
            int kk = (int)e.KeyChar;
            bool b = false;
            b |= kk >= 48 && kk <= 57;
            b |= kk >= 65 && kk <= 90;
            b |= kk >= 97 && kk <= 122;
            b |= kk == 32;
            e.Handled = !b;
        }

        private void Text_KeyStr_TextChanged(object sender, EventArgs e)
        {
            RefreshKey();
        }

        private void RefreshKey()
        {
            int c = text_KeyStr.SelectionStart;
            string m = "";
            string s = text_KeyStr.Text;
            if (s.Length > 9)
            {
                s = s.Substring(0, 9);
            }
            int i;
            for (i = 0; i < s.Length && i < 9; i++)
            {
                m += s[i];
            }
            for (; i < 9; i++)
            {
                m += '-';
            }
            m = m.ToUpper();
            text_KeyStr.Text = m;
            text_KeyStr.SelectionStart = c;

            for (i = 0; i < keyshow.Length; i++)
            {
                keyshow[i].Text = m[i].ToString();
                if(m[i] == ' ')
                {
                    keyshow[i].Text = "Space";
                }
            }
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string s = text_KeyStr.Text;
                for (byte i = 9; i >= 1; i--)
                {
                    if (s[9 - i] == '-') throw new Exception();
                    setting.pos2key[i] = (byte)s[9 - i];
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("键值串填写有误!");
                return;
            }
            try
            {
                setting.trimValue = int.Parse(textBox1.Text);
                setting.oneKeyLoopTime = float.Parse(textBox2.Text);
                setting.keyAfterTickOffset = long.Parse(textBox3.Text);
                setting.longpressInterval = int.Parse(textBox4.Text);
                setting.keyupTimes = int.Parse(textBox5.Text);
                setting.appName = textBox6.Text;
            }
            catch
            {
                MessageBox.Show("数值格式不正确!");
                return;
            }
            Setting.Save(setting);
            onSave?.Invoke();
            this.Dispose();
        }

        private void Btn_Default_Key_Click(object sender, EventArgs e)
        {
            setting.SetDefaultValue();
            string s = "";
            for (byte i = 9; i >= 1; i--)
            {
                s += (char)setting.pos2key[i];
            }
            text_KeyStr.Text = s;
        }
    }
}
