using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class AirplaneTypeCoefficientInfo
    {
        public int AirplaneType { get; set; }
        public float AirplaneTypeCoefficient { get; set; }
        public AirplaneTypeCoefficientInfo(int AirplaneType, float AirplaneTypeCoefficient)
        {
            this.AirplaneType = AirplaneType;
            this.AirplaneTypeCoefficient = AirplaneTypeCoefficient;
        }
    }
}