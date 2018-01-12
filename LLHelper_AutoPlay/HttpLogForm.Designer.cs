namespace LLHelper_AutoPlay
{
    partial class HttpLogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.HttpList = new System.Windows.Forms.ListBox();
            this.LogText = new System.Windows.Forms.TextBox();
            this.AllowOutput = new System.Windows.Forms.CheckBox();
            this.OnlyAllowLL = new System.Windows.Forms.CheckBox();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // HttpList
            // 
            this.HttpList.FormattingEnabled = true;
            this.HttpList.ItemHeight = 12;
            this.HttpList.Location = new System.Drawing.Point(12, 36);
            this.HttpList.Name = "HttpList";
            this.HttpList.Size = new System.Drawing.Size(275, 436);
            this.HttpList.TabIndex = 0;
            this.HttpList.SelectedIndexChanged += new System.EventHandler(this.HttpList_SelectedIndexChanged);
            // 
            // LogText
            // 
            this.LogText.Location = new System.Drawing.Point(293, 36);
            this.LogText.MaxLength = 2147483647;
            this.LogText.Multiline = true;
            this.LogText.Name = "LogText";
            this.LogText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogText.Size = new System.Drawing.Size(732, 436);
            this.LogText.TabIndex = 1;
            // 
            // AllowOutput
            // 
            this.AllowOutput.AutoSize = true;
            this.AllowOutput.Checked = true;
            this.AllowOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllowOutput.Location = new System.Drawing.Point(12, 12);
            this.AllowOutput.Name = "AllowOutput";
            this.AllowOutput.Size = new System.Drawing.Size(48, 16);
            this.AllowOutput.TabIndex = 2;
            this.AllowOutput.Text = "输出";
            this.AllowOutput.UseVisualStyleBackColor = true;
            // 
            // OnlyAllowLL
            // 
            this.OnlyAllowLL.AutoSize = true;
            this.OnlyAllowLL.Checked = true;
            this.OnlyAllowLL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OnlyAllowLL.Location = new System.Drawing.Point(66, 12);
            this.OnlyAllowLL.Name = "OnlyAllowLL";
            this.OnlyAllowLL.Size = new System.Drawing.Size(60, 16);
            this.OnlyAllowLL.TabIndex = 3;
            this.OnlyAllowLL.Text = "OnlyLL";
            this.OnlyAllowLL.UseVisualStyleBackColor = true;
            this.OnlyAllowLL.CheckedChanged += new System.EventHandler(this.OnlyAllowLL_CheckedChanged);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(133, 10);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(83, 19);
            this.btn_Clear.TabIndex = 4;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.Btn_Clear_Click);
            // 
            // HttpLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 484);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.OnlyAllowLL);
            this.Controls.Add(this.AllowOutput);
            this.Controls.Add(this.LogText);
            this.Controls.Add(this.HttpList);
            this.Name = "HttpLogForm";
            this.Text = "HttpLog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HttpLogForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox HttpList;
        private System.Windows.Forms.TextBox LogText;
        private System.Windows.Forms.CheckBox AllowOutput;
        private System.Windows.Forms.CheckBox OnlyAllowLL;
        private System.Windows.Forms.Button btn_Clear;
    }
}