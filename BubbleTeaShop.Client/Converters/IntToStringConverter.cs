using System.Globalization;

namespace BubbleTeaShop.Client.Converters;

public class IntToStringConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue.ToString();
        }
        return string.Empty; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string strValue)
        {
            if (string.IsNullOrWhiteSpace(strValue))
            {
                return 0; 
            }
            if (int.TryParse(strValue, out int intResult))
            {
                return intResult;
            }
        }
        return 0;
    }
}