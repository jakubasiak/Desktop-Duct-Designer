using HVACDesigner.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HVACDesigner.ViewModels
{
    class ValueWindowViewModel
    {
        public double Value { get; set; }
        private ICommand _setValueCommand;
        public ICommand SetValueCommand
        {
            get
            {
                if (_setValueCommand == null)
                    _setValueCommand = new RelayCommand(
                        x =>
                        {
                            ValueButton vb = x as ValueButton;
                            Value = vb.Value;                            
                        }
                        );

                return _setValueCommand;
            }
        }
        private ICommand _windowCloseCommand;
        public ICommand WindowCloseCommand
        {
            get
            {
                if (_windowCloseCommand == null)
                    _windowCloseCommand = new RelayCommand(
                        x =>
                        {
                            ((Window)x).Close();
                        }
                        );

                return _windowCloseCommand;
            }
        }


    }
}
