namespace LLHelper_AutoPlay
{
    partial class ColorPlayForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_GetPos = new System.Windows.Forms.Button();
            this.btn_Start = new System.Windows.Forms.Button();
            this.lab_FPS = new System.Windows.Forms.Label();
            this.lab_Percent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 60);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 360);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(408, 48);
            this.label1.TabIndex = 1;
            this.label1.Text = "本工具稳定性一般,目前能够平稳度过EX难度(不能够FC).\r\n! 强调: 为了稳定运行,需要将打歌速度调整为1速 !\r\n当抓包失败时,可使用本工具辅助打歌.";
            // 
            // btn_GetPos
            // 
            this.btn_GetPos.Font = new System.Drawing.Font("宋体", 15F);
            this.btn_GetPos.Location = new System.Drawing.Point(446, 9);
            this.btn_GetPos.Name = "btn_GetPos";
            this.btn_GetPos.Size = new System.Drawing.Size(100, 48);
            this.btn_GetPos.TabIndex = 2;
            this.btn_GetPos.Text = "定位";
            this.btn_GetPos.UseVisualStyleBackColor = true;
            this.btn_GetPos.Click += new System.EventHandler(this.Btn_GetPos_Click);
            // 
            // btn_Start
            // 
            this.btn_Start.Font = new System.Drawing.Font("宋体", 15F);
            this.btn_Start.Location = new System.Drawing.Point(552, 9);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(100, 48);
            this.btn_Start.TabIndex = 2;
            this.btn_Start.Text = "启动";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // lab_FPS
            // 
            this.lab_FPS.BackColor = System.Drawing.Color.Gold;
            this.lab_FPS.Font = new System.Drawing.Font("宋体", 12F);
            this.lab_FPS.Location = new System.Drawing.Point(583, 65);
            this.lab_FPS.Name = "lab_FPS";
            this.lab_FPS.Size = new System.Drawing.Size(64, 17);
            this.lab_FPS.TabIndex = 3;
            this.lab_FPS.Text = "FPS:123";
            this.lab_FPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lab_Percent
            // 
            this.lab_Percent.BackColor = System.Drawing.Color.White;
            this.lab_Percent.Font = new System.Drawing.Font("宋体", 12F);
            this.lab_Percent.Location = new System.Drawing.Point(15, 65);
            this.lab_Percent.Name = "lab_Percent";
            this.lab_Percent.Size = new System.Drawing.Size(562, 17);
            this.lab_Percent.TabIndex = 4;
            // 
            // ColorPlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 431);
            this.Controls.Add(this.lab_Percent);
            this.Controls.Add(this.lab_FPS);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.btn_GetPos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ColorPlayForm";
            this.Text = "图色打歌工具 测试版";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColorPlayForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_GetPos;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Label lab_FPS;
        private System.Windows.Forms.Label lab_Percent;
    }
}