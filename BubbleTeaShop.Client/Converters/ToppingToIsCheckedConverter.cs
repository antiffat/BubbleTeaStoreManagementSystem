using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Client.Converters;

public class ToppingToIsCheckedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not Topping topping) return false;
        
        if (value is IEnumerable enumVal)
        {
            foreach (var item in enumVal)
            {
                if (item is Topping t && t == topping) return true;
            }
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}