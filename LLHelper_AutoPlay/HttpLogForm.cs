using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LLHelper_AutoPlay
{
    public partial class HttpLogForm : Form
    {
        public HttpLogForm()
        {
            InitializeComponent();
        }

        private void HttpList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HttpList.SelectedIndex < 0) return;

            LogText.Text = HttpList.SelectedItem.ToString();
        }

        public void ClearList()
        {
            HttpList.Items.Clear();
        }

        public void OnNewHttpAdd(string s)
        {
            if (Visible && AllowOutput.CheckState == CheckState.Checked)
            {
                if(OnlyAllowLL.CheckState != CheckState.Checked)
                {
                    HttpList.Items.Add(s);
                }   
            }
        }

        public void OnNewHttpLLAdd(string s)
        {
            if (Visible && AllowOutput.CheckState == CheckState.Checked)
            {
                if (OnlyAllowLL.CheckState == CheckState.Checked)
                {
                    HttpList.Items.Add(s);
                }
            }
        }

        private void HttpLogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            ClearList();
        }

        private void OnlyAllowLL_CheckedChanged(object sender, EventArgs e)
        {
            ClearList();
        }
    }
}
