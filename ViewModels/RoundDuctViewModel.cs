using HVAC.Elements;
using HVAC.FluidMechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVACDesigner.ViewModels
{
    public class RoundDuctViewModel : RoundDuct
    {
        public double TargetValue { get; set; }
        public float VelocityAlphaValue
        {
            get
            {
                double delta = Velocity - TargetValue;
                if (delta == 0.0)
                    return 1.0f;
                else
                    return (float)(1.0 / (Math.Abs(delta) + 1));
            }
        }
        public float PressureAlphaValue
        {
            get
            {
                double delta = FrictionLoss - TargetValue;
                if (delta == 0.0)
                    return 1.0f;
                else
                    return (float)(1.0 / (Math.Abs(delta) + 1));
            }
        }
        public RoundDuctViewModel()
        {

        }
        public RoundDuctViewModel(
                        DarcyFrictionFactorApproximation approximation,
                        double relativeRoughness,
                        double diameter,
                        double length,
                        AirFlow airFlow,
                        double targetValue) : base(approximation, relativeRoughness, diameter, length, airFlow)
        {
            TargetValue = targetValue;
        }

    }
}
