using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class AirplaneTypeInfo : Info
    {
        public int? ID { get; set; }
        public string TypeName { get; set; }
        public int? Carrying { get; set; }
        public int? CrewCount { get; set; }
        public int? BussinessCount { get; set; }
        public int? EconomCount { get; set; }

        public AirplaneTypeInfo(int? ID, string typeName, int? carrying, int? crewCount, int? bussinessCount, int? economCount)
        {
            this.ID = ID;
            TypeName = typeName;
            Carrying = carrying;
            CrewCount = crewCount;
            BussinessCount = bussinessCount;
            EconomCount = economCount;
        }
    }
}
