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
    public partial class SettingForm : Form
    {
        public Setting setting;

        public SettingForm(Setting setting)
        {
            InitializeComponent();
            this.setting = setting;
        }
    }
}
