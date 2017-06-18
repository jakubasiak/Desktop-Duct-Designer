using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HVACDesigner.ViewModels
{
    public abstract class DialogBox : FrameworkElement, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            #endregion
        }
        protected Action<Object> execute = null;
        public string Caption { get; set; }
        protected ICommand show;

        public virtual ICommand Show
        {
            get
            {
                //if (show == null)
                   // show = new RelayCommand(execute);
                return show;
            }
        }
    }
}
