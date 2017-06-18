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
    class RoundDuctDesigner
    {
        private double[] diameterList = { 0.08, 0.1, 0.125, 0.16, 0.2, 0.25,
                                          0.3, 0.315, 0.350, 0.400, 0.450,
                                        0.500, 0.550, 0.600, 0.650, 0.700,
                                        0.750, 0.800, 0.850, 0.900, 0.950, 1.0 };
        public ObservableCollection<RoundDuctViewModel> DuctCollection { get; set; }
        public void Execute(
            AirFlow airFloe, 
            DarcyFrictionFactorApproximation approximation, 
            double relativeRoughness,
            double ductLenght,
            SelectionType selType,
            double targetVal,
            ObservableCollection<LocalLoss> localLosses)
        {
            DuctCollection = new ObservableCollection<RoundDuctViewModel>();
            foreach(double d in diameterList)
            {
                RoundDuctViewModel duct = new RoundDuctViewModel(approximation,relativeRoughness, d, ductLenght, airFloe,targetVal);
                duct.LocalLosses = localLosses.Where(x=>x.LocalLossCoefficient>0.0).ToList();
                DuctCollection.Add(duct);
            }

        }
    }
}
