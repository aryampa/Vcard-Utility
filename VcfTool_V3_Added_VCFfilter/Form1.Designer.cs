namespace VcfTool_V3_Added_VCFfilter
{
    partial class Form1
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
            this.cbxTools = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSelectedTool = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ckBxFileMode = new System.Windows.Forms.CheckBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lblFileOpen = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.lblFileCounter = new System.Windows.Forms.Label();
            this.lbLinesCounter = new System.Windows.Forms.Label();
            this.tbxPreview = new System.Windows.Forms.TextBox();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.tbxFilter = new System.Windows.Forms.TextBox();
            this.ckbxFilter = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbxTools
            // 
            this.cbxTools.FormattingEnabled = true;
            this.cbxTools.Items.AddRange(new object[] {
            "Vcf To Csv",
            "Csv To Vcf",
            "Apple Vcf to Nornal Vcf",
            "Outlook to Vcf",
            "Vcf To Outlook",
            "Excel To Csv",
            "Csv To Excel"});
            this.cbxTools.Location = new System.Drawing.Point(83, 12);
            this.cbxTools.Name = "cbxTools";
            this.cbxTools.Size = new System.Drawing.Size(189, 21);
            this.cbxTools.TabIndex = 0;
            this.cbxTools.DropDownClosed += new System.EventHandler(this.cbxTools_DropDownClosed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Tool:";
            // 
            // lblSelectedTool
            // 
            this.lblSelectedTool.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblSelectedTool.Location = new System.Drawing.Point(98, 40);
            this.lblSelectedTool.Name = "lblSelectedTool";
            this.lblSelectedTool.Size = new System.Drawing.Size(174, 16);
            this.lblSelectedTool.TabIndex = 2;
            this.lblSelectedTool.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Selected Tool:";
            // 
            // ckBxFileMode
            // 
            this.ckBxFileMode.AutoSize = true;
            this.ckBxFileMode.Location = new System.Drawing.Point(19, 82);
            this.ckBxFileMode.Name = "ckBxFileMode";
            this.ckBxFileMode.Size = new System.Drawing.Size(86, 17);
            this.ckBxFileMode.TabIndex = 4;
            this.ckBxFileMode.Text = "Multiple Files";
            this.ckBxFileMode.UseVisualStyleBackColor = true;
            this.ckBxFileMode.CheckedChanged += new System.EventHandler(this.ckBxFileMode_CheckedChanged);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(112, 82);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(160, 41);
            this.btnOpen.TabIndex = 5;
            this.btnOpen.Text = "Select Folder";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // lblFileOpen
            // 
            this.lblFileOpen.BackColor = System.Drawing.Color.Red;
            this.lblFileOpen.Location = new System.Drawing.Point(19, 130);
            this.lblFileOpen.Name = "lblFileOpen";
            this.lblFileOpen.Size = new System.Drawing.Size(253, 18);
            this.lblFileOpen.TabIndex = 6;
            this.lblFileOpen.Text = "label3";
            this.lblFileOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(112, 296);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(160, 43);
            this.btnGo.TabIndex = 8;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.EnabledChanged += new System.EventHandler(this.btnGo_EnabledChanged);
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblFileCounter
            // 
            this.lblFileCounter.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblFileCounter.Location = new System.Drawing.Point(312, 12);
            this.lblFileCounter.Name = "lblFileCounter";
            this.lblFileCounter.Size = new System.Drawing.Size(263, 28);
            this.lblFileCounter.TabIndex = 9;
            this.lblFileCounter.Text = "label4";
            this.lblFileCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLinesCounter
            // 
            this.lbLinesCounter.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbLinesCounter.Location = new System.Drawing.Point(312, 53);
            this.lbLinesCounter.Name = "lbLinesCounter";
            this.lbLinesCounter.Size = new System.Drawing.Size(263, 23);
            this.lbLinesCounter.TabIndex = 10;
            this.lbLinesCounter.Text = "label4";
            this.lbLinesCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbxPreview
            // 
            this.tbxPreview.BackColor = System.Drawing.Color.Black;
            this.tbxPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.tbxPreview.Location = new System.Drawing.Point(306, 82);
            this.tbxPreview.Multiline = true;
            this.tbxPreview.Name = "tbxPreview";
            this.tbxPreview.ReadOnly = true;
            this.tbxPreview.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbxPreview.Size = new System.Drawing.Size(269, 257);
            this.tbxPreview.TabIndex = 11;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            // 
            // tbxFilter
            // 
            this.tbxFilter.Enabled = false;
            this.tbxFilter.Location = new System.Drawing.Point(88, 166);
            this.tbxFilter.Name = "tbxFilter";
            this.tbxFilter.Size = new System.Drawing.Size(184, 20);
            this.tbxFilter.TabIndex = 13;
            // 
            // ckbxFilter
            // 
            this.ckbxFilter.AutoSize = true;
            this.ckbxFilter.Location = new System.Drawing.Point(16, 168);
            this.ckbxFilter.Name = "ckbxFilter";
            this.ckbxFilter.Size = new System.Drawing.Size(66, 17);
            this.ckbxFilter.TabIndex = 14;
            this.ckbxFilter.Text = "-ve Filter";
            this.ckbxFilter.UseVisualStyleBackColor = true;
            this.ckbxFilter.CheckedChanged += new System.EventHandler(this.ckbxFilter_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 351);
            this.Controls.Add(this.ckbxFilter);
            this.Controls.Add(this.tbxFilter);
            this.Controls.Add(this.tbxPreview);
            this.Controls.Add(this.lbLinesCounter);
            this.Controls.Add(this.lblFileCounter);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lblFileOpen);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.ckBxFileMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSelectedTool);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxTools);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Contacts Tool V3 (Filter added)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxTools;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSelectedTool;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckBxFileMode;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label lblFileOpen;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label lblFileCounter;
        private System.Windows.Forms.Label lbLinesCounter;
        private System.Windows.Forms.TextBox tbxPreview;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.CheckBox ckbxFilter;
        public System.Windows.Forms.TextBox tbxFilter;

    }
}

