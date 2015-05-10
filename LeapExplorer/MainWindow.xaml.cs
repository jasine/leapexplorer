using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using DeviceHelper;
using DoubanFM.Bass;
using Id3Lib;
using Leap;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Mp3Lib;
using Path = System.IO.Path;


namespace LeapExplorer
{
    public partial class MainWindow 
    {
        #region 私有变量

        private readonly LeapHelper _leapHelper;

        private List<FileInfo> _images;
        private int _currentIndex;

        private DirectoryInfo _myMusicDir, _myCoverDir, _myLyricDir, _myVideoDir;

        private volatile DetialWindow _detialWindow;
        private volatile MusicWindow _musicWindow;
        private volatile VideoWindow _videoWindow;

        private readonly Storyboard _stdStart;
        private Storyboard _stdEnd;

        /// <summary>
        /// Time until skeleton ceases to be highlighted.
        /// </summary>
        private DateTime _highlightTime = DateTime.MinValue;

        /// <summary>
        /// The ID of the skeleton to highlight.
        /// </summary>
        private int _highlightId = -1;

        #endregion

        #region 载入与初始化

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _leapHelper = new LeapHelper();
            }
            catch (Exception)
            {
                MessageBox.Show("LeapMotion启动失败，请检查连接或驱动");
            }

            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            Top = 0;
            Left = 0;

            flow.Width = Width;
            flow.Height = Height;
            flow.IndexChanged += flow_IndexChanged;
            flow.CenterCoverClicked += flow_CenterCoverClicked;

            flow.Cache = new ThumbnailManager();
            LoadFiles();

            slider.Minimum = 0;
            slider.Maximum = flow.Count - 1;

            Loaded += Window_Loaded;
            List<GestureType> gestureses = new List<GestureType>
                {
                    GestureType.JoinedHands,
                    GestureType.LeftHandHalfLift,
                    GestureType.Pull,
                    GestureType.Push,
                    GestureType.SwipLeft,
                    GestureType.SwipRight,
                    GestureType.ZoomIn,
                    GestureType.ZoomOut
                };
          

            _leapHelper.Init();
            if (_leapHelper.IsConnected)
            {
                //_leapHelper.Init();
                _leapHelper.Listener.LeapSwipeReady += ListenerLeapSwipeReady;
                _leapHelper.Listener.LeapFingerReady += Listener_LeapFingerReady;
                _leapHelper.Listener.LeapTapScreenReady += ListenerLeapTapScreenReady;
                _leapHelper.Listener.LeapCircleReady += Listener_LeapCircleReady;
            }


            _stdStart = (Storyboard) Resources["sb_start"];
            _stdStart.Begin();

            InitBassEngine(); //初始化播放器
            ChangeFileInfo(); //显示文件信息           
            

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// 初始化音乐播放器
        /// </summary>
        private void InitBassEngine()
        {
            //歌曲播放完毕
            //SpectrumAnalyzer.RegisterSoundPlayer(BassEngine.Instance);
            BassEngine.ExplicitInitialize();
            BassEngine.Instance.TrackEnded += delegate
                {
                    if (_musicWindow != null)
                    {
                        _musicWindow.CloseThis();
                        _musicWindow = null;
                    }
                };
            //音乐加载成功
            BassEngine.Instance.OpenSucceeded += delegate
                {
                    Debug.WriteLine(" 音乐加载成功");
                    BassEngine.Instance.Volume = 1;
                    BassEngine.Instance.Play();
                };
            //打开音乐失败
            BassEngine.Instance.OpenFailed += delegate
                {
                    if (_musicWindow != null)
                    {
                        _musicWindow.CloseThis();
                        _musicWindow = null;
                    }
                };

            //绑定音量设置
        }

        #endregion

        #region Kinect相关处理

        #region 事件处理

       

       

        /// <summary>
        /// 检查是否追踪双手
        /// </summary>
        /// <param name="skeleton"></param>
        /// <returns></returns>
        

        #endregion

        #endregion

        #region LeapMotion相关事件

        private void Listener_LeapCircleReady(object sender)
        {
            if (_musicWindow != null)
            {
                Action action = () => _musicWindow.ChangeStatue();
                Dispatcher.BeginInvoke(DispatcherPriority.Send, action);
            }
            else if (_videoWindow != null)
            {
                Action action = () => _videoWindow.ChangeStatue();
                Dispatcher.BeginInvoke(DispatcherPriority.Send, action);
            }
        }

        private void ListenerLeapTapScreenReady(object sender)
        {
            if (_detialWindow != null || _musicWindow != null || _videoWindow != null)
            {
                Action action2;
                if (_detialWindow != null)
                {
                    action2 = () => _detialWindow.CloseThis();
                }
                else if (_musicWindow != null)
                {
                    action2 = () => _musicWindow.CloseThis();
                }
                else
                {
                    action2 = () => _videoWindow.CloseThis();
                }
                Dispatcher.BeginInvoke(DispatcherPriority.Send, action2).Completed += (a, b) =>
                    {
                        _detialWindow = null;
                        _musicWindow = null;
                        _videoWindow = null;
                    };
            }
            else
            {
                Action action = OpenSubWindow;
                Dispatcher.BeginInvoke(DispatcherPriority.Send, action);
            }
        }

        private void Listener_LeapFingerReady(object sender, Finger first, Finger second)
        {
            if (_detialWindow != null)
            {
                Action action1, action2;
                Point3D left = new Point3D(first.TipPosition.x*3.5,
                                           -3.5*(first.TipPosition.y - 100) + SystemParameters.PrimaryScreenHeight/2,
                                           first.TipPosition.z);
                Point3D right = new Point3D(second.TipPosition.x*3.5,
                                            -3.5*(second.TipPosition.y - 100) + SystemParameters.PrimaryScreenHeight/2,
                                            second.TipPosition.z);
                if (first.TipPosition.x <= second.TipPosition.x)
                {
                    action1 = () => _detialWindow.SetHandLeftPoint3D(left);
                    action2 = () => _detialWindow.SetHandRightPoint3D(right);
                }
                else
                {
                    action1 = () => _detialWindow.SetHandLeftPoint3D(right);
                    action2 = () => _detialWindow.SetHandRightPoint3D(left);
                }
                Dispatcher.BeginInvoke(DispatcherPriority.Send, action1);
                Dispatcher.BeginInvoke(DispatcherPriority.Send, action2);
            }
        }

        private void ListenerLeapSwipeReady(object sender,SwipeType type)
        {            
            Action action = null;
            switch (type)
            {
                case SwipeType.SwipeLeft:
                    if (_musicWindow != null)
                    {
                        action = () => _musicWindow.Backword();
                    }
                    else if (_videoWindow != null)
                    {
                        action = () => _videoWindow.Backword();
                    }
                    else if (_detialWindow != null)
                    {
                        action = () => _detialWindow.Forword();
                    }
                    else
                    {
                        action = () => flow.GoToNext();
                    }

                    break;
                case SwipeType.SwipeRight:
                    if (_musicWindow != null)
                    {
                        action = () => _musicWindow.Forword();
                    }
                    else if (_videoWindow != null)
                    {
                        action = () => _videoWindow.Forword();
                    }
                    else if (_detialWindow != null)
                    {
                        action = () => _detialWindow.Backword();
                    }
                    else
                    {
                        action = () => flow.GoToPrevious();
                    }

                    break;
                    //case  SwipeType.SwipeOut:
                    //if (detialWindow == null && musicWindow == null)
                    //{
                    //    action = OpenSubWindow;
                    //}
                    //break;
                    //case SwipeType.SwpieIn:
                    //if (detialWindow != null || musicWindow != null)
                    //{
                    //    Action action2 = null;
                    //    if (detialWindow != null)
                    //    {
                    //        action2 =()=> detialWindow.CloseThis();
                    //        //detialWindow.CloseThis();
                    //        //detialWindow = null;
                    //    }
                    //    else
                    //    {
                    //        action2=()=>musicWindow.CloseThis();
                    //        //musicWindow.CloseThis();
                    //        //musicWindow = null;
                    //    }
                    //    Dispatcher.BeginInvoke(DispatcherPriority.Send, action2).Completed+=(a, b) =>
                    //        {
                    //            detialWindow = null;
                    //            musicWindow = null;
                    //        };
                    //}
                    //break;
            }

            if (action != null)
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }

        #endregion

        #region 打开下一级展示窗口(Kinect与LeapMotion部分共同调用)

        /// <summary>
        /// 打开图片展示/音乐播放/视频播放窗口
        /// </summary>
        private void OpenSubWindow()
        {
            FileInfo info = CheckIfMediaPath(_currentIndex);
            if (info != null)
            {
                if (info.Extension == ".mp3")
                {
                    BassEngine.Instance.OpenFile(info.FullName);
                    _musicWindow = MusicWindow.GetInstance(_images[_currentIndex]);
                    _musicWindow.Closing += (a, b) => _musicWindow = null;
                    _musicWindow.Show();
                }
                else
                {
                    _videoWindow = VideoWindow.GetInstance(_images[_currentIndex], info);
                    _videoWindow.Closing += (a, b) => _videoWindow = null;
                    _videoWindow.Show();
                }
            }
            else
            {
                _detialWindow = DetialWindow.GetInstance(_images[_currentIndex]);
                _detialWindow.Closing += (a, b) => _detialWindow = null;
                _detialWindow.Show();
            }
        }

        #endregion

        #region 界面事件

        /// <summary>
        /// 中间的Cover被点击
        /// </summary>
        /// <param name="sender"></param>
        private void flow_CenterCoverClicked(object sender)
        {
            OpenSubWindow();
        }

        /// <summary>
        /// 中间的Cover发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flow_IndexChanged(object sender, int e)
        {
            _currentIndex = e;
            ChangeFileInfo();
        }

        /// <summary>
        /// 滑动条进度发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            flow.Index = Convert.ToInt32(slider.Value);
        }

        /// <summary>
        /// 键盘操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    flow.GoToNext();
                    break;
                case Key.Left:
                    flow.GoToPrevious();
                    break;
                case Key.PageUp:
                    flow.GoToNextPage();
                    break;
                case Key.PageDown:
                    flow.GoToPreviousPage();
                    break;
                case Key.Escape:
                    CloseThis();
                    break;
            }
            if (flow.Index != Convert.ToInt32(slider.Value))
                slider.Value = flow.Index;
        }

        #endregion

        #region 文件相关处理

        /// <summary>
        /// 加载文件
        /// </summary>
        public void LoadFiles()
        {
            _images = new List<FileInfo>();

            var commonPicturesDir =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
            _images.AddRange(commonPicturesDir.GetFiles("*.jpg"));
            _images.AddRange(commonPicturesDir.GetFiles("*.png"));

            var myPicturesDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            _images.AddRange(myPicturesDir.GetFiles("*.jpg"));
            _images.AddRange(myPicturesDir.GetFiles("*.png"));


            _myMusicDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            _myCoverDir = new DirectoryInfo(_myMusicDir.FullName + @"\Covers\");
            if (!_myCoverDir.Exists)
            {
                _myCoverDir.Create();
            }
            _myLyricDir = new DirectoryInfo(_myMusicDir.FullName + @"\Lyrics\");
            if (!_myLyricDir.Exists)
            {
                _myLyricDir.Create();
            }
            FileInfo[] musics = _myMusicDir.GetFiles("*.mp3");

            foreach (var music in musics)
            {
                string cover = _myCoverDir + @"\" + Path.GetFileNameWithoutExtension(music.FullName) + ".jpg";
                if (!File.Exists(cover))
                {
                    TagHandler tagHandler = new TagHandler(new Mp3File(music).TagModel);
                    if (tagHandler.Picture != null)
                    {
                        tagHandler.Picture.Save(cover);
                    }
                    else
                    {
                        File.Copy(Environment.CurrentDirectory + @"\cover.jpg", cover, true);
                    }
                }
                _images.Add(new FileInfo(cover));
            }


            _myVideoDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            DirectoryInfo thumbnailDir = new DirectoryInfo(_myVideoDir.FullName + @"\thumbnais");
            if (!thumbnailDir.Exists)
            {
                thumbnailDir = _myVideoDir.CreateSubdirectory("thumbnails");
            }
            FileInfo[] videos = _myVideoDir.GetFiles("*.mp4");
            foreach (var video in videos)
            {
                string thumbnail = thumbnailDir + @"\" + Path.GetFileNameWithoutExtension(video.FullName) +
                                   ".jpg";

                if (!File.Exists(thumbnail))
                {
                    if (VideoUnity.CatchImg(video.FullName, thumbnail))
                    {
                        _images.Add(new FileInfo(thumbnail));
                    }
                    //images.Add(new FileInfo(thumbnail));
                }
                else
                {
                    _images.Add(new FileInfo(thumbnail));
                }
            }

            //_images.Sort(new FileInfoComparer());
            foreach (FileInfo f in _images)
                flow.Add(Environment.MachineName, f.FullName);
        }

        /// <summary>
        /// 文件名比较
        /// </summary>
        private class FileInfoComparer : IComparer<FileInfo>
        {
            public int Compare(FileInfo x, FileInfo y)
            {
                return String.CompareOrdinal(x.FullName, y.FullName);
            }
        }

        /// <summary>
        /// 检查Cover是否是音乐或视频文件
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private FileInfo CheckIfMediaPath(int currentIndex)
        {
            string musicPath = _myMusicDir.FullName + @"\" +
                               Path.GetFileNameWithoutExtension(_images[currentIndex].FullName) + ".mp3";
            if (File.Exists(musicPath))
                return new FileInfo(musicPath);
            string videoPath = _myVideoDir.FullName + @"\" +
                               Path.GetFileNameWithoutExtension(_images[currentIndex].FullName) + ".mp4";
            if (File.Exists(videoPath))
            {
                return new FileInfo(videoPath);
            }
            return null;
        }


        /// <summary>
        /// 显示文件信息
        /// </summary>
        private void ChangeFileInfo()
        {
            FileInfo info = CheckIfMediaPath(_currentIndex);
            string strName = info != null ? info.Name : _images[_currentIndex].Name;
            fileInfo.Text = Path.GetFileNameWithoutExtension(strName);
            if (flow.Index != Convert.ToInt32(slider.Value))
                slider.Value = flow.Index;
            if (info == null)
                info = new FileInfo(_images[_currentIndex].Name);
            string picName;
            switch (info.Extension)
            {
                case ".png":
                case ".jpg":
                    picName = "photo.png";
                    break;
                case ".mp3":
                case ".wav":
                    picName = "music.png";
                    break;
                case ".mp4":
                case ".flv":
                    picName = "video.png";
                    break;
                default:
                    picName = null;
                    break;
            }
            if (picName != null)
                ImgMedia.Source = new BitmapImage(PackUriHelper.CreatePackUri("/resouces/"+picName));
        }

        #endregion

        #region 退出和释放资源

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void CloseThis()
        {
            if (_detialWindow != null)
                _detialWindow.CloseThis();
            if (_musicWindow != null)
                _musicWindow.CloseThis();
            if (_videoWindow != null)
                _videoWindow.CloseThis();

            _stdEnd = (Storyboard) Resources["sb_end"];
            _stdEnd.Completed += (a, b) => Close();
            _stdEnd.Begin();

            _leapHelper.Close();

        }

        #endregion
    }
}