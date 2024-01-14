using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RadioButtonCustom
{
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Преобразование из double в строку
            if (value is double)
            {
                return ((double)value).ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Преобразование из строки в double
            if (value is string)
            {
                double result;
                if (double.TryParse((string)value, out result))
                {
                    return result;
                }
            }
            return value;
        }
    }
}
