using HVAC.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace HVACDesigner.ViewModels
{
    public class RelayCommand : ICommand
    {
        #region Filds
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion
        #region Constructor
        public RelayCommand(Action<object>execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion
        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
    class UpdateOnEnterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            TextBox tBox = parameter as TextBox;
            if (tBox != null)
            {
                DependencyProperty prop = TextBox.TextProperty;
                BindingExpression binding = BindingOperations.GetBindingExpression(tBox, prop);
                if (binding != null)
                    binding.UpdateSource();
            }

        }
    }
    class UpdateOnDataGridCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DataGrid grid = parameter as DataGrid;
            if (grid != null)
            {

                DependencyProperty prop = DataGrid.ItemsSourceProperty;
                BindingExpression binding = BindingOperations.GetBindingExpression(grid, prop);
                if (binding != null)
                    binding.UpdateSource();
            }
        }
    }

    class SelectRowCommend : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DataGrid grid = parameter as DataGrid;
            if (grid != null)
            {
                RoundDuct rd = grid.Items.GetItemAt(0) as RoundDuct;
                grid.RowBackground = new SolidColorBrush(Colors.Red);
            }
        }
    }
    
}
