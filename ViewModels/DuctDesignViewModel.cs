using HVAC.FluidMechanics;
using HVAC.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using HVACDesigner.Helpers;
using Microsoft.Win32;

namespace HVACDesigner.ViewModels
{
    class DuctDesignViewModel : INotifyPropertyChanged
    {
        #region Właściwości

        public string XMLpath { get; set; }
        public string CSVPath { get; set; }
        public string PDFPath { get; set; }

        private AirFlow AirFlow;

        private RoundDuctDesigner _roundDuctDesigner;
        public RoundDuctDesigner RoundDuctDesigner
        {
            get
            {
                return _roundDuctDesigner;
            }
            set
            {
                if (_roundDuctDesigner == null)
                    _roundDuctDesigner = value;
            }
        }

        private RectangularDuctDesigner _rectangularDuctDesigner;
        public RectangularDuctDesigner RectangularDuctDesigner
        {
            get
            {
                return _rectangularDuctDesigner;
            }
            set
            {
                if (_rectangularDuctDesigner == null)
                    _rectangularDuctDesigner = value;
            }
        }

        public double Flow
        {
            get
            {
                return AirFlow.Flow;
            }
            set
            {
                if (value >= 0)
                {
                    AirFlow.Flow = value;
                    OnPropertyChanged("Flow");
                }
            }
        }
        public double Temperature
        {
            get
            {
                return AirFlow.Temperature - 273.15;
            }
            set
            {
                if (value > -273.15)
                {
                    AirFlow.Temperature = value + 273.15;
                    OnPropertyChanged("Temperature");
                    OnPropertyChanged("Density");
                }
            }
        }
        public double Pressure
        {
            get
            {
                return AirFlow.Pressure;
            }
            set
            {
                if (value >= 0)
                {
                    AirFlow.Pressure = value;
                    OnPropertyChanged("Pressure");
                    OnPropertyChanged("Density");
                }
            }
        }
        public double Density
        {
            get
            {
                return AirFlow.Density;
            }
        }
        public IEnumerable<SelectionType> SelectionTypeSource
        {
            get
            {
                return Enum.GetValues(typeof(SelectionType)).Cast<SelectionType>();
            }
        }

        SelectionType _selectionType;
        public SelectionType SelectionType
        {
            get
            {
                return _selectionType;
            }
            set
            {
                _selectionType = value;
                OnPropertyChanged("SelectionType");
            }
        }

        double _targetValue;
        public double TargetValue
        {
            get
            {
                return _targetValue;
            }
            set
            {
                _targetValue = value;
                OnPropertyChanged("TargetValue");
            }
        }

        public IEnumerable<DarcyFrictionFactorApproximation> ApproximationSource
        {
            get
            {
                return Enum.GetValues(typeof(DarcyFrictionFactorApproximation)).Cast<DarcyFrictionFactorApproximation>();
            }
        }

        DarcyFrictionFactorApproximation _approximation;
        public DarcyFrictionFactorApproximation Approximation
        {
            get
            {
                return _approximation;
            }
            set
            {
                _approximation = value;
                OnPropertyChanged("Approximation");
            }
        }

        double _rectangularDuctHeight;
        public double RectangularDuctHeight
        {
            get
            {
                return _rectangularDuctHeight;
            }
            set
            {
                _rectangularDuctHeight = value;
                OnPropertyChanged("RectangularDuctHeight");
            }
        }

        double _relativeRoughness;
        public double RelativeRoughness
        {
            get
            {
                return _relativeRoughness;
            }
            set
            {
                _relativeRoughness = value;
                OnPropertyChanged("RelativeRoughness");
            }
        }

        double _ductLenght;
        public double DuctLenght
        {
            get
            {
                return _ductLenght;
            }
            set
            {
                _ductLenght = value;
                OnPropertyChanged("DuctLenght");
            }
        }

        ObservableCollection<LocalLoss> _localLosses;
        public ObservableCollection<LocalLoss> LocalLosses
        {
            get
            {
                return _localLosses;
            }
            set
            {
                _localLosses = value;
                OnPropertyChanged("LocalLosses");
            }
        }

        ObservableCollection<RoundDuctViewModel> _roundDuctList;
        public ObservableCollection<RoundDuctViewModel> RoundDuctList
        {
            get
            {
                return _roundDuctList;
            }
            set
            {
                _roundDuctList = value;
                OnPropertyChanged("RoundDuctList");
            }
        }

        ObservableCollection<RectangularDuctViewModel> _rectangularDuctList;
        public ObservableCollection<RectangularDuctViewModel> RectangularDuctList
        {
            get
            {
                return _rectangularDuctList;
            }
            set
            {
                _rectangularDuctList = value;
                OnPropertyChanged("RectangularDuctList");
            }
        }
        DuctSystem _ductSystem;
        public DuctSystem DuctSystem
        {
            get
            {
                return _ductSystem;
            }
            set
            {
                _ductSystem = value;
                OnPropertyChanged("DuctSystem");
            }
        }
        private int _ductSystemSelectedItem;
        public int DuctSystemSelectedItem
        {
            get
            {
                return _ductSystemSelectedItem;
            }
            set
            {
                _ductSystemSelectedItem = value;
                OnPropertyChanged("DuctSystemSelectedItem");
            }
        }
        #endregion
        #region Komendy
        private ICommand _updateOnEnterCommand;
        public ICommand UpdateOnEnterCommand
        {
            get
            {
                if (_updateOnEnterCommand == null)
                    _updateOnEnterCommand = new UpdateOnEnterCommand();
                return _updateOnEnterCommand;
            }
        }
        private ICommand _updateOnDataGridCommand;
        public ICommand UpdateOnDataGridCommand
        {
            get
            {
                if (_updateOnDataGridCommand == null)
                    _updateOnDataGridCommand = new UpdateOnDataGridCommand();
                return _updateOnDataGridCommand;
            }
        }

        private ICommand _selectRowCommend;
        public ICommand SelectRowCommend
        {
            get
            {
                if (_selectRowCommend == null)
                    _selectRowCommend = new SelectRowCommend();
                return _selectRowCommend;
            }
        }
        private ICommand _startTargetValueWindowCommand;
        public ICommand StartTargetValueWindowCommand
        {
            get
            {
                if (_startTargetValueWindowCommand == null)
                    _startTargetValueWindowCommand = new RelayCommand(
                        x =>
                        {
                            if (SelectionType == SelectionType.Velocity)
                            {
                                TargetValueWindow window = new TargetValueWindow();
                                window.ShowDialog();
                                ValueWindowViewModel vm = window.DataContext as ValueWindowViewModel;
                                if (vm.Value != 0)
                                    TargetValue = vm.Value;
                            }
                        }
                        );

                return _startTargetValueWindowCommand;
            }
        }
        private ICommand _startRelativeRoughnessWindowCommand;
        public ICommand StartRelativeRoughnessWindowCommand
        {
            get
            {
                if (_startRelativeRoughnessWindowCommand == null)
                    _startRelativeRoughnessWindowCommand = new RelayCommand(
                        x =>
                        {
                            RelativeRoughnessWindow window = new RelativeRoughnessWindow();
                            window.ShowDialog();
                            ValueWindowViewModel vm = window.DataContext as ValueWindowViewModel;
                            if (vm.Value != 0)
                                RelativeRoughness = vm.Value;
                        }
                        );

                return _startRelativeRoughnessWindowCommand;
            }
        }
        private ICommand _startDuctDetailsWindowCommand;
        public ICommand StartDuctDetailsWindowCommand
        {
            get
            {
                if (_startDuctDetailsWindowCommand == null)
                    _startDuctDetailsWindowCommand = new RelayCommand(
                        x =>
                        {
                            if (x != null)
                            {
                                BaseDuct duct = x as BaseDuct;
                                DuctInfoViewModel vm = new DuctInfoViewModel();
                                vm.CopyPoprtiesValuesFromDuct(duct);

                                DuctDetailsWindow window = new DuctDetailsWindow();
                                window.DataContext = vm;

                                window.ShowDialog();
                            }
                        }
                        );

                return _startDuctDetailsWindowCommand;
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

        private ICommand _addToDuctSystemCommand;
        public ICommand AddToDuctSystemCommand
        {
            get
            {
                if (_addToDuctSystemCommand == null)
                    _addToDuctSystemCommand = new RelayCommand(
                        x =>
                        {
                            if (x != null)
                            {
                                BaseDuct duct = (BaseDuct)x;
                                DuctSystem.Add(duct.Clone() as BaseDuct);

                            }
                        }
                        );

                return _addToDuctSystemCommand;
            }
        }
        private ICommand _deleteDuctFromDuctSystem;
        public ICommand DeleteDuctFromDuctSystem
        {
            get
            {
                if (_deleteDuctFromDuctSystem == null)
                    _deleteDuctFromDuctSystem = new RelayCommand(
                        x =>
                        {
                            BaseDuct duct = (BaseDuct)x;
                            DuctSystem.RemoveDuct(duct);
                        }
                        );

                return _deleteDuctFromDuctSystem;
            }
        }

        private ICommand _newProjectCommand;
        public ICommand NewProjectCommand
        {
            get
            {
                if (_newProjectCommand == null)
                    _newProjectCommand = new RelayCommand(
                        x =>
                        {
                            DuctSystem.RemoveAllDucts();
                            LocalLosses.Clear();
                            TargetValue = 3.5;
                            SelectionType = SelectionType.Velocity;
                            Approximation = DarcyFrictionFactorApproximation.GoudarSonnad;
                            RectangularDuctHeight = 0.2;
                            RelativeRoughness = 0.00001;
                            DuctLenght = 1;
                        }
                        );

                return _newProjectCommand;
            }
        }
        private ICommand _saveDuctSystem;
        public ICommand SaveDuctSystem
        {
            get
            {
                if (_saveDuctSystem == null)
                    _saveDuctSystem = new RelayCommand(
                        x =>
                        {
                            if (DuctSystem != null)
                            {
                                if (string.IsNullOrEmpty(XMLpath))
                                {
                                    SaveFileDialog sfd = new SaveFileDialog();
                                    sfd.FileName = "DuctSystem";
                                    sfd.DefaultExt = ".xml";
                                    sfd.Filter = "XML Files (*.xml)|*.xml";
                                    sfd.Title = "Save duct system";

                                    sfd.ShowDialog();
                                    XMLpath = sfd.FileName;
                                }

                                if(XMLpath!=null)
                                ImportExportHelper.SaveDuctSystemToXML(XMLpath, DuctSystem);
                            }
                        }
                        );

                return _saveDuctSystem;
            }
        }
        private ICommand _saveAsDuctSystem;
        public ICommand SaveAsDuctSystem
        {
            get
            {
                if (_saveAsDuctSystem == null)
                    _saveAsDuctSystem = new RelayCommand(
                        x =>
                        {
                            if (DuctSystem != null)
                            {
                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.FileName = "DuctSystem";
                                sfd.DefaultExt = ".xml";
                                sfd.Filter = "XML Files (*.xml)|*.xml";
                                sfd.Title = "Save duct system";

                                sfd.ShowDialog();
                                XMLpath = sfd.FileName;

                                if(XMLpath!=null)
                                ImportExportHelper.SaveDuctSystemToXML(XMLpath, DuctSystem);
                            }
                        }
                        );

                return _saveAsDuctSystem;
            }
        }

        private ICommand _loadDuctSystem;
        public ICommand LoadDuctSystem
        {
            get
            {
                if (_loadDuctSystem == null)
                    _loadDuctSystem = new RelayCommand(
                        x =>
                        {
                                OpenFileDialog sfd = new OpenFileDialog();
                                sfd.Multiselect = false;
                                sfd.DefaultExt = ".xml";
                                sfd.Filter = "XML Files (*.xml)|*.xml";
                                sfd.Title = "Open duct system";

                                sfd.ShowDialog();
                                XMLpath = sfd.FileName;


                                DuctSystem ds = ImportExportHelper.ReadDuctSystemFromXML(XMLpath);
                                if (ds != null)
                                    DuctSystem = ds;
                            
                        }
                        );

                return _loadDuctSystem;
            }
        }
        private ICommand _exportDuctSystemToCsv;
        public ICommand ExportDuctSystemToCsv
        {
            get
            {
                if (_exportDuctSystemToCsv == null)
                    _exportDuctSystemToCsv = new RelayCommand(
                        x =>
                        {
                            if (DuctSystem != null)
                            {
                                if (CSVPath == null)
                                {
                                    SaveFileDialog sfd = new SaveFileDialog();
                                    sfd.FileName = "DuctSystem";
                                    sfd.DefaultExt = ".csv";
                                    sfd.Filter = "CSV |*.csv"; ;
                                    sfd.Title = "Export duct system";

                                    sfd.ShowDialog();
                                    CSVPath = sfd.FileName;
                                }
                                if(CSVPath!=null)
                                ImportExportHelper.ExportDuctSystemToCSV(CSVPath, DuctSystem);
                            }
                        }
                        );

                return _exportDuctSystemToCsv;
            }
        }
        private ICommand _exportDuctSystemToCsvAs;
        public ICommand ExportDuctSystemToCsvAs
        {
            get
            {
                if (_exportDuctSystemToCsvAs == null)
                    _exportDuctSystemToCsvAs = new RelayCommand(
                        x =>
                        {
                            if (DuctSystem != null)
                            {
                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.FileName = "DuctSystem";
                                sfd.DefaultExt = ".csv";
                                sfd.Filter = "CSV |*.csv"; ;
                                sfd.Title = "Export duct system";

                                sfd.ShowDialog();
                                CSVPath = sfd.FileName;
                                if(CSVPath!=null)
                                ImportExportHelper.ExportDuctSystemToCSV(CSVPath, DuctSystem);
                            }
                        }
                        );

                return _exportDuctSystemToCsvAs;
            }
        }
        private ICommand _saveDuctSystemRaport;
        public ICommand SaveDuctSystemRaport
        {
            get
            {
                if (_saveDuctSystemRaport == null)
                    _saveDuctSystemRaport = new RelayCommand(
                        x =>
                        {
                            if (DuctSystem != null)
                            {
                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.FileName = "Duct System Raport";
                                sfd.DefaultExt = ".pdf";
                                sfd.Filter = "PDF |*.pdf"; ;
                                sfd.Title = "Create duct system raport";

                                sfd.ShowDialog();
                                PDFPath = sfd.FileName;

                                if(PDFPath!=null)
                                ImportExportHelper.CreateDuctSystemRaport(PDFPath, DuctSystem);
                            }
                        }
                        );

                return _saveDuctSystemRaport;
            }
        }
        private ICommand _moveUpCommand;
        public ICommand MoveUpCommand
        {
            get
            {
                if (_moveUpCommand == null)
                    _moveUpCommand = new RelayCommand(
                        x =>
                        {
                            if (x != null)
                            {
                                BaseDuct duct = (BaseDuct)x;

                                DuctSystem.MoveUp(duct);
                                DuctSystemSelectedItem = DuctSystem.DuctCollection.IndexOf(duct);
                            }
                        }
                        );

                return _moveUpCommand;
            }
        }
        private ICommand _moveDownCommand;
        public ICommand MoveDownCommand
        {
            get
            {
                if (_moveDownCommand == null)
                    _moveDownCommand = new RelayCommand(
                        x =>
                        {
                            if (x != null)
                            {
                                BaseDuct duct = (BaseDuct)x;

                                DuctSystem.MoveDown(duct);
                                DuctSystemSelectedItem = DuctSystem.DuctCollection.IndexOf(duct);
                            }
                        }
                        );

                return _moveDownCommand;
            }
        }
        
        private ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                if (_aboutCommand == null)
                    _aboutCommand = new RelayCommand(
                        x =>
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("Author: Jakub Kubasiak");
                            sb.AppendLine();
                            sb.Append("e-mail: kubakubasiak@gmail.com");
                            sb.AppendLine();
                            sb.Append("website: http://kubakubasiak.wixsite.com/home");
                            sb.AppendLine();
                            MessageBox.Show(sb.ToString(), "About", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        );

                return _aboutCommand;
            }
        }
        #endregion
        #region konstruktor
        public DuctDesignViewModel()
        {
            PropertyChanged += DuctDesignViewModel_PropertyChanged;

            AirFlow = new AirFlow(293.15, 101325, 0.0);
            TargetValue = 3.5;
            SelectionType = SelectionType.Velocity;
            Approximation = DarcyFrictionFactorApproximation.GoudarSonnad;
            RectangularDuctHeight = 0.2;
            RelativeRoughness = 0.00001;
            DuctLenght = 1;
            RoundDuctDesigner = new RoundDuctDesigner();
            RectangularDuctDesigner = new RectangularDuctDesigner();
            LocalLosses = new ObservableCollection<LocalLoss>();
            DuctSystem = new DuctSystem();
        }
        #endregion
        #region Reszta
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void DuctDesignViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string name = e.PropertyName;
            if (name == "Flow" ||
                   name == "Density" ||
                   name == "Approximation" ||
                   name == "RectangularDuctHeight" ||
                   name == "RelativeRoughness" ||
                   name == "DuctLenght" ||
                   name == "SelectionType" ||
                   name == "TargetValue" ||
                   name == "LocalLosses")
                DuctDesignExecute();
        }

        private void DuctDesignExecute()
        {
            bool canExecute = IsReadyToDuctDesignExecute();
            RoundDuctDesignExecute(canExecute);
            RectangularDuctDesignExecute(canExecute);
        }
        private bool IsReadyToDuctDesignExecute()
        {
            if (Flow > 0.0 && TargetValue > 0.0 && RectangularDuctHeight > 0.0 && RelativeRoughness > 0 && DuctLenght > 0)
                return true;
            else
                return false;
        }
        private void RoundDuctDesignExecute(bool canExecute)
        {
            if (canExecute)
            {
                RoundDuctDesigner.Execute(AirFlow,
                    Approximation,
                    RelativeRoughness,
                    DuctLenght,
                    SelectionType,
                    TargetValue,
                    LocalLosses);
                RoundDuctList = RoundDuctDesigner.DuctCollection;
            }
        }
        private void RectangularDuctDesignExecute(bool canExecute)
        {
            if (canExecute)
            {
                RectangularDuctDesigner.Execute(AirFlow,
                        Approximation,
                        RelativeRoughness,
                        DuctLenght,
                        RectangularDuctHeight,
                        SelectionType,
                        TargetValue,
                        LocalLosses);
                RectangularDuctList = RectangularDuctDesigner.DuctCollection;
            }
        }

        #endregion
    }
    enum SelectionType
    {
        Velocity,
        Pressure
    }
}
