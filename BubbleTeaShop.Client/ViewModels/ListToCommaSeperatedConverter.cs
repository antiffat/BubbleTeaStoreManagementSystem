using System.Collections;
using System.Globalization;
using System.Collections; 
using System.Globalization; 
using System.Linq;     

namespace BubbleTeaShop.Client.ViewModels;

public class ListToCommaSeperatedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable list)
        {
            var strings = list.Cast<object>().Select(o => o.ToString());
            if (strings.Any())
            {
                return string.Join(", ", strings);
            }
        }
        return "None";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}