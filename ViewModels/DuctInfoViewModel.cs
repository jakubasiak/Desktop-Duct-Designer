using HVAC.Elements;
using HVAC.FluidMechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HVACDesigner.ViewModels
{
    class DuctInfoViewModel
    {
        public double LocalPressureDrop { get; set; }
        public double LinearPressureDrop { get; set; }
        public DarcyFrictionFactorApproximation Approximation { get; set; }
        public double RelativeRoughness { get; set; }
        public double Length { get; set; }
        public double AirFlow { get; set; }
        public double HydraulicDiameter { get; set; }
        public string Size { get; set; }
        public double Velocity { get; set; }
        public double FrictionFactor { get; set; }
        public double VelocityPressure { get; set; }
        public double FrictionLoss { get; set; }
        public double PressureDrop { get; set; }
        public double ReynoldsNumber { get; set; }
        public List<LocalLoss> LocalLosses { get; set; }

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

        public void CopyPoprtiesValuesFromDuct(BaseDuct duct)
        {
            LocalPressureDrop = duct.LocalPressureDrop;
            LinearPressureDrop = duct.LinearPressureDrop;
            Approximation = duct.Approximation;
            RelativeRoughness = duct.RelativeRoughness;
            Length = duct.Length;
            AirFlow = duct.AirFlow.Flow;
            HydraulicDiameter = duct.HydraulicDiameter;
            Size = duct.Size;
            Velocity = duct.Velocity;
            FrictionFactor = duct.FrictionFactor;
            VelocityPressure = duct.VelocityPressure;
            FrictionLoss = duct.FrictionLoss;
            PressureDrop = duct.PressureDrop;
            ReynoldsNumber = duct.ReynoldsNumber;
            LocalLosses = duct.LocalLosses;
        }
    }
}
