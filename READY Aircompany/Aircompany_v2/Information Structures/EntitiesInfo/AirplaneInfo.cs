using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class AirplaneInfo : Info
    {
        public int? AirplaneID { get; set; }
        public int? AirplaneType { get; set; }

        public AirplaneInfo(int airplaneID, int airplaneType)
        {
            AirplaneID = airplaneID;
            AirplaneType = airplaneType;
        }
    }
}
