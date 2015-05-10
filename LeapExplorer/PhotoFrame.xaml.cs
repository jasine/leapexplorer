using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Expression.Interactivity.Core;

namespace LeapExplorer
{
    /// <summary>
    /// PhotoFrame.xaml 的互動邏輯
    /// </summary>
    public partial class PhotoFrame : UserControl
    {
        public PhotoFrame()
        {
            try
            {
                this.InitializeComponent();
                //ImageUrl = "Image/1.png";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public string ImageUrl
        {
            get { return (string) GetValue(ImageUrlProperty); }
            set
            {
                if (imgAnimation.Equals("ImageChanged1"))
                {
                    imgAnimation = "ImageChanged2";
                }
                else
                {
                    imgAnimation = "ImageChanged1";
                }
                //imgAnimation = "ImageChanged2";
                ExtendedVisualStateManager.GoToElementState(this.LayoutRoot, imgAnimation, true);
                SetValue(ImageUrlProperty, value);
            }
        }

        private string imgAnimation = "ImageChanged1";

        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof (string), typeof (PhotoFrame),
                                        new UIPropertyMetadata(string.Empty));

        private void img_Loaded(object sender, RoutedEventArgs e)
        {
            //ExtendedVisualStateManager.GoToElementState(this.LayoutRoot, "ImageChanged", true);
        }
    }
}