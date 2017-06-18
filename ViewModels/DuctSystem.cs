using HVAC.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Serialization;

namespace HVACDesigner.ViewModels
{

    public class DuctSystem : INotifyPropertyChanged, IEnumerable<BaseDuct>
    {
        private ObservableCollection<BaseDuct> _ductCollection = new ObservableCollection<BaseDuct>();
        public ObservableCollection<BaseDuct> DuctCollection
        {
            get
            {
                return _ductCollection;
            }
            set
            {
                _ductCollection = value;
                OnPropertyChanged("DuctCollection");
            }

        }
        public double TotalPressureDrop
        {
            get
            {
                double pressureDrop = 0.0;
                foreach (var duct in DuctCollection)
                {
                    pressureDrop += duct.PressureDrop;
                }
                return pressureDrop;
            }
        }
        public double LocalPressureDrop
        {
            get
            {
                double pressureDrop = 0.0;
                foreach (var duct in DuctCollection)
                {
                    pressureDrop += duct.LocalPressureDrop;
                }
                return pressureDrop;
            }
        }
        public double LinearPressureDrop
        {
            get
            {
                double pressureDrop = 0.0;
                foreach (var duct in DuctCollection)
                {
                    pressureDrop += duct.LinearPressureDrop;
                }
                return pressureDrop;
            }
        }
        public double TotalSystemLength
        {
            get
            {
                double length = 0.0;
                foreach (var duct in DuctCollection)
                {
                    length += duct.Length;
                }
                return length;
            }
        }
        public double AverageAirVelocity
        {
            get
            {
                double velocity = 0.0;
                foreach (var duct in DuctCollection)
                {
                    velocity += duct.Velocity * duct.Length;
                }
                return velocity / TotalSystemLength;
            }
        }
        public double AverageFrictionLoss
        {
            get
            {
                double frictionLoss = 0.0;
                foreach (var duct in DuctCollection)
                {
                    frictionLoss += duct.FrictionLoss * duct.Length;
                }
                return frictionLoss / TotalSystemLength;
            }
        }

        public int Count
        {
            get { return DuctCollection.Count; }
        }
        public BaseDuct this[int index]
        {
            get
            {
                return DuctCollection[index];
            }
        }

        public DuctSystem()
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        private void ReadOnlyPropertyChanged()
        {
            OnPropertyChanged("TotalPressureDrop");
            OnPropertyChanged("LocalPressureDrop");
            OnPropertyChanged("LinearPressureDrop");
            OnPropertyChanged("TotalSystemLength");
            OnPropertyChanged("AverageAirVelocity");
            OnPropertyChanged("AverageFrictionLoss");
            OnPropertyChanged("Count");
        }
        public void Add(BaseDuct duct)
        {
            DuctCollection.Add(duct);
            ReadOnlyPropertyChanged();
        }
        public void InsertDuct(int index, BaseDuct duct)
        {
            DuctCollection.Insert(index, duct);
            ReadOnlyPropertyChanged();
        }
        public void RemoveDuctAt(int index)
        {
            DuctCollection.RemoveAt(index);
            ReadOnlyPropertyChanged();
        }
        public bool RemoveDuct(BaseDuct duct)
        {
            bool removeOk = DuctCollection.Remove(duct);
            ReadOnlyPropertyChanged();
            return removeOk;
        }
        public void MoveUp(BaseDuct duct)
        {
            int index = DuctCollection.IndexOf(duct);
            if (index - 1 >= 0)
            {
                BaseDuct temp = DuctCollection[index - 1];
                DuctCollection[index - 1] = duct;
                DuctCollection[index] = temp;
            }

        }
        public void MoveDown(BaseDuct duct)
        {
            int index = DuctCollection.IndexOf(duct);
            if (index + 1 < DuctCollection.Count)
            {
                BaseDuct temp = DuctCollection[index + 1];
                DuctCollection[index + 1] = duct;
                DuctCollection[index] = temp;
            }

        }
        public void RemoveAllDucts()
        {
            DuctCollection.Clear();
        }

        public IEnumerator<BaseDuct> GetEnumerator()
        {
            return DuctCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }
    }
}
