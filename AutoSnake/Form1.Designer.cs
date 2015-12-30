namespace AutoSnake
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SetStart = new System.Windows.Forms.Button();
            this.SetEnd = new System.Windows.Forms.Button();
            this.SetWall = new System.Windows.Forms.Button();
            this.SetRoad = new System.Windows.Forms.Button();
            this.BeginFind = new System.Windows.Forms.Button();
            this.ResultText = new System.Windows.Forms.TextBox();
            this.Reset = new System.Windows.Forms.Button();
            this.FindType = new System.Windows.Forms.ComboBox();
            this.Refind = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(187, 28);
            this.textBox1.TabIndex = 0;
            // 
            // SetStart
            // 
            this.SetStart.Location = new System.Drawing.Point(266, 10);
            this.SetStart.Name = "SetStart";
            this.SetStart.Size = new System.Drawing.Size(75, 30);
            this.SetStart.TabIndex = 1;
            this.SetStart.Text = "起点";
            this.SetStart.UseVisualStyleBackColor = true;
            this.SetStart.Click += new System.EventHandler(this.SetStart_Click);
            // 
            // SetEnd
            // 
            this.SetEnd.Location = new System.Drawing.Point(347, 10);
            this.SetEnd.Name = "SetEnd";
            this.SetEnd.Size = new System.Drawing.Size(75, 30);
            this.SetEnd.TabIndex = 2;
            this.SetEnd.Text = "终点";
            this.SetEnd.UseVisualStyleBackColor = true;
            this.SetEnd.Click += new System.EventHandler(this.SetEnd_Click);
            // 
            // SetWall
            // 
            this.SetWall.Location = new System.Drawing.Point(428, 10);
            this.SetWall.Name = "SetWall";
            this.SetWall.Size = new System.Drawing.Size(75, 30);
            this.SetWall.TabIndex = 3;
            this.SetWall.Text = "墙";
            this.SetWall.UseVisualStyleBackColor = true;
            this.SetWall.Click += new System.EventHandler(this.SetWall_Click);
            // 
            // SetRoad
            // 
            this.SetRoad.Location = new System.Drawing.Point(529, 9);
            this.SetRoad.Name = "SetRoad";
            this.SetRoad.Size = new System.Drawing.Size(75, 30);
            this.SetRoad.TabIndex = 4;
            this.SetRoad.Text = "路";
            this.SetRoad.UseVisualStyleBackColor = true;
            this.SetRoad.Click += new System.EventHandler(this.SetRoad_Click);
            // 
            // BeginFind
            // 
            this.BeginFind.Location = new System.Drawing.Point(347, 55);
            this.BeginFind.Name = "BeginFind";
            this.BeginFind.Size = new System.Drawing.Size(75, 30);
            this.BeginFind.TabIndex = 5;
            this.BeginFind.Text = "开始";
            this.BeginFind.UseVisualStyleBackColor = true;
            this.BeginFind.Click += new System.EventHandler(this.BeginFind_Click);
            // 
            // ResultText
            // 
            this.ResultText.Location = new System.Drawing.Point(12, 55);
            this.ResultText.Name = "ResultText";
            this.ResultText.Size = new System.Drawing.Size(187, 28);
            this.ResultText.TabIndex = 6;
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(529, 55);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(75, 30);
            this.Reset.TabIndex = 7;
            this.Reset.Text = "重置";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // FindType
            // 
            this.FindType.FormattingEnabled = true;
            this.FindType.Location = new System.Drawing.Point(220, 55);
            this.FindType.Name = "FindType";
            this.FindType.Size = new System.Drawing.Size(121, 26);
            this.FindType.TabIndex = 8;
            // 
            // Refind
            // 
            this.Refind.Location = new System.Drawing.Point(428, 55);
            this.Refind.Name = "Refind";
            this.Refind.Size = new System.Drawing.Size(95, 30);
            this.Refind.TabIndex = 9;
            this.Refind.Text = "重新寻找";
            this.Refind.UseVisualStyleBackColor = true;
            this.Refind.Click += new System.EventHandler(this.Refind_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 729);
            this.Controls.Add(this.Refind);
            this.Controls.Add(this.FindType);
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.ResultText);
            this.Controls.Add(this.BeginFind);
            this.Controls.Add(this.SetRoad);
            this.Controls.Add(this.SetWall);
            this.Controls.Add(this.SetEnd);
            this.Controls.Add(this.SetStart);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SetStart;
        private System.Windows.Forms.Button SetEnd;
        private System.Windows.Forms.Button SetWall;
        private System.Windows.Forms.Button SetRoad;
        private System.Windows.Forms.Button BeginFind;
        private System.Windows.Forms.TextBox ResultText;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.ComboBox FindType;
        private System.Windows.Forms.Button Refind;




    }
}

