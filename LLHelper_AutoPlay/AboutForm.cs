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
        public AboutForm()
        {
            InitializeComponent();
        }

        private void Btn_Copy_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/sharpoverflow/LLHelper_AutoPlay";
            Clipboard.SetText(url);
        }
    }
}
