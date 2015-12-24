using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class FlightStatusInfo : Info
    {
        public int? FlightID { get; set; }
        public string FlightStatus { get; set; }
        public DateTime? DateDeparture { get; set; }
        public DateTime? DateDestination { get; set; }
        public TimeSpan? TimeDeparture { get; set; }

        public FlightStatusInfo(int? flightID, string flightStatus, DateTime? dateDeparture, DateTime? dateDestination, TimeSpan? timeDeparture)
        {
            FlightID = flightID;
            FlightStatus = flightStatus;
            DateDeparture = dateDeparture;
            DateDestination = dateDestination;
            TimeDeparture = timeDeparture;
        }
    }
}