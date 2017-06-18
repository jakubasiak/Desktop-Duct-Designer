using HVACDesigner.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using NCalc;

namespace HVACDesigner.ViewModels
{
    class SelectionTypeToStringConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SelectionType type = (SelectionType)value;
            if (type == SelectionType.Velocity)
                return "[m/s]";
            else
                return "[Pa/m]";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    class MeterToMillimeterConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            return val * 1000.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sVal = (value as string).Replace('.', ',');
            try
            {
                double val = double.Parse(sVal);
                return val / 1000.0;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    class ComaToDotConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = (value as string).Replace(',', '.');
            try
            {
                return val;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    class TargetValueToSolidBrushConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float val = (float)value;
            return new SolidColorBrush(Color.FromScRgb(val, 0, 200, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    class ColapseDuctSystemPanelConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int) value;
            if (val > 0)
                return new GridLength(1,GridUnitType.Star);
            else
                return new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class NCalcConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double) value;

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string val = value.ToString().Replace(',','.');
                NCalc.Expression e = new NCalc.Expression(val);

                return e.Evaluate();
            }
            catch (Exception)
            {
                return 0;
            }

        }
    }
}
