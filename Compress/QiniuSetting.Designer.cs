namespace Compress
{
    partial class QiniuSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QiniuSetting));
            this.label1 = new System.Windows.Forms.Label();
            this.txtAK = new System.Windows.Forms.TextBox();
            this.txtSK = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtZone = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cb1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "AccessKey";
            // 
            // txtAK
            // 
            this.txtAK.Location = new System.Drawing.Point(160, 35);
            this.txtAK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAK.Name = "txtAK";
            this.txtAK.Size = new System.Drawing.Size(239, 21);
            this.txtAK.TabIndex = 1;
            // 
            // txtSK
            // 
            this.txtSK.Location = new System.Drawing.Point(160, 78);
            this.txtSK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSK.Name = "txtSK";
            this.txtSK.Size = new System.Drawing.Size(239, 21);
            this.txtSK.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "SecretKey";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(119, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "机房";
            // 
            // txtZone
            // 
            this.txtZone.Location = new System.Drawing.Point(160, 150);
            this.txtZone.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtZone.Name = "txtZone";
            this.txtZone.Size = new System.Drawing.Size(239, 21);
            this.txtZone.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 153);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "空间名";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(160, 217);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 31);
            this.button1.TabIndex = 8;
            this.button1.Text = "保存配置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cb1
            // 
            this.cb1.FormattingEnabled = true;
            this.cb1.Items.AddRange(new object[] {
            "ZONE_CN_East",
            "ZONE_CN_North",
            "ZONE_CN_South",
            "ZONE_US_North",
            "ZONE_AS_Singapore"});
            this.cb1.Location = new System.Drawing.Point(160, 113);
            this.cb1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cb1.Name = "cb1";
            this.cb1.Size = new System.Drawing.Size(239, 20);
            this.cb1.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(119, 191);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "域名";
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(160, 186);
            this.txtDomain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(239, 21);
            this.txtDomain.TabIndex = 11;
            // 
            // QiniuSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 266);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cb1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtZone);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAK);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QiniuSetting";
            this.Text = "七牛云配置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.QiniuSetting_FormClosed);
            this.Load += new System.EventHandler(this.QiniuSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAK;
        private System.Windows.Forms.TextBox txtSK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtZone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cb1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDomain;
    }
}