using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class FlightInfo : Info
    {
        public int? FlightID { get; set; }
        public int? AirplaneID { get; set; }
        public string CityDeparture { get; set; }
        public string CityDestination { get; set; }
        public string AirportDeparture { get; set; }
        public string AirportDestination { get; set; }
        public string TerminalDeparture { get; set; }
        public string TerminalDestination { get; set; }
        public string GateDeparture { get; set; }
        public string GateDestination { get; set; }
        public TimeSpan? TimeFlight { get; set; }
        public int? MilesCount { get; set; }

        public FlightInfo(int? flightID, int? airplaneID, string cityDeparture, string cityDestination, string airportDeparture, string airportDestination,
            string terminalDeparture, string terminalDestination, string gateDeparture, string gateDestination, TimeSpan? timeFlight, int? milesCount)
        {
            FlightID = flightID;
            AirplaneID = airplaneID;
            CityDeparture = cityDeparture;
            CityDestination = cityDestination;
            AirportDeparture = airportDeparture;
            AirportDestination = airportDestination;
            TerminalDeparture = terminalDeparture;
            TerminalDestination = terminalDestination;
            GateDeparture = gateDeparture;
            GateDestination = gateDestination;
            TimeFlight = timeFlight;
            MilesCount = milesCount;
        }

    

    }
}