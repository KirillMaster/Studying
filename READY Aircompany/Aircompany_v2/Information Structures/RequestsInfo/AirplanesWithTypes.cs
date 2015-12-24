using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
   public class AirplanesWithTypes
    {
        public int? ID_airplane { get; set; }
        public int? airplaneTypeId { get; set; }
        public int? econom_seats_count { get; set; }
        public int? business_seats_count { get; set; }
        public int? crew_count { get; set; }
        public int? carrying { get; set; }
        public string name { get; set; }


        public AirplanesWithTypes(int? airplaneID, int? airplaneTypeId, int? economSeatsCount, int? businessSeatsCount, int? crewCount, int? carrying, string name)
        {
            ID_airplane = airplaneID;
            this.airplaneTypeId = airplaneTypeId;
            econom_seats_count = economSeatsCount;
            business_seats_count = businessSeatsCount;
            crew_count = crewCount;
            this.carrying = carrying;
            this.name = name;
        }
        
    }
}
