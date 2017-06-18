using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HVACDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DateTime now = DateTime.Now;
            DateTime end = new DateTime(2018, 1, 1);
            if (now < end)
            {
                InitializeComponent();
            }
            else
            {
                MessageBox.Show("Your license has expired. To extend the license, please write on kubakubasiak@gmail.com", "License expired", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
    }
}

