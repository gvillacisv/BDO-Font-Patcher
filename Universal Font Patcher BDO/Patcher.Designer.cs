namespace Universal_Font_Patcher_BDO;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gunaLabel1 = new System.Windows.Forms.Label();
            this.gunaPanel1 = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.gunaLabel2 = new System.Windows.Forms.Label();
            this.BtnContinue = new System.Windows.Forms.Button();
            this.TxtGamePath = new System.Windows.Forms.TextBox();
            this.TxtFontPath = new System.Windows.Forms.TextBox();
            this.BtnSelectGameFolder = new System.Windows.Forms.Button();
            this.BtnSelectFont = new System.Windows.Forms.Button();
            this.gunaLabel3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gunaPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gunaLabel1
            // 
            this.gunaLabel1.AutoSize = true;
            this.gunaLabel1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel1.ForeColor = System.Drawing.Color.White;
            this.gunaLabel1.Location = new System.Drawing.Point(10, 11);
            this.gunaLabel1.Name = "gunaLabel1";
            this.gunaLabel1.Size = new System.Drawing.Size(163, 14);
            this.gunaLabel1.TabIndex = 0;
            this.gunaLabel1.Text = "Universal Font Patcher [BDO]";
            // 
            // gunaPanel1
            // 
            this.gunaPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.gunaPanel1.Controls.Add(this.BtnExit);
            this.gunaPanel1.Controls.Add(this.gunaLabel1);
            this.gunaPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gunaPanel1.Location = new System.Drawing.Point(0, 0);
            this.gunaPanel1.Name = "gunaPanel1";
            this.gunaPanel1.Size = new System.Drawing.Size(478, 38);
            this.gunaPanel1.TabIndex = 1;
            // 
            // BtnExit
            // 
            this.BtnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.BtnExit.FlatAppearance.BorderSize = 0;
            this.BtnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BtnExit.ForeColor = System.Drawing.Color.White;
            this.BtnExit.Location = new System.Drawing.Point(421, 7);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(51, 24);
            this.BtnExit.TabIndex = 26;
            this.BtnExit.Text = "X";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // gunaLabel2
            // 
            this.gunaLabel2.AutoSize = true;
            this.gunaLabel2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel2.ForeColor = System.Drawing.Color.White;
            this.gunaLabel2.Location = new System.Drawing.Point(6, 41);
            this.gunaLabel2.Name = "gunaLabel2";
            this.gunaLabel2.Size = new System.Drawing.Size(0, 14);
            this.gunaLabel2.TabIndex = 24;
            // 
            // BtnContinue
            // 
            this.BtnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.BtnContinue.FlatAppearance.BorderSize = 0;
            this.BtnContinue.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.BtnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnContinue.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnContinue.ForeColor = System.Drawing.Color.White;
            this.BtnContinue.Location = new System.Drawing.Point(13, 128);
            this.BtnContinue.Name = "BtnContinue";
            this.BtnContinue.Size = new System.Drawing.Size(459, 30);
            this.BtnContinue.TabIndex = 25;
            this.BtnContinue.Text = "Use selected font";
            this.BtnContinue.UseVisualStyleBackColor = false;
            this.BtnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // TxtGamePath
            // 
            this.TxtGamePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.TxtGamePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtGamePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TxtGamePath.ForeColor = System.Drawing.Color.White;
            this.TxtGamePath.Location = new System.Drawing.Point(195, 44);
            this.TxtGamePath.Name = "TxtGamePath";
            this.TxtGamePath.ReadOnly = true;
            this.TxtGamePath.Size = new System.Drawing.Size(277, 36);
            this.TxtGamePath.TabIndex = 28;
            this.TxtGamePath.Text = "C:\\..";
            this.TxtGamePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // TxtFontPath
            // 
            this.TxtFontPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.TxtFontPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtFontPath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TxtFontPath.ForeColor = System.Drawing.Color.White;
            this.TxtFontPath.Location = new System.Drawing.Point(195, 86);
            this.TxtFontPath.Name = "TxtFontPath";
            this.TxtFontPath.ReadOnly = true;
            this.TxtFontPath.Size = new System.Drawing.Size(277, 36);
            this.TxtFontPath.TabIndex = 29;
            this.TxtFontPath.Text = "C:\\..";
            this.TxtFontPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // BtnSelectGameFolder
            // 
            this.BtnSelectGameFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.BtnSelectGameFolder.FlatAppearance.BorderSize = 0;
            this.BtnSelectGameFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.BtnSelectGameFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSelectGameFolder.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelectGameFolder.ForeColor = System.Drawing.Color.White;
            this.BtnSelectGameFolder.Location = new System.Drawing.Point(12, 44);
            this.BtnSelectGameFolder.Name = "BtnSelectGameFolder";
            this.BtnSelectGameFolder.Size = new System.Drawing.Size(177, 36);
            this.BtnSelectGameFolder.TabIndex = 30;
            this.BtnSelectGameFolder.Text = "Select game folder";
            this.BtnSelectGameFolder.UseVisualStyleBackColor = false;
            this.BtnSelectGameFolder.Click += new System.EventHandler(this.BtnSelectGameFolder_Click);
            // 
            // BtnSelectFont
            // 
            this.BtnSelectFont.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.BtnSelectFont.FlatAppearance.BorderSize = 0;
            this.BtnSelectFont.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.BtnSelectFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSelectFont.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelectFont.ForeColor = System.Drawing.Color.White;
            this.BtnSelectFont.Location = new System.Drawing.Point(12, 86);
            this.BtnSelectFont.Name = "BtnSelectFont";
            this.BtnSelectFont.Size = new System.Drawing.Size(177, 36);
            this.BtnSelectFont.TabIndex = 31;
            this.BtnSelectFont.Text = "Select font";
            this.BtnSelectFont.UseVisualStyleBackColor = false;
            this.BtnSelectFont.Click += new System.EventHandler(this.BtnSelectFont_Click);
            // 
            // gunaLabel3
            // 
            this.gunaLabel3.AutoSize = true;
            this.gunaLabel3.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel3.ForeColor = System.Drawing.Color.White;
            this.gunaLabel3.Location = new System.Drawing.Point(12, 161);
            this.gunaLabel3.Name = "gunaLabel3";
            this.gunaLabel3.Size = new System.Drawing.Size(45, 13);
            this.gunaLabel3.TabIndex = 32;
            this.gunaLabel3.Text = "@Sehyn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(478, 182);
            this.Controls.Add(this.gunaLabel3);
            this.Controls.Add(this.BtnSelectFont);
            this.Controls.Add(this.BtnSelectGameFolder);
            this.Controls.Add(this.TxtFontPath);
            this.Controls.Add(this.TxtGamePath);
            this.Controls.Add(this.BtnContinue);
            this.Controls.Add(this.gunaLabel2);
            this.Controls.Add(this.gunaPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UFP - TERMS";
            this.gunaPanel1.ResumeLayout(false);
            this.gunaPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label gunaLabel1 = null!;
        private System.Windows.Forms.Panel gunaPanel1 = null!;
        private System.Windows.Forms.Label gunaLabel2 = null!;
        private System.Windows.Forms.Button BtnContinue = null!;
        private System.Windows.Forms.Button BtnExit = null!;
        private System.Windows.Forms.Button BtnSelectFont = null!;
        private System.Windows.Forms.Button BtnSelectGameFolder = null!;
        private System.Windows.Forms.TextBox TxtFontPath = null!;
        private System.Windows.Forms.TextBox TxtGamePath = null!;
        private System.Windows.Forms.Label gunaLabel3 = null!;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = null!;
    }

