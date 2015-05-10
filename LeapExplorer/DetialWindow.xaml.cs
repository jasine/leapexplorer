using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Sheva.Windows.Controls;

namespace LeapExplorer
{
    /// <summary>
    /// DetialWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DetialWindow : Window
    {
        private ImageSource img;
        private double size, w, h;
        private Storyboard stdStart, stdEnd, stdMiddle, stdEnd2;

        private double angleYLast = 0;
        private double lastTimesX = 1;
        private Point gdCenter;

        private Point leftHand, rightHand, leftHandLast, rightHandLast;
        private Point3D leftHand3D, rightHand3D;
        private bool allowTrans = true;
        public DispatcherTimer timerTrans;
        private bool allowTic = true;
        public Point heapCenter;

        private DateTime lastTime;

        public static DetialWindow Instance { get; private set; }

        public static DetialWindow GetInstance(FileInfo imgSrc)
        {
            if (Instance != null)
                Instance.Close();
            Instance = new DetialWindow(imgSrc);
            return Instance;
        }

        private DetialWindow(FileInfo imgSrc)
        {
            InitializeComponent();
            gdCenter = new Point(0, 0);

            Images.Focus();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.Left = 0;
            this.Top = 0;
            img = new BitmapImage(new Uri(imgSrc.FullName));
            this.Loaded += MainWindow_Loaded;
           
            List<ImageSource> photos = new List<ImageSource>();
            photos.Add(img);
            DirectoryInfo phothDir =
                new DirectoryInfo(imgSrc.Directory.FullName + @"\" + Path.GetFileNameWithoutExtension(imgSrc.Name));
            if (phothDir.Exists)
            {
                ImageList.Visibility = Visibility.Visible;
                List<FileInfo> photoFiles = new List<FileInfo>();
                photoFiles.AddRange(phothDir.GetFiles("*.jpg", SearchOption.AllDirectories));
                photoFiles.AddRange(phothDir.GetFiles("*.png", SearchOption.AllDirectories));
                foreach (var file in photoFiles)
                {
                    photos.Add(new BitmapImage(new Uri(file.FullName)));
                }
            }
            Images.ItemsSource = photos;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // this.Topmost = true;
            //根据分辨率不同，调整DetialWindow出现的位置
            if (this.Width > 1300)
            {
                size = SystemParameters.PrimaryScreenWidth * 0.415;
            }
            else if (this.Width < 1300 && this.Width > 1000)
            {
                size = SystemParameters.PrimaryScreenWidth * 0.415;
            }

            //gd.Background =  new ImageBrush(img);
            if (img.Width >= img.Height)
            {
                w = size;
                h = size / img.Width * img.Height;
            }
            else
            {
                h = size;
                w = size / img.Height * img.Width;
            }
            


            stdStart = (Storyboard)this.Resources["start"];
            stdMiddle = (Storyboard)this.Resources["middle"];
            stdEnd = (Storyboard)this.Resources["end"];
            stdEnd2 = (Storyboard)this.Resources["end2"];

            stdStart.Completed += (a, b) =>
            {
                //stdMiddle.Begin(); 
                var transY = new DoubleAnimation(0, -85, new Duration(TimeSpan.FromMilliseconds(200)));
                list_trans.BeginAnimation(TranslateTransform.YProperty, transY);
            };
            stdStart.Begin();

            stdEnd.Completed += (c, d) => this.Close();

            Size newSize = AdjusePhotoFitScreen(img);
            var opacityGrid = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0)));
            var widthx = new DoubleAnimation(w, newSize.Width, new Duration(TimeSpan.FromMilliseconds(500)));
            var heightx = new DoubleAnimation(h, newSize.Height, new Duration(TimeSpan.FromMilliseconds(500)));
            if (Images.Items.Count > 1)
            {
                var dat2 = new DoubleAnimation(0, -40, new Duration(TimeSpan.FromMilliseconds(500)));
                tlt.BeginAnimation(TranslateTransform.YProperty, dat2);
                gdCenter.Y = -40;
            }
                    

            gd.BeginAnimation(Grid.OpacityProperty, opacityGrid);
            gd.BeginAnimation(Grid.WidthProperty, widthx);
            gd.BeginAnimation(Grid.HeightProperty, heightx);

            timerTrans = new DispatcherTimer();
            timerTrans.Interval = TimeSpan.FromMilliseconds(800);
            timerTrans.Tick += (a, b) =>
                {
                    timerTrans.Stop();
                    khbTip.Hovering();
                };
            khbTip.Click += (c, d) => { allowTrans = allowTrans == false; };
        }

        private void main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //ClosingAnmit();
        }


        public void SetHandLeftPoint(Point point)
        {
            SetElementPosition(LeftHand, point);
            leftHand = point;
            Transform();
        }

        public void SetHandRightPoint(Point point)
        {
            SetElementPosition(RightHand, point);
            rightHand = point;
            Transform();
        }

        private void Transform()
        {
            double centerX = (rightHand.X - leftHand.X)/2 + leftHand.X;
            double centerY = (rightHand.Y - leftHand.Y)/2 + leftHand.Y;

            double timesX = (rightHand.X - leftHand.X)/gd.ActualWidth;
            double angleY = Math.Atan((rightHand.Y - leftHand.Y)/(rightHand.X - leftHand.X))*180/Math.PI;

            if (Math.Abs(rightHand.X - rightHandLast.X) < 25 && Math.Abs(rightHand.Y - rightHandLast.Y) < 25 &&
                Math.Abs(leftHand.X - leftHandLast.X) < 25 && Math.Abs(leftHand.Y - leftHandLast.Y) < 25)

            {
                if (allowTic)
                {
                    allowTic = false;
                    timerTrans.Start();
                }
            }
            else
            {
                khbTip.Release();
                allowTic = true;
            }
            leftHandLast = leftHand;
            rightHandLast = rightHand;
            if (allowTrans)
            {
                if (!double.IsInfinity(timesX) && !double.IsNaN(timesX))
                {
                    var das1 = new DoubleAnimation(lastTimesX, timesX, new Duration(TimeSpan.FromMilliseconds(0)));
                    var das2 = new DoubleAnimation(lastTimesX, timesX, new Duration(TimeSpan.FromMilliseconds(0)));
                    sct.CenterX = gd.ActualWidth/2;
                    sct.CenterY = gd.ActualHeight/2;
                    sct.BeginAnimation(ScaleTransform.ScaleXProperty, das1);
                    sct.BeginAnimation(ScaleTransform.ScaleYProperty, das2);
                    lastTimesX = timesX;
                }

                //double tanOffset = gd.RenderSize.Width / 2 + centerightHand.X / gd.RenderSize.Height / 2 + centerightHand.Y;
                //angleY += Math.Atan(tanOffset) * 180 / Math.PI;         
                var dar1 = new DoubleAnimation(angleYLast, angleY, new Duration(TimeSpan.FromMilliseconds(0)));
                rt.CenterX = gd.ActualWidth/2;
                rt.CenterY = gd.ActualHeight/2;
                rt.BeginAnimation(RotateTransform.AngleProperty, dar1);
                angleYLast = angleY;


                if (!double.IsNaN(rightHand.X) && !double.IsNaN(rightHand.Y))
                {
                    var dat1 = new DoubleAnimation(gdCenter.X, centerX, new Duration(TimeSpan.FromMilliseconds(0)));
                    var dat2 = new DoubleAnimation(gdCenter.Y, centerY, new Duration(TimeSpan.FromMilliseconds(0)));
                    tlt.BeginAnimation(TranslateTransform.XProperty, dat1);
                    tlt.BeginAnimation(TranslateTransform.YProperty, dat2);
                    gdCenter.X = centerX;
                    gdCenter.Y = centerY;
                }
            }
        }

        public void SetHandLeftPoint3D(Point3D point)
        {
            SetElementPosition(LeftHand, new Point(point.X, point.Y));
            leftHand3D = point;
            //LeapTransform();
        }

        public void SetHandRightPoint3D(Point3D point)
        {
            SetElementPosition(RightHand, new Point(point.X, point.Y));
            rightHand3D = point;
            LeapTransform();
        }

        private void LeapTransform()
        {
            double centerX = (rightHand3D.X - leftHand3D.X)/2 + leftHand3D.X;
            double centerY = (rightHand3D.Y - leftHand3D.Y)/2 + leftHand3D.Y;

            //double timesX = ((rightHand3D.Z+300)*3.2) / gd.ActualWidth;
            //timesX *= timesX;
            double timesX = (rightHand3D.X - leftHand3D.X)/gd.ActualWidth;
            timesX *= 1.5;
            double angleY = Math.Atan((rightHand3D.Y - leftHand3D.Y)/(rightHand3D.X - leftHand3D.X))*180/Math.PI;

            //if (Math.Abs(rightHand.X - rightHandLast.X) < 25 && Math.Abs(rightHand.Y - rightHandLast.Y) < 25 &&
            //    Math.Abs(leftHand.X - leftHandLast.X) < 25 && Math.Abs(leftHand.Y - leftHandLast.Y) < 25)
            //{
            //    if (allowTic)
            //    {
            //        allowTic = false;
            //        timerTrans.Start();
            //    }

            //}
            //else
            //{
            //    khbTip.Release();
            //    allowTic = true;
            //}
            leftHandLast = leftHand;
            rightHandLast = rightHand;
            if (true)
            {
                if (!double.IsInfinity(timesX) && !double.IsNaN(timesX))
                {
                    var das1 = new DoubleAnimation(lastTimesX, timesX, new Duration(TimeSpan.FromMilliseconds(100)));
                    var das2 = new DoubleAnimation(lastTimesX, timesX, new Duration(TimeSpan.FromMilliseconds(100)));
                    sct.CenterX = gd.ActualWidth/2;
                    sct.CenterY = gd.ActualHeight/2;
                    sct.BeginAnimation(ScaleTransform.ScaleXProperty, das1);
                    sct.BeginAnimation(ScaleTransform.ScaleYProperty, das2);
                    lastTimesX = timesX;
                }

                //double tanOffset = gd.RenderSize.Width / 2 + centerightHand.X / gd.RenderSize.Height / 2 + centerightHand.Y;
                //angleY += Math.Atan(tanOffset) * 180 / Math.PI;         
                if (!double.IsNaN(angleY))
                {
                    var dar1 = new DoubleAnimation(angleYLast, angleY, new Duration(TimeSpan.FromMilliseconds(100)));
                    rt.CenterX = gd.ActualWidth/2;
                    rt.CenterY = gd.ActualHeight/2;
                    rt.BeginAnimation(RotateTransform.AngleProperty, dar1);
                    angleYLast = angleY;
                }


                if (!double.IsNaN(rightHand.X) && !double.IsNaN(rightHand.Y))
                {
                    var dat1 = new DoubleAnimation(gdCenter.X, centerX, new Duration(TimeSpan.FromMilliseconds(100)));
                    var dat2 = new DoubleAnimation(gdCenter.Y, centerY, new Duration(TimeSpan.FromMilliseconds(100)));
                    tlt.BeginAnimation(TranslateTransform.XProperty, dat1);
                    tlt.BeginAnimation(TranslateTransform.YProperty, dat2);
                    gdCenter.X = centerX;
                    gdCenter.Y = centerY;
                }
            }
        }

        public void CloseThis()
        {
            Images.SelectedIndex = 0;
            stdEnd2.Completed += (a, b) => CloseAnmit();
            stdEnd2.Begin();
            var transY = new DoubleAnimation(-80, 0, new Duration(TimeSpan.FromMilliseconds(300)));
            list_trans.BeginAnimation(TranslateTransform.YProperty, transY);
        }

        private void CloseAnmit()
        {
            stdEnd.Begin();
            var widthx = new DoubleAnimation(gd.ActualWidth, w, new Duration(TimeSpan.FromMilliseconds(300)));
            var heightx = new DoubleAnimation(gd.ActualHeight, h, new Duration(TimeSpan.FromMilliseconds(300)));
            gd.BeginAnimation(Grid.WidthProperty, widthx);
            gd.BeginAnimation(Grid.HeightProperty, heightx);

            if (lastTimesX != 0 && !double.IsNaN(lastTimesX))
            {
                var das1 = new DoubleAnimation(lastTimesX, 1, new Duration(TimeSpan.FromMilliseconds(300)));
                var das2 = new DoubleAnimation(lastTimesX, 1, new Duration(TimeSpan.FromMilliseconds(300)));
                sct.CenterX = gd.ActualWidth/2;
                sct.CenterY = gd.ActualHeight/2;
                sct.BeginAnimation(ScaleTransform.ScaleXProperty, das1);
                sct.BeginAnimation(ScaleTransform.ScaleYProperty, das2);
            }

            if (!double.IsNaN(angleYLast))
            {
                var dar1 = new DoubleAnimation(angleYLast, 0, new Duration(TimeSpan.FromMilliseconds(300)));
                rt.CenterX = gd.ActualWidth/2;
                rt.CenterY = gd.ActualHeight/2;
                rt.BeginAnimation(RotateTransform.AngleProperty, dar1);
            }

            var dat1 = new DoubleAnimation(gdCenter.X, 0, new Duration(TimeSpan.FromMilliseconds(300)));
            var dat2 = new DoubleAnimation(gdCenter.Y, 0, new Duration(TimeSpan.FromMilliseconds(300)));
            tlt.BeginAnimation(TranslateTransform.XProperty, dat1);
            tlt.BeginAnimation(TranslateTransform.YProperty, dat2);
        }


        private void main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseThis();
        }


        private static void SetElementPosition(FrameworkElement element, Point point)
        {
            //从对象的（left,top）修正为该对象的质心位置
            Canvas.SetLeft(element, point.X - element.Width/2);
            Canvas.SetTop(element, point.Y - element.Height/2);
        }


        public void Forword()
        {
            if (Images.Items.Count <= 1)
                return;
            if (DateTime.Now - lastTime < TimeSpan.FromSeconds(1))
                return;
            if (Images.SelectedIndex == Images.Items.Count - 1)
                Images.SelectedIndex = 0;
            else
            {
                Images.SelectedIndex++;
            }
            lastTime = DateTime.Now;
        }

        public void Backword()
        {
            if (Images.Items.Count <= 1)
                return;
            if (DateTime.Now - lastTime < TimeSpan.FromSeconds(1))
                return;
            if (Images.SelectedIndex == 0)
                Images.SelectedIndex = Images.Items.Count - 1;
            else
            {
                Images.SelectedIndex--;
            }
            lastTime = DateTime.Now;
        }


        private void Images_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Random random = new Random();
            int i = random.Next(0, effect.Items.Count);
            if (i == 1)
                i = random.Next(0, effect.Items.Count);
            effect.SelectedIndex = i;

            ListBox lb = (ListBox) sender;
            ImageSource selectedImg = (ImageSource) lb.SelectedItem;
            Size newSize = AdjusePhotoFitScreen(selectedImg);

            var widthx = new DoubleAnimation(gd.ActualWidth, newSize.Width, new Duration(TimeSpan.FromMilliseconds(300)));
            var heightx = new DoubleAnimation(gd.ActualHeight, newSize.Height,
                                              new Duration(TimeSpan.FromMilliseconds(300)));
            gd.BeginAnimation(Grid.WidthProperty, widthx);
            gd.BeginAnimation(Grid.HeightProperty, heightx);
        }

        private Size AdjusePhotoFitScreen(ImageSource img)
        {
            double width = img.Width,
                   height = img.Height,
                   adjust=40;
            if (Images.Items.Count > 1)
            {
                adjust = 110;
            }
            if (img.Width > SystemParameters.PrimaryScreenWidth - 40 ||
                img.Height > SystemParameters.PrimaryScreenHeight - h)
            {
                double times1 = img.Width/(SystemParameters.PrimaryScreenWidth - 40.0);
                double times2 = img.Height/(SystemParameters.PrimaryScreenHeight - adjust);
                double times = times1 > times2 ? times1 : times2;
                width = width/times;
                height = height/times;
            }
            return new Size(width, height);
        }
    }
}