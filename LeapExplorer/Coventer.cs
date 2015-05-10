﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace LeapExplorer
{
    public class ToTransparentColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Color.FromArgb(0, ((Color) value).R, ((Color) value).G, ((Color) value).B);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}