// Copyright(C) 2002-2003 Hugo Rumayor Montemayor, All rights reserved.
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Id3Lib;
using Mp3Lib;
using Utils;

namespace TagEditor
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ID3AdapterEdit : System.Windows.Forms.Form
    {
        #region fields

        private System.Windows.Forms.TabControl _tabControlLyrics;
        private IContainer components;
		private System.Windows.Forms.Button _buttonOK;
        private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private BindingSource _tagHandlerBindingSource;
        private BindingSource _mp3FileBindingSource;

        #endregion

        /// <summary>
        /// MP3.File reference
        /// encapsulates both the TagModel and the audio information
        /// </summary>
        private Mp3File _mp3File = null;
        private ToolTip toolTip1;
        private TabPage _tabPageGeneric;
        private Button _addPicture;
        private Button _removePicture;
        private PictureBox _artPictureBox;
        private ComboBox _comboBoxGenre;
        private Label _labelGenre;
        private TextBox _textBoxYear;
        private Label _labelYear;
        private Label _labelAlbum;
        private Label _labelArtist;
        private TextBox _textBoxAlbum;
        private TextBox _textBoxTitle;
        private Label _labelTitle;
        private Label _labelTrackNo;
        private ComboBox _textBoxTrackNo;
        private ComboBox _textBoxArtist;

        /// <summary>
		/// Tag Handler reference
        /// encapsulates TagModel
		/// </summary>
        private TagHandler _tagHandler = null;


        public ID3AdapterEdit(Mp3File mp3File)
		{
            _mp3File = mp3File;
            _tagHandler = new TagHandler(_mp3File.TagModel);
			InitializeComponent();
            _textBoxTrackNo.Text = _tagHandler.Track;
            _textBoxArtist.Text = _tagHandler.Artist;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ID3AdapterEdit));
            this._tabControlLyrics = new System.Windows.Forms.TabControl();
            this._tabPageGeneric = new System.Windows.Forms.TabPage();
            this._addPicture = new System.Windows.Forms.Button();
            this._removePicture = new System.Windows.Forms.Button();
            this._artPictureBox = new System.Windows.Forms.PictureBox();
            this._tagHandlerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._textBoxTrackNo = new System.Windows.Forms.ComboBox();
            this._textBoxArtist = new System.Windows.Forms.ComboBox();
            this._comboBoxGenre = new System.Windows.Forms.ComboBox();
            this._labelGenre = new System.Windows.Forms.Label();
            this._textBoxYear = new System.Windows.Forms.TextBox();
            this._labelYear = new System.Windows.Forms.Label();
            this._labelAlbum = new System.Windows.Forms.Label();
            this._labelArtist = new System.Windows.Forms.Label();
            this._textBoxAlbum = new System.Windows.Forms.TextBox();
            this._labelTrackNo = new System.Windows.Forms.Label();
            this._textBoxTitle = new System.Windows.Forms.TextBox();
            this._labelTitle = new System.Windows.Forms.Label();
            this._mp3FileBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._buttonOK = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._tabControlLyrics.SuspendLayout();
            this._tabPageGeneric.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._artPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._tagHandlerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._mp3FileBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // _tabControlLyrics
            // 
            this._tabControlLyrics.Controls.Add(this._tabPageGeneric);
            this._tabControlLyrics.Location = new System.Drawing.Point(8, 7);
            this._tabControlLyrics.Name = "_tabControlLyrics";
            this._tabControlLyrics.SelectedIndex = 0;
            this._tabControlLyrics.Size = new System.Drawing.Size(552, 274);
            this._tabControlLyrics.TabIndex = 0;
            // 
            // _tabPageGeneric
            // 
            this._tabPageGeneric.AutoScroll = true;
            this._tabPageGeneric.Controls.Add(this._addPicture);
            this._tabPageGeneric.Controls.Add(this._removePicture);
            this._tabPageGeneric.Controls.Add(this._artPictureBox);
            this._tabPageGeneric.Controls.Add(this._textBoxTrackNo);
            this._tabPageGeneric.Controls.Add(this._textBoxArtist);
            this._tabPageGeneric.Controls.Add(this._comboBoxGenre);
            this._tabPageGeneric.Controls.Add(this._labelGenre);
            this._tabPageGeneric.Controls.Add(this._textBoxYear);
            this._tabPageGeneric.Controls.Add(this._labelYear);
            this._tabPageGeneric.Controls.Add(this._labelAlbum);
            this._tabPageGeneric.Controls.Add(this._labelArtist);
            this._tabPageGeneric.Controls.Add(this._textBoxAlbum);
            this._tabPageGeneric.Controls.Add(this._labelTrackNo);
            this._tabPageGeneric.Controls.Add(this._textBoxTitle);
            this._tabPageGeneric.Controls.Add(this._labelTitle);
            this._tabPageGeneric.Location = new System.Drawing.Point(4, 22);
            this._tabPageGeneric.Name = "_tabPageGeneric";
            this._tabPageGeneric.Size = new System.Drawing.Size(544, 248);
            this._tabPageGeneric.TabIndex = 0;
            this._tabPageGeneric.Text = "信息设置";
            // 
            // _addPicture
            // 
            this._addPicture.Location = new System.Drawing.Point(350, 212);
            this._addPicture.Name = "_addPicture";
            this._addPicture.Size = new System.Drawing.Size(91, 21);
            this._addPicture.TabIndex = 14;
            this._addPicture.Text = "添加图片";
            this._addPicture.Click += new System.EventHandler(this.addPicture_Click);
            // 
            // _removePicture
            // 
            this._removePicture.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._removePicture.Location = new System.Drawing.Point(447, 212);
            this._removePicture.Name = "_removePicture";
            this._removePicture.Size = new System.Drawing.Size(22, 21);
            this._removePicture.TabIndex = 13;
            this._removePicture.Text = "X";
            this._removePicture.Click += new System.EventHandler(this.removePicture_Click);
            // 
            // _artPictureBox
            // 
            this._artPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._artPictureBox.DataBindings.Add(new System.Windows.Forms.Binding("Image", this._tagHandlerBindingSource, "Picture", true));
            this._artPictureBox.Location = new System.Drawing.Point(316, 20);
            this._artPictureBox.Name = "_artPictureBox";
            this._artPictureBox.Size = new System.Drawing.Size(197, 186);
            this._artPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._artPictureBox.TabIndex = 12;
            this._artPictureBox.TabStop = false;
            // 
            // _tagHandlerBindingSource
            // 
            this._tagHandlerBindingSource.DataSource = typeof(Id3Lib.TagHandler);
            // 
            // _textBoxTrackNo
            // 
            this._textBoxTrackNo.Items.AddRange(new object[] {
            "世界名录",
            "国家名录",
            "省级名录"});
            this._textBoxTrackNo.Location = new System.Drawing.Point(74, 88);
            this._textBoxTrackNo.Name = "_textBoxTrackNo";
            this._textBoxTrackNo.Size = new System.Drawing.Size(211, 20);
            this._textBoxTrackNo.TabIndex = 11;
            // 
            // _textBoxArtist
            // 
            this._textBoxArtist.Items.AddRange(new object[] {
            "口头传统和表述",
            "表演艺术",
            "社会风俗、礼仪、节庆",
            "自然界和宇宙的知识实践",
            "传统手工艺技能"});
            this._textBoxArtist.Location = new System.Drawing.Point(74, 55);
            this._textBoxArtist.Name = "_textBoxArtist";
            this._textBoxArtist.Size = new System.Drawing.Size(211, 20);
            this._textBoxArtist.TabIndex = 11;
            // 
            // _comboBoxGenre
            // 
            this._comboBoxGenre.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._tagHandlerBindingSource, "Genre", true));
            this._comboBoxGenre.Items.AddRange(new object[] {
            "夏",
            "商",
            "周",
            "春秋/战国",
            "秦",
            "汉",
            "三国/晋",
            "南北朝",
            "隋",
            "唐",
            "五代十国",
            "宋",
            "元",
            "明",
            "清"});
            this._comboBoxGenre.Location = new System.Drawing.Point(74, 153);
            this._comboBoxGenre.Name = "_comboBoxGenre";
            this._comboBoxGenre.Size = new System.Drawing.Size(211, 20);
            this._comboBoxGenre.TabIndex = 11;
            // 
            // _labelGenre
            // 
            this._labelGenre.Location = new System.Drawing.Point(7, 156);
            this._labelGenre.Name = "_labelGenre";
            this._labelGenre.Size = new System.Drawing.Size(60, 17);
            this._labelGenre.TabIndex = 10;
            this._labelGenre.Text = "起源时间:";
            this._labelGenre.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _textBoxYear
            // 
            this._textBoxYear.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._tagHandlerBindingSource, "Year", true));
            this._textBoxYear.Location = new System.Drawing.Point(74, 185);
            this._textBoxYear.Name = "_textBoxYear";
            this._textBoxYear.Size = new System.Drawing.Size(211, 21);
            this._textBoxYear.TabIndex = 9;
            // 
            // _labelYear
            // 
            this._labelYear.Location = new System.Drawing.Point(5, 190);
            this._labelYear.Name = "_labelYear";
            this._labelYear.Size = new System.Drawing.Size(63, 19);
            this._labelYear.TabIndex = 8;
            this._labelYear.Text = "入遗时间:";
            this._labelYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _labelAlbum
            // 
            this._labelAlbum.Location = new System.Drawing.Point(12, 124);
            this._labelAlbum.Name = "_labelAlbum";
            this._labelAlbum.Size = new System.Drawing.Size(56, 15);
            this._labelAlbum.TabIndex = 7;
            this._labelAlbum.Text = "发源地:";
            this._labelAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _labelArtist
            // 
            this._labelArtist.Location = new System.Drawing.Point(3, 60);
            this._labelArtist.Name = "_labelArtist";
            this._labelArtist.Size = new System.Drawing.Size(65, 15);
            this._labelArtist.TabIndex = 6;
            this._labelArtist.Text = "类别:";
            this._labelArtist.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _textBoxAlbum
            // 
            this._textBoxAlbum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._tagHandlerBindingSource, "Album", true));
            this._textBoxAlbum.Location = new System.Drawing.Point(74, 120);
            this._textBoxAlbum.Name = "_textBoxAlbum";
            this._textBoxAlbum.Size = new System.Drawing.Size(211, 21);
            this._textBoxAlbum.TabIndex = 5;
            // 
            // _labelTrackNo
            // 
            this._labelTrackNo.Location = new System.Drawing.Point(-1, 88);
            this._labelTrackNo.Name = "_labelTrackNo";
            this._labelTrackNo.Size = new System.Drawing.Size(68, 15);
            this._labelTrackNo.TabIndex = 2;
            this._labelTrackNo.Text = "级别:";
            this._labelTrackNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _textBoxTitle
            // 
            this._textBoxTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._tagHandlerBindingSource, "Title", true));
            this._textBoxTitle.Location = new System.Drawing.Point(74, 22);
            this._textBoxTitle.Name = "_textBoxTitle";
            this._textBoxTitle.Size = new System.Drawing.Size(211, 21);
            this._textBoxTitle.TabIndex = 1;
            // 
            // _labelTitle
            // 
            this._labelTitle.Location = new System.Drawing.Point(11, 28);
            this._labelTitle.Name = "_labelTitle";
            this._labelTitle.Size = new System.Drawing.Size(56, 15);
            this._labelTitle.TabIndex = 0;
            this._labelTitle.Text = "名称:";
            this._labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _mp3FileBindingSource
            // 
            this._mp3FileBindingSource.DataSource = typeof(Mp3Lib.Mp3File);
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            this._errorProvider.DataMember = "";
            // 
            // _buttonOK
            // 
            this._buttonOK.Location = new System.Drawing.Point(202, 298);
            this._buttonOK.Name = "_buttonOK";
            this._buttonOK.Size = new System.Drawing.Size(72, 22);
            this._buttonOK.TabIndex = 1;
            this._buttonOK.Text = "确认";
            this._buttonOK.Click += new System.EventHandler(this.OnOkClick);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.Location = new System.Drawing.Point(282, 298);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(72, 22);
            this._buttonCancel.TabIndex = 2;
            this._buttonCancel.Text = "取消";
            this._buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // ID3AdapterEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(567, 332);
            this.Controls.Add(this._buttonCancel);
            this.Controls.Add(this._buttonOK);
            this.Controls.Add(this._tabControlLyrics);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._mp3FileBindingSource, "FileName", true, System.Windows.Forms.DataSourceUpdateMode.Never));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ID3AdapterEdit";
            this.ShowInTaskbar = false;
            this.Text = "MP3文件信息编辑器";
            this.Load += new System.EventHandler(this.ID3Edit_Load);
            this._tabControlLyrics.ResumeLayout(false);
            this._tabPageGeneric.ResumeLayout(false);
            this._tabPageGeneric.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._artPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._tagHandlerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._mp3FileBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void ID3Edit_Load(object sender, System.EventArgs e)
		{
			// If there is no model
			if(_mp3File == null)
				throw new ApplicationException("No data to edit on load");

            // set up databindings
            this._tagHandlerBindingSource.DataSource = _tagHandler;
            this._mp3FileBindingSource.DataSource = _mp3File;

            //this._textBoxSampleRate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._mp3FileBindingSource, "Audio.AudioHeader.SamplesPerSecond", true));
            // Add the delegates to the events.      
        }

		private void OnOkClick(object sender, System.EventArgs e)
		{
            // picturebox is not a control that can receive focus,
            // so it doesn't save its image with the databinding mechanism.
            _tagHandler.Picture  = this._artPictureBox.Image;
		    _tagHandler.Track = _textBoxTrackNo.Text.Trim();
		    _tagHandler.Artist = _textBoxArtist.Text.Trim();
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void addPicture_Click(object sender, System.EventArgs e)
		{
			_openFileDialog.Multiselect= false;
			_openFileDialog.CheckFileExists = true;
			_openFileDialog.CheckPathExists = true;
			_openFileDialog.Title = "Select a picture";
            _openFileDialog.Filter = "Picture Files(*.bmp;*.jpg;*.gif;*.png)|*.bpm;*.jpg;*.gif;*.png|Bitmap (*.bmp)|*.bmp|jpg (*.jpg)|*.jpg|jpeg (*.jpeg)|*.jpeg|gif (*.gif)|*.gif|gif (*.png)|*.png";
			if(_openFileDialog.ShowDialog() == DialogResult.OK)
			{ 
				using (FileStream stream = File.Open(_openFileDialog.FileName,FileMode.Open,FileAccess.Read,FileShare.Read))
                {
				    byte[] buffer = new Byte[stream.Length];
				    stream.Read(buffer,0,buffer.Length);
				    if(buffer != null)
				    {
					    MemoryStream memoryStream = new MemoryStream(buffer,false);
					    this._artPictureBox.Image = Image.FromStream(memoryStream);
				    }
			    }
			}
		}

		private void removePicture_Click(object sender, System.EventArgs e)
		{
			this._artPictureBox.Image = null;
        }

        
	}
}
