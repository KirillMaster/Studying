using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class PermanentCoefficientInfo
    {
        public int ID { get; set; }
        public float PerMile { get; set; }
        public float EconomClass { get; set; }
        public float BusinessClass { get; set; }
        public float BuyDateCoefficient { get; set; }

        public PermanentCoefficientInfo(int ID, float PerMile, float EconomClass, float BusinessClass, float BuyDateCoefficient)
        {
            this.ID = ID;
            this.PerMile = PerMile;
            this.EconomClass = EconomClass;
            this.BusinessClass = BusinessClass;
            this.BuyDateCoefficient = BuyDateCoefficient;
        }
    }
}