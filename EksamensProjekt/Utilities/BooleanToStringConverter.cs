using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EksamensProjekt.Utilities
{
    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? "Ja" : "Nej";
            }
            return "Nej"; // Hvis værdien er null
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (stringValue == "Ja")
                    return true;
                if (stringValue == "Nej")
                    return false;
            }
            return false; // Default værdi
        }
    }

}
