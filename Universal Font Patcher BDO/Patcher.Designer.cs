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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        gunaLabel1 = new Label();
        gunaPanel1 = new Panel();
        BtnExit = new Button();
        gunaLabel2 = new Label();
        BtnContinue = new Button();
        TxtGamePath = new TextBox();
        TxtFontPath = new TextBox();
        BtnSelectGameFolder = new Button();
        BtnSelectFont = new Button();
        lblDetectedPaths = new Label();
        flowPathsPanel = new FlowLayoutPanel();
        folderBrowserDialog1 = new FolderBrowserDialog();
        gunaLabel3 = new Label();
        gunaPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // gunaLabel1
        // 
        gunaLabel1.AutoSize = true;
        gunaLabel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        gunaLabel1.ForeColor = Color.White;
        gunaLabel1.Location = new Point(15, 8);
        gunaLabel1.Margin = new Padding(4, 0, 4, 0);
        gunaLabel1.Name = "gunaLabel1";
        gunaLabel1.Size = new Size(110, 17);
        gunaLabel1.TabIndex = 0;
        gunaLabel1.Text = "BDO Font Patcher";
        // 
        // gunaPanel1
        // 
        gunaPanel1.BackColor = Color.FromArgb(35, 35, 35);
        gunaPanel1.Controls.Add(BtnExit);
        gunaPanel1.Controls.Add(gunaLabel1);
        gunaPanel1.Dock = DockStyle.Top;
        gunaPanel1.Location = new Point(0, 0);
        gunaPanel1.Margin = new Padding(4, 3, 4, 3);
        gunaPanel1.Name = "gunaPanel1";
        gunaPanel1.Size = new Size(560, 33);
        gunaPanel1.TabIndex = 1;
        // 
        // BtnExit
        // 
        BtnExit.BackColor = Color.FromArgb(64, 64, 64);
        BtnExit.FlatAppearance.BorderSize = 0;
        BtnExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);
        BtnExit.FlatStyle = FlatStyle.Flat;
        BtnExit.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        BtnExit.ForeColor = Color.White;
        BtnExit.Location = new Point(506, 4);
        BtnExit.Margin = new Padding(4, 3, 4, 3);
        BtnExit.Name = "BtnExit";
        BtnExit.Size = new Size(50, 25);
        BtnExit.TabIndex = 26;
        BtnExit.Text = "X";
        BtnExit.UseVisualStyleBackColor = false;
        BtnExit.Click += BtnExit_Click;
        // 
        // gunaLabel2
        // 
        gunaLabel2.AutoSize = true;
        gunaLabel2.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        gunaLabel2.ForeColor = Color.White;
        gunaLabel2.Location = new Point(7, 42);
        gunaLabel2.Margin = new Padding(4, 0, 4, 0);
        gunaLabel2.Name = "gunaLabel2";
        gunaLabel2.Size = new Size(0, 14);
        gunaLabel2.TabIndex = 24;
        // 
        // BtnContinue
        // 
        BtnContinue.BackColor = SystemColors.HotTrack;
        BtnContinue.FlatAppearance.BorderSize = 0;
        BtnContinue.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
        BtnContinue.FlatStyle = FlatStyle.Flat;
        BtnContinue.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        BtnContinue.ForeColor = Color.White;
        BtnContinue.Location = new Point(15, 237);
        BtnContinue.Margin = new Padding(4, 3, 4, 3);
        BtnContinue.Name = "BtnContinue";
        BtnContinue.Size = new Size(530, 35);
        BtnContinue.TabIndex = 25;
        BtnContinue.Text = "Patch";
        BtnContinue.UseVisualStyleBackColor = false;
        BtnContinue.Click += BtnContinue_Click;
        // 
        // TxtGamePath
        // 
        TxtGamePath.BackColor = Color.FromArgb(35, 35, 35);
        TxtGamePath.BorderStyle = BorderStyle.FixedSingle;
        TxtGamePath.Cursor = Cursors.IBeam;
        TxtGamePath.ForeColor = Color.White;
        TxtGamePath.Location = new Point(170, 45);
        TxtGamePath.Margin = new Padding(4, 3, 4, 3);
        TxtGamePath.Name = "TxtGamePath";
        TxtGamePath.ReadOnly = true;
        TxtGamePath.Size = new Size(323, 23);
        TxtGamePath.TabIndex = 28;
        TxtGamePath.Text = "...";
        // 
        // TxtFontPath
        // 
        TxtFontPath.BackColor = Color.FromArgb(35, 35, 35);
        TxtFontPath.BorderStyle = BorderStyle.FixedSingle;
        TxtFontPath.Cursor = Cursors.IBeam;
        TxtFontPath.ForeColor = Color.White;
        TxtFontPath.Location = new Point(170, 75);
        TxtFontPath.Margin = new Padding(4, 3, 4, 3);
        TxtFontPath.Name = "TxtFontPath";
        TxtFontPath.ReadOnly = true;
        TxtFontPath.Size = new Size(323, 23);
        TxtFontPath.TabIndex = 29;
        TxtFontPath.Text = "...";
        // 
        // BtnSelectGameFolder
        // 
        BtnSelectGameFolder.BackColor = SystemColors.HotTrack;
        BtnSelectGameFolder.FlatAppearance.BorderSize = 0;
        BtnSelectGameFolder.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
        BtnSelectGameFolder.FlatStyle = FlatStyle.Flat;
        BtnSelectGameFolder.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        BtnSelectGameFolder.ForeColor = Color.White;
        BtnSelectGameFolder.Location = new Point(15, 47);
        BtnSelectGameFolder.Margin = new Padding(4, 3, 4, 3);
        BtnSelectGameFolder.Name = "BtnSelectGameFolder";
        BtnSelectGameFolder.Size = new Size(150, 20);
        BtnSelectGameFolder.TabIndex = 30;
        BtnSelectGameFolder.Text = "Game Folder";
        BtnSelectGameFolder.UseVisualStyleBackColor = false;
        BtnSelectGameFolder.Click += BtnSelectGameFolder_Click;
        // 
        // BtnSelectFont
        // 
        BtnSelectFont.BackColor = SystemColors.HotTrack;
        BtnSelectFont.FlatAppearance.BorderSize = 0;
        BtnSelectFont.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 55, 55);
        BtnSelectFont.FlatStyle = FlatStyle.Flat;
        BtnSelectFont.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        BtnSelectFont.ForeColor = Color.White;
        BtnSelectFont.Location = new Point(15, 77);
        BtnSelectFont.Margin = new Padding(4, 3, 4, 3);
        BtnSelectFont.Name = "BtnSelectFont";
        BtnSelectFont.Size = new Size(150, 20);
        BtnSelectFont.TabIndex = 31;
        BtnSelectFont.Text = "Select font";
        BtnSelectFont.UseVisualStyleBackColor = false;
        BtnSelectFont.Click += BtnSelectFont_Click;
        // 
        // lblDetectedPaths
        // 
        lblDetectedPaths.AutoSize = true;
        lblDetectedPaths.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblDetectedPaths.ForeColor = Color.White;
        lblDetectedPaths.Location = new Point(15, 120);
        lblDetectedPaths.Margin = new Padding(4, 0, 4, 0);
        lblDetectedPaths.Name = "lblDetectedPaths";
        lblDetectedPaths.Size = new Size(123, 15);
        lblDetectedPaths.TabIndex = 33;
        lblDetectedPaths.Text = "Detected installations:";
        // 
        // flowPathsPanel
        // 
        flowPathsPanel.AutoScroll = true;
        flowPathsPanel.BackColor = Color.FromArgb(64, 64, 64);
        flowPathsPanel.FlowDirection = FlowDirection.TopDown;
        flowPathsPanel.ForeColor = Color.White;
        flowPathsPanel.Location = new Point(15, 140);
        flowPathsPanel.Margin = new Padding(4, 3, 4, 3);
        flowPathsPanel.Name = "flowPathsPanel";
        flowPathsPanel.Size = new Size(530, 69);
        flowPathsPanel.TabIndex = 34;
        flowPathsPanel.WrapContents = false;
        // 
        // gunaLabel3
        // 
        gunaLabel3.AutoSize = true;
        gunaLabel3.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        gunaLabel3.ForeColor = Color.White;
        gunaLabel3.Location = new Point(15, 275);
        gunaLabel3.Margin = new Padding(4, 0, 4, 0);
        gunaLabel3.Name = "gunaLabel3";
        gunaLabel3.Size = new Size(45, 13);
        gunaLabel3.TabIndex = 32;
        gunaLabel3.Text = "@Sehyn";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(40, 40, 40);
        ClientSize = new Size(560, 300);
        Controls.Add(gunaLabel3);
        Controls.Add(flowPathsPanel);
        Controls.Add(lblDetectedPaths);
        Controls.Add(BtnSelectFont);
        Controls.Add(BtnSelectGameFolder);
        Controls.Add(TxtFontPath);
        Controls.Add(TxtGamePath);
        Controls.Add(BtnContinue);
        Controls.Add(gunaLabel2);
        Controls.Add(gunaPanel1);
        FormBorderStyle = FormBorderStyle.None;
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(4, 3, 4, 3);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "UFP - TERMS";
        Load += Form1_Load;
        gunaPanel1.ResumeLayout(false);
        gunaPanel1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();

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
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = null!;
    private System.Windows.Forms.Label lblDetectedPaths = null!;
    private System.Windows.Forms.FlowLayoutPanel flowPathsPanel = null!;
    private Label gunaLabel3 = null!;
}
