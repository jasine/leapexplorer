using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Threading;
using DoubanFM.Bass;
using Id3Lib;
using Mp3Lib;
using System.IO.Packaging;

namespace LeapExplorer
{
    /// <summary>
    /// DetialWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MusicWindow 
    {
        private readonly ImageSource _img;
        private readonly double _w,_h,_size;
        private readonly Storyboard _stdMusicFinish,_stdEnd2;
        private Storyboard _stdEnd;
        private readonly Storyboard _stdStart;
        //private Point lastCenter;
        public static MusicWindow Instace { get; private set; }

        public static MusicWindow GetInstance(FileInfo imgSrc)
        {
            if (Instace != null)
                Instace.Close();
            Instace = new MusicWindow(imgSrc);
            return Instace;
        }

        private readonly DispatcherTimer _timerLyric;
        private readonly DispatcherTimer _timerPhoto;
        private readonly string _fileName;
        private int _photoIndex;

        private MusicWindow(FileInfo imgSrc)
        {
            InitializeComponent();
            SpectrumAnalyzer.RegisterSoundPlayer(BassEngine.Instance);
            string file = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" +
                          Path.GetFileNameWithoutExtension(imgSrc.Name) + ".mp3";
            _fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\Lyrics\"
                       + Path.GetFileNameWithoutExtension(imgSrc.Name) + ".lrc";

            TagHandler tagHandler = new TagHandler(new Mp3File(file).TagModel);
            ItemTitle.Text = tagHandler.Title != ""
                                 ? tagHandler.Title
                                 : Path.GetFileNameWithoutExtension(imgSrc.Name);
            OrilTime.Text = tagHandler.Track != "" ? "发源时间：" + tagHandler.Genre : "";

            Kind.Text = tagHandler.Track != "" ? "类别：" + tagHandler.Artist : "";
            // != "" ? _tagHandler.Artist : "未知类别";
            OrilLoca.Text = tagHandler.Track != "" ? "起源地：" + tagHandler.Album : "";
            // != "" ? "《" + _tagHandler.Album + "》" : "未知专辑";
            Year.Text = tagHandler.Track != "" ? "入遗时间：" + tagHandler.Year : "";
            Rank.Text = tagHandler.Track != "" ? "级别：" + tagHandler.Track : "";


            _timerLyric = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(200)};
            _timerLyric.Tick += (a, b) => ShowLyric();
            _timerLyric.Start();

            DirectoryInfo phothDir =
                new DirectoryInfo(imgSrc.Directory.FullName + @"\" + Path.GetFileNameWithoutExtension(imgSrc.Name));
            if (phothDir.Exists)
            {
                photo.ImageUrl = imgSrc.FullName;
                photo.Width = gd.Width;
                photo.Height = gd.Height;
                List<FileInfo> photos = new List<FileInfo>();

                photos.AddRange(phothDir.GetFiles("*.jpg", SearchOption.AllDirectories));
                photos.AddRange(phothDir.GetFiles("*.png", SearchOption.AllDirectories));
                photos.Add(imgSrc);
                _timerPhoto = new DispatcherTimer {Interval = TimeSpan.FromSeconds(4)};
                _timerPhoto.Tick += (a, b) =>
                    {
                        photo.ImageUrl = photos[_photoIndex++].FullName;
                        _photoIndex = _photoIndex%photos.Count;
                    };
                _timerPhoto.Start();
            }

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            Left = 0;
            Top = 0;
            // this.Topmost = true;

            _img = new BitmapImage(new Uri(imgSrc.FullName));

            //根据分辨率不同，调整DetialWindow出现的位置
            if (Width > 1300)
            {
                _size = SystemParameters.PrimaryScreenWidth*0.415;
            }
            else if (Width < 1300 && Width > 1000)
            {
                _size = SystemParameters.PrimaryScreenWidth*0.415;
            }
            gd.Background = new ImageBrush(_img);
            if (_img.Width >= _img.Height)
            {
                _w = _size;
                _h = _size/_img.Width*_img.Height;
            }
            else
            {
                _h = _size;
                _w = _size/_img.Height*_img.Width;
            }

            _stdStart = (Storyboard) Resources["start"];
            _stdMusicFinish = (Storyboard) Resources["MusicFinish"];
            _stdEnd = (Storyboard) Resources["end"];
            _stdEnd2 = (Storyboard) Resources["end_2"];

            _stdStart.Completed += (a, b) =>
                {
                    //stdMiddle.Begin();
                    MusicInfo.Visibility = Visibility.Visible;
                    //MusicInfo.Opacity = 0;
                    var datImg = new DoubleAnimation(0, -1*SystemParameters.PrimaryScreenWidth*0.23,
                                                     new Duration(TimeSpan.FromMilliseconds(700)));
                    var datInfo = new DoubleAnimation(0, SystemParameters.PrimaryScreenWidth*0.23,
                                                      new Duration(TimeSpan.FromMilliseconds(700)));
                    var datPrs = new DoubleAnimation(0, 300, new Duration(TimeSpan.FromMilliseconds(2000)));

                    tlt.BeginAnimation(TranslateTransform.XProperty, datImg);
                    infoTlt.BeginAnimation(TranslateTransform.XProperty, datInfo);
                    process.BeginAnimation(WidthProperty, datPrs);
                };
            _stdEnd.Completed += (c, d) =>
                {
                    CloseAnmit();
                    _stdEnd2.Begin();
                };
            _stdEnd2.Completed += (e, f) => Close();
            Loaded += MainWindow_Loaded;
        }


        /// <summary>
        /// 显示歌词
        /// </summary>
        private void ShowLyric()
        {
            string sss = BassEngine.Instance.ChannelPosition.Minutes.ToString("00") + ":" +
                         BassEngine.Instance.ChannelPosition.Seconds.ToString("00");
            if (!File.Exists(_fileName))
            {
                lyric.Text = "";
            }
            else
            {
                string sttr;
                UTF8Encoding encode = new UTF8Encoding();
                StreamReader objFile = new StreamReader(_fileName, encode);
                sttr = objFile.ReadLine();
                while (sttr != null)
                {
                    int temp = sttr.IndexOf(sss, StringComparison.Ordinal);
                    if (temp != -1)
                    {
                        if (sttr.Length > sttr.LastIndexOf("]", StringComparison.Ordinal) + 1)
                            lyric.Text =
                                (sttr.Substring(sttr.LastIndexOf("]", StringComparison.Ordinal) + 1, sttr.Length - sttr.LastIndexOf("]", StringComparison.Ordinal) - 1) +
                                 "\r\n");
                    }
                    sttr = objFile.ReadLine();
                }
                objFile.Close();
                //objFile.Dispose();
            }
            double a = BassEngine.Instance.ChannelPosition.Seconds + BassEngine.Instance.ChannelPosition.Minutes*60;
            double b = BassEngine.Instance.ChannelLength.Seconds + BassEngine.Instance.ChannelLength.Minutes*60;
            process.Value = a/b*100;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _stdStart.Begin();
            //stdMiddle.Begin();
            playStatue.Source = new BitmapImage(PackUriHelper.CreatePackUri("/resouces/pause.png"));

            var opacityGrid = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0)));
            var widthx = new DoubleAnimation(_w, 400, new Duration(TimeSpan.FromMilliseconds(500)));
            var heightx = new DoubleAnimation(_h, 400, new Duration(TimeSpan.FromMilliseconds(500)));

            gd.BeginAnimation(OpacityProperty, opacityGrid);
            gd.BeginAnimation(WidthProperty, widthx);
            gd.BeginAnimation(HeightProperty, heightx);

            var datImg = new DoubleAnimation(0, -1*SystemParameters.PrimaryScreenHeight*0.1,
                                             new Duration(TimeSpan.FromMilliseconds(500)));
            //var datInfo = new DoubleAnimation(0, deltaY * 1.5, new Duration(TimeSpan.FromMilliseconds(200)));
            tlt.BeginAnimation(TranslateTransform.YProperty, datImg);
            infoTlt.BeginAnimation(TranslateTransform.YProperty, datImg);
        }


        public void CloseThis()
        {
            if (_stdEnd != null)
            {
                _stdMusicFinish.Begin();
                playStatue.Opacity = 0;
                _timerLyric.Stop();
                //timerLyric = null;
                lyric.Text = "";
                var datImg = new DoubleAnimation(-1*SystemParameters.PrimaryScreenWidth*0.2, 0,
                                                 new Duration(TimeSpan.FromMilliseconds(700)));
                var datInfo = new DoubleAnimation(SystemParameters.PrimaryScreenWidth*0.2, 0,
                                                  new Duration(TimeSpan.FromMilliseconds(700)));
                tlt.BeginAnimation(TranslateTransform.XProperty, datImg);
                infoTlt.BeginAnimation(TranslateTransform.XProperty, datInfo);
                BassEngine.Instance.Stop();
                _stdEnd.Begin();
                _stdEnd = null;
            }
        }

        private void CloseAnmit()
        {
            MusicInfo.Visibility = Visibility.Hidden;
            //var opacityGrid = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0)));
            var widthx = new DoubleAnimation(gd.ActualWidth, _w, new Duration(TimeSpan.FromMilliseconds(300)));
            var heightx = new DoubleAnimation(gd.ActualHeight, _h, new Duration(TimeSpan.FromMilliseconds(300)));

            //wb.BeginAnimation(Grid.OpacityProperty, opacityGrid);
            gd.BeginAnimation(WidthProperty, widthx);
            gd.BeginAnimation(HeightProperty, heightx);

            var datImg = new DoubleAnimation(-1*SystemParameters.PrimaryScreenHeight*0.1, 0,
                                             new Duration(TimeSpan.FromMilliseconds(300)));
            //var datInfo = new DoubleAnimation(0, deltaY * 1.5, new Duration(TimeSpan.FromMilliseconds(200)));
            tlt.BeginAnimation(TranslateTransform.YProperty, datImg);
        }


        private void main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(gd);
            if (p.X < gd.ActualWidth && p.Y < gd.ActualHeight && p.X > 0 && p.Y > 0)
                return;
            Point q = e.GetPosition(process);
            if (q.X < process.ActualWidth + 5 && q.Y < process.ActualHeight + 5 && q.X > -5 && q.Y > -5)
                return;

            CloseThis();
        }


        private void gd_MouseEnter(object sender, MouseEventArgs e)
        {
            if (BassEngine.Instance.ChannelPosition.Seconds >= 1)
            {
                //var opacityImg = new DoubleAnimation(07, 0, new Duration(TimeSpan.FromSeconds(1000)));
                //playStatue.BeginAnimation(Image.OpacityProperty, opacityImg);
                playStatue.Opacity = 0.7;
            }
        }

        private void gd_MouseLeave(object sender, MouseEventArgs e)
        {
            if (BassEngine.Instance.CanPlay)
                return;
            //var opacityImg = new DoubleAnimation(0, 0.7, new Duration(TimeSpan.FromSeconds(1000)));
            //playStatue.BeginAnimation(Image.OpacityProperty, opacityImg);       
            playStatue.Opacity = 0;
        }

        private void playStatue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeStatue();
        }

        public void ChangeStatue()
        {
            if (BassEngine.Instance.CanPause)
            {
                playStatue.Source = new BitmapImage(PackUriHelper.CreatePackUri("/resouces/play.png"));
                _timerLyric.Stop();
                _timerPhoto.Stop();
                BassEngine.Instance.Pause();
                playStatue.Opacity = 0.7;
            }
            else
            {
                playStatue.Source = new BitmapImage(PackUriHelper.CreatePackUri("/resouces/pause.png"));
                _timerLyric.Start();
                _timerPhoto.Start();
                BassEngine.Instance.Play();
                playStatue.Opacity = 0;
            }
        }

        private void process_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(process);
            double precent = p.X/process.Width;
            ChangePlayingTime(precent);
        }

        private void ChangePlayingTime(double precent)
        {
            precent = precent > 1 ? 1 : precent;
            precent = precent < 0 ? 0 : precent;
            BassEngine.Instance.ChannelPosition =
                TimeSpan.FromMilliseconds(BassEngine.Instance.ChannelLength.TotalMilliseconds*precent);
            ShowLyric();
            //lyric.Text = "";
        }


        public void Forword()
        {
            ChangePlayingTime(BassEngine.Instance.ChannelPosition.TotalMilliseconds/
                              BassEngine.Instance.ChannelLength.TotalMilliseconds + 0.1);
        }

        public void Backword()
        {
            ChangePlayingTime(BassEngine.Instance.ChannelPosition.TotalMilliseconds/
                              BassEngine.Instance.ChannelLength.TotalMilliseconds - 0.1);
        }
    }
}