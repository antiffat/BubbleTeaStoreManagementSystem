using System.Globalization;
using Size = BubbleTeaShop.Backend.Enums.Size;

namespace BubbleTeaShop.Client.ViewModels;

public class SizetoIsCheckedConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Size selectedSize && parameter is Size currentRadioButtonSize)
        {
            return selectedSize == currentRadioButtonSize;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
        {
            return parameter;
        }
        return Binding.DoNothing;
    }
}