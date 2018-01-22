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
    public partial class AboutForm : Form
    {
        string url = "https://github.com/sharpoverflow/LLHelper_AutoPlay";

        public AboutForm()
        {
            InitializeComponent();
        }

        private void Btn_Copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(url);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(url);
        }
    }
}
