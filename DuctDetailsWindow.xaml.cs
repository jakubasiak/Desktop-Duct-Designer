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
using System.Windows.Shapes;

namespace HVACDesigner
{
    /// <summary>
    /// Logika interakcji dla klasy DuctDetailsWindow.xaml
    /// </summary>
    public partial class DuctDetailsWindow : Window
    {
        public DuctDetailsWindow()
        {
            InitializeComponent();
        }

        private void TextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
