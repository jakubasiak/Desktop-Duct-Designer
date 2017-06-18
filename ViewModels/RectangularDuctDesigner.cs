using HVAC.Elements;
using HVAC.FluidMechanics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVACDesigner.ViewModels
{
    class RectangularDuctDesigner
    {
        private double[] aSizeList = { 0.05,0.1,0.15,0.2,0.25,0.3,0.35,0.4,0.45,0.5,0.55,0.6,0.65,0.7,0.8,0.85,0.9,0.95,1.0,1.05,1.1,1.15,1.2,1.25,1.3,1.35,1.4,1.45,1.5};
        public ObservableCollection<RectangularDuctViewModel> DuctCollection { get; set; }
        public void Execute(
            AirFlow airFloe,
            DarcyFrictionFactorApproximation approximation,
            double relativeRoughness,
            double ductLenght,
            double bSize,
            SelectionType selType,
            double targetVal,
            ObservableCollection<LocalLoss> localLosses)
        {
            DuctCollection = new ObservableCollection<RectangularDuctViewModel>();
            foreach (double aSize in aSizeList)
            {
                RectangularDuctViewModel duct = new RectangularDuctViewModel(approximation, relativeRoughness, aSize, bSize, ductLenght, airFloe,targetVal);
                duct.LocalLosses = localLosses.Where(x => x.LocalLossCoefficient > 0.0).ToList();
                DuctCollection.Add(duct);
            }
        }
    }
}
