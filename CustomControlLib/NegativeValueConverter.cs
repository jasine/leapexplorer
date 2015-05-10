using System;
using System.Windows.Data;

namespace Sheva.Windows.Controls
{
    internal class NegativeValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            return (0d - (Double)value);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
