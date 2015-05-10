using System.Windows.Forms;
using System.ComponentModel;

namespace TagEditor
{
    partial class MainForm
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
            if( disposing && (_presenter != null) )
            {
                _presenter.Dispose();
            }
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
        protected void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this._scanMenuItem = new System.Windows.Forms.MenuItem();
            this._mainListBox = new System.Windows.Forms.ListBox();
            this._listBoxContextMenu = new System.Windows.Forms.ContextMenu();
            this._editListBoxMenuItem = new System.Windows.Forms.MenuItem();
            this._launchListBoxMenuItem = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // _mainMenu
            // 
            this._mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._scanMenuItem});
            // 
            // _scanMenuItem
            // 
            this._scanMenuItem.Index = 0;
            this._scanMenuItem.Text = "浏览";
            this._scanMenuItem.Click += new System.EventHandler(this._scanMenuItem_Click);
            // 
            // _mainListBox
            // 
            this._mainListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._mainListBox.ContextMenu = this._listBoxContextMenu;
            this._mainListBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._mainListBox.ItemHeight = 16;
            this._mainListBox.Location = new System.Drawing.Point(10, 8);
            this._mainListBox.Name = "_mainListBox";
            this._mainListBox.Size = new System.Drawing.Size(653, 308);
            this._mainListBox.TabIndex = 0;
            this._mainListBox.DoubleClick += new System.EventHandler(this._mainListBox_DoubleClick);
            this._mainListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this._mainListBox_MouseDown);
            // 
            // _listBoxContextMenu
            // 
            this._listBoxContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._editListBoxMenuItem,
            this._launchListBoxMenuItem});
            // 
            // _editListBoxMenuItem
            // 
            this._editListBoxMenuItem.Index = 0;
            this._editListBoxMenuItem.Text = "Edit";
            this._editListBoxMenuItem.Click += new System.EventHandler(this._mainListBoxMenu_EditTag);
            // 
            // _launchListBoxMenuItem
            // 
            this._launchListBoxMenuItem.Index = 1;
            this._launchListBoxMenuItem.Text = "Launch";
            this._launchListBoxMenuItem.Click += new System.EventHandler(this._mainListBoxMenu_Launch);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(672, 324);
            this.Controls.Add(this._mainListBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this._mainMenu;
            this.Name = "MainForm";
            this.Text = "MP3文件信息编辑器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);

        }
        #endregion

        #region fields
        protected System.Windows.Forms.MainMenu _mainMenu;
        protected System.Windows.Forms.MenuItem _scanMenuItem;
        protected System.Windows.Forms.ListBox _mainListBox;
        protected System.Windows.Forms.ContextMenu _listBoxContextMenu;
        protected System.Windows.Forms.MenuItem _editListBoxMenuItem;
        protected System.Windows.Forms.MenuItem _launchListBoxMenuItem;
        #endregion

    }
}