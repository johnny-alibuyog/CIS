using System;
using System.Windows.Data;

namespace CIS.UI.Utilities.Converters;

public class EnumToBooleanConverter : IValueConverter
{
    //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //{
    //    return ((Enum)value).HasFlag((Enum)parameter);
    //}

    //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //{
    //    return value.Equals(true) ? parameter : Binding.DoNothing;
    //}

    //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //{
    //    var enumValue = parameter != null
    //        ? parameter.ToString()
    //        : null;

    //    if (enumValue == null || Enum.IsDefined(value.GetType(), value) == false)
    //        return DependencyProperty.UnsetValue;

    //    return Enum.Parse(value.GetType(), enumValue, true).Equals(value);
    //}


    //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //{
    //    if (value.Equals(false)) 
    //        return DependencyProperty.UnsetValue;

    //    var parameterString = parameter != null
    //        ? parameter.ToString()
    //        : null;

    //    return parameterString == null
    //        ? DependencyProperty.UnsetValue
    //        : Enum.Parse(targetType, parameterString, true);
    //}


    //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //{
    //    return value.Equals(parameter);
    //}

    //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //{
    //    return value.Equals(true) ? parameter : Binding.DoNothing;
    //}

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value != null && value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value != null && value.Equals(true) 
            ? parameter 
            : Binding.DoNothing;
    }
}
