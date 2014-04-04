using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Amur8.Converters
{
    public class AddZeroToNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int timeValue = (int)value;
            if (timeValue > 9)
            {
                return value;
            }
            else
            {
                int decimalLength = timeValue.ToString("D").Length + 1;
                return timeValue.ToString("D" + decimalLength.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
