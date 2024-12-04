using System;
using System.Globalization;
using System.Windows.Data;

namespace EksamensProjekt.Utilities
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Invert boolean value
            if (value is bool)
            {
                return !(bool)value;
            }
            return value; // Return original value if it's not a boolean
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Handle two-way binding if needed, or simply return the original value
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }
    }
}
