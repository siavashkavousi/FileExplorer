namespace FileExplorer
{
    partial class UI
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView = new System.Windows.Forms.TreeView();
            this.fileView = new System.Windows.Forms.ListView();
            this.lnameClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.extClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sizeClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deletClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hidClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sysClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.writepClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastcClm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dirRClikMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dirToggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileRClikMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fileToggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.dirRClikMenu.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.fileRClikMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 337);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(995, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(78, 17);
            this.toolStripStatusLabel1.Text = "<folder info>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Drive : ";
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(15, 62);
            this.treeView.Name = "treeView";
            this.treeView.ShowNodeToolTips = true;
            this.treeView.Size = new System.Drawing.Size(272, 272);
            this.treeView.TabIndex = 4;
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.directoryExplorer_NodeMouseClick);
            // 
            // fileView
            // 
            this.fileView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lnameClm,
            this.extClm,
            this.sizeClm,
            this.deletClm,
            this.hidClm,
            this.sysClm,
            this.writepClm,
            this.lastcClm});
            this.fileView.Location = new System.Drawing.Point(293, 62);
            this.fileView.Name = "fileView";
            this.fileView.Size = new System.Drawing.Size(690, 272);
            this.fileView.TabIndex = 6;
            this.fileView.UseCompatibleStateImageBehavior = false;
            this.fileView.View = System.Windows.Forms.View.Details;
            this.fileView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.filePlorer_MouseClick);
            // 
            // lnameClm
            // 
            this.lnameClm.Text = "Long Name";
            this.lnameClm.Width = 115;
            // 
            // extClm
            // 
            this.extClm.Text = "Extention";
            this.extClm.Width = 57;
            // 
            // sizeClm
            // 
            this.sizeClm.Text = "Size";
            this.sizeClm.Width = 77;
            // 
            // deletClm
            // 
            this.deletClm.Text = "Deleted";
            this.deletClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.deletClm.Width = 49;
            // 
            // hidClm
            // 
            this.hidClm.Text = "Hidden";
            this.hidClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hidClm.Width = 47;
            // 
            // sysClm
            // 
            this.sysClm.Text = "System File";
            this.sysClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sysClm.Width = 66;
            // 
            // writepClm
            // 
            this.writepClm.Text = "Write Protected";
            this.writepClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.writepClm.Width = 88;
            // 
            // lastcClm
            // 
            this.lastcClm.Text = "Last Change";
            this.lastcClm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lastcClm.Width = 186;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(59, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(228, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dirRClikMenu
            // 
            this.dirRClikMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dirToggleToolStripMenuItem});
            this.dirRClikMenu.Name = "dirRClikMenu";
            this.dirRClikMenu.Size = new System.Drawing.Size(118, 26);
            // 
            // dirToggleToolStripMenuItem
            // 
            this.dirToggleToolStripMenuItem.Name = "dirToggleToolStripMenuItem";
            this.dirToggleToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.dirToggleToolStripMenuItem.Text = "Toggle !";
            this.dirToggleToolStripMenuItem.Click += new System.EventHandler(this.dirToggleToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(995, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileRClikMenu
            // 
            this.fileRClikMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToggleToolStripMenuItem});
            this.fileRClikMenu.Name = "fileRClikMenu";
            this.fileRClikMenu.Size = new System.Drawing.Size(118, 26);
            // 
            // fileToggleToolStripMenuItem
            // 
            this.fileToggleToolStripMenuItem.Name = "fileToggleToolStripMenuItem";
            this.fileToggleToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.fileToggleToolStripMenuItem.Text = "Toggle !";
            this.fileToggleToolStripMenuItem.Click += new System.EventHandler(this.fileToggleToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(322, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Quick Format";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 359);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.fileView);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "UI";
            this.Text = "File Explorer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.dirRClikMenu.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.fileRClikMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ListView fileView;
        private System.Windows.Forms.ColumnHeader lnameClm;
        private System.Windows.Forms.ColumnHeader extClm;
        private System.Windows.Forms.ColumnHeader hidClm;
        private System.Windows.Forms.ColumnHeader sysClm;
        private System.Windows.Forms.ColumnHeader writepClm;
        private System.Windows.Forms.ColumnHeader lastcClm;
        private System.Windows.Forms.ColumnHeader deletClm;
        private System.Windows.Forms.ColumnHeader sizeClm;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ContextMenuStrip dirRClikMenu;
        private System.Windows.Forms.ToolStripMenuItem dirToggleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ContextMenuStrip fileRClikMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToggleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}

