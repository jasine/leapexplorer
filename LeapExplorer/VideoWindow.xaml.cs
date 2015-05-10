using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Threading;
using DoubanFM.Bass;
using System.Windows.Controls.Primitives;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Declarations.Players;
using Implementation;
using Microsoft.Win32;

namespace LeapExplorer
{
    /// <summary>
    /// DetialWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VideoWindow : Window
    {
        private ImageSource img;
        private double size, w, h;
        private Storyboard stdStart, stdEnd, stdEnd2, stdVideoFinish;
        public static VideoWindow Instance { get; private set; }

        private string  fileName;

        private IMediaPlayerFactory m_factory;
        private IVideoPlayer m_player;
        private IMedia m_media;

        public  static  VideoWindow GetInstance(FileInfo imgSrc, FileInfo videoSrc)
        {
            
            if (Instance != null)
                Instance.Close();
            Instance = new VideoWindow(imgSrc, videoSrc);
            return Instance;
        }

        private VideoWindow(FileInfo imgSrc, FileInfo videoSrc)
            //public VideoWindow()
        {
            InitializeComponent();

            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.Left = 0;
            this.Top = 0;
            // this.Topmost = true;
            //imgSrc = new FileInfo(@"C:\Users\Administrator\Videos\thumbnails\b.jpg");
            //videoSrc = new FileInfo(@"C:\Users\Administrator\Videos\b.mp4");

            img = new BitmapImage(new Uri(imgSrc.FullName));
            fileName = videoSrc.FullName;

            //根据分辨率不同，调整DetialWindow出现的位置
            if (this.Width > 1300)
            {
                size = SystemParameters.PrimaryScreenWidth*0.415;
            }
            else if (this.Width < 1300 && this.Width > 1000)
            {
                size = SystemParameters.PrimaryScreenWidth*0.415;
            }
            if (img.Width >= img.Height)
            {
                w = size;
                h = size/img.Width*img.Height;
            }
            else
            {
                h = size;
                w = size/img.Height*img.Width;
            }
            gd.Background = new ImageBrush(img);


            stdStart = (Storyboard) this.Resources["start"];
            stdEnd = (Storyboard) this.Resources["end"];
            stdEnd2 = (Storyboard) this.Resources["end_2"];
            stdVideoFinish = (Storyboard) Resources["VideoFinish"];

            stdStart.Completed += (a, b) =>
                {


                    //stdMiddle.Begin();
                    TimeSplit.Visibility = Visibility.Visible;
                    var datPrs = new DoubleAnimation(0, 600, new Duration(TimeSpan.FromMilliseconds(1000)));
                    process.BeginAnimation(ProgressBar.WidthProperty, datPrs);

                    ///播放视频
                    m_media = m_factory.CreateMedia<IMediaFromFile>(fileName);
                    m_media.Events.DurationChanged += Events_DurationChanged;
                    m_media.Events.StateChanged += Events_StateChanged;
                    m_player.Open(m_media);

                    m_media.Parse(true);
                    m_player.Play();
                    //System.Drawing.Size s = m_player.GetVideoSize(0);
                    //m_player.TakeSnapShot(0, @"C:\");
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(1000);
                    timer.Tick += (c, d) =>
                        {
                            gd.Background = null;
                            timer.Stop();
                        };
                    timer.Start();
                };

            stdEnd.Completed += (c, d) =>
                {
                    CloseAnmit();
                    stdEnd2.Begin();
                };
            stdEnd2.Completed += (e, f) => { this.Close(); };
            this.Loaded += MainWindow_Loaded;

            m_factory = new MediaPlayerFactory();
            m_player = m_factory.CreatePlayer<IVideoPlayer>();
            m_videoImage.Initialize(m_player.CustomRendererEx);

            m_player.Events.PlayerPositionChanged +=
                new EventHandler<MediaPlayerPositionChanged>(Events_PlayerPositionChanged);
            m_player.Events.TimeChanged += new EventHandler<MediaPlayerTimeChanged>(Events_TimeChanged);
            m_player.Events.MediaEnded += new EventHandler(Events_MediaEnded);
            m_player.Events.PlayerStopped += new EventHandler(Events_PlayerStopped);
        }

        private  void Events_PlayerStopped(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(InitControls));
        }

        private void Events_MediaEnded(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(InitControls));
        }

        private void InitControls()
        {
            gd.Background = new ImageBrush(img);
            //m_videoImage.Visibility = Visibility.Hidden;
            process.Value = 0;
            TimeTotal.Content = "00:00:00";
            TimeNow.Content = "00:00:00";
            CloseThis();
            //stdVideoFinish.Begin();
            //stdVideoFinish.Completed += (a, b) => CloseThis();
        }

        private void Events_TimeChanged(object sender, MediaPlayerTimeChanged e)
        {
            this.Dispatcher.BeginInvoke(
                new Action(
                    delegate { TimeNow.Content = TimeSpan.FromMilliseconds(e.NewTime).ToString().Substring(0, 8); }));
        }

        private void Events_PlayerPositionChanged(object sender, MediaPlayerPositionChanged e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate { process.Value = (double) e.NewPosition*100; }));
        }

        private void Events_StateChanged(object sender, MediaStateChange e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate { }));
        }

        private void Events_DurationChanged(object sender, MediaDurationChange e)
        {
            this.Dispatcher.BeginInvoke(
                new Action(
                    delegate
                        { TimeTotal.Content = TimeSpan.FromMilliseconds(e.NewDuration).ToString().Substring(0, 8); }));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            stdStart.Begin();
            //stdMiddle.Begin();
            playStatue.Source = new BitmapImage(PackUriHelper.CreatePackUri("/resouces/pause.png"));
            double w2 = img.Width, h2 = img.Height;

            double times1 = img.Width/(SystemParameters.PrimaryScreenWidth - 80.0);
            double times2 = img.Height/(SystemParameters.PrimaryScreenHeight - 60.0);
            double times = times1 > times2 ? times1 : times2;
            w2 = w2/times;
            h2 = h2/times;

            var opacityGrid = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0)));
            var widthx = new DoubleAnimation(w, w2, new Duration(TimeSpan.FromMilliseconds(500)));
            var heightx = new DoubleAnimation(h, h2, new Duration(TimeSpan.FromMilliseconds(500)));

            gd.BeginAnimation(Grid.OpacityProperty, opacityGrid);
            gd.BeginAnimation(Grid.WidthProperty, widthx);
            gd.BeginAnimation(Grid.HeightProperty, heightx);
        }


        public void CloseThis()
        {
            stdVideoFinish.Begin();
            playStatue.Opacity = 0;
            m_player.Stop();
            stdEnd.Begin();
            TimeNow.Visibility = Visibility.Collapsed;
            TimeSplit.Visibility = Visibility.Collapsed;
            TimeTotal.Visibility = Visibility.Collapsed;
            ;
            var datPrs = new DoubleAnimation(600, 0, new Duration(TimeSpan.FromMilliseconds(600)));
            process.BeginAnimation(ProgressBar.WidthProperty, datPrs);
        }

        private void CloseAnmit()
        {
            //var opacityGrid = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0)));
            var widthx = new DoubleAnimation(gd.ActualWidth, w, new Duration(TimeSpan.FromMilliseconds(300)));
            var heightx = new DoubleAnimation(gd.ActualHeight, h, new Duration(TimeSpan.FromMilliseconds(300)));

            //wb.BeginAnimation(Grid.OpacityProperty, opacityGrid);
            gd.BeginAnimation(Grid.WidthProperty, widthx);
            gd.BeginAnimation(Grid.HeightProperty, heightx);
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
            if (m_player.Position*1000 >= 1)
            {
                //Visibility vi = gd.Visibility;
                //var opacityImg = new DoubleAnimation(07, 0, new Duration(TimeSpan.FromSeconds(1000)));
                //playStatue.BeginAnimation(Image.OpacityProperty, opacityImg);
                playStatue.Opacity = 0.7;
            }
        }

        private void gd_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!m_player.IsPlaying)
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
            if (m_player.IsPlaying)
            {
                playStatue.Source = new BitmapImage(PackUriHelper.CreatePackUri("/resouces/play.png"));
                m_player.Pause();
                playStatue.Opacity = 0.7;
            }
            else
            {
                playStatue.Source = new BitmapImage(PackUriHelper.CreatePackUri("/resouces/pause.png"));
                m_player.Play();
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
            m_player.Position = (float) precent;
            process.Value = precent*100;
        }


        public void Forword()
        {
            ChangePlayingTime(m_player.Position = (float) (m_player.Position + 0.1));
        }

        public void Backword()
        {
            ChangePlayingTime(m_player.Position = (float) (m_player.Position - 0.1));
        }


        private void VideoWindow_OnClosing(object sender, CancelEventArgs e)
        {
            m_player.Stop();
            m_player.Dispose();
            m_media.Dispose();
        }
    }
}