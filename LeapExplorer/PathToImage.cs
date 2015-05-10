using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace LeapExplorer
{
    public class PathToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapSource bs = null;

            try
            {
                string fullname = value.ToString();

                if (fullname.StartsWith("http://"))
                {
                    bs = new BitmapImage(new Uri(fullname));
                }
                else
                {
                    BitmapFrame bit = BitmapFrame.Create(new Uri(fullname, UriKind.Relative),
                                                         BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);

                    bs = bit.Thumbnail == null ? bit : bit.Thumbnail;
                }
            }
            catch (Exception )
            {
                throw ;
            }

            return bs;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}