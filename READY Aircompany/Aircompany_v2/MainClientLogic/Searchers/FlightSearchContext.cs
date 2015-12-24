using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class FlightSearchContext
    {
        FlightInfo flightInfo;
        FlightStatusInfo statusInfo;
        public List<FlightInfo> Flights;
        public List<FlightStatusInfo> StatusFlights;
        AirportEntities databaseContext;
        public FlightSearchContext()
        {
            Flights = new List<FlightInfo>();
            databaseContext = new AirportEntities();
            StatusFlights = new List<FlightStatusInfo>();
        }

        public List<FlightInfo> Search(FlightInfo info)
        {
            flightInfo = info;
            Flights.Clear();
            Func<Flight, bool> predicate = PredicateToFlightSearch;
            var res = databaseContext.Flight.Select(item => item).Where(predicate);
            foreach (var item in res)
            {
                Flights.Add(new FlightInfo(item.ID_flight,
                    item.ID_airplane.Value,
                    item.city_departure,
                    item.city_destination,
                    item.airport_departure,
                    item.airport_destination,
                    item.terminal_departure,
                    item.terminal_destination,
                    item.gates_departure,
                    item.gates_destination,
                    item.time_flight.Value,
                    item.miles_count.Value));
            }
            return Flights;
        }
        public List<FlightStatusInfo> SearchByStatusInfo(FlightStatusInfo info)
        {
            statusInfo = info;
            StatusFlights.Clear();
            Func<StatusFlight, bool> predicate = PredicateToStatusSearch;
            var res = databaseContext.StatusFlight.Select(item => item).Where(predicate);
            foreach (var item in res)
            {
                StatusFlights.Add(new FlightStatusInfo(item.ID_flight,
                   item.status_flight,
                    item.date_departure,
                    item.date_destination,
                    item.time_departure));
            }
            return StatusFlights;

        }
        private bool CheckForEqualStrings(string str1, string str2)
            {
                if (str1 == null || str2 == null) return false;
                return str1.Trim().Equals(str2.Trim());
            }
        private int StatusEmptyFieldsCount(FlightStatusInfo info)
        {
            int countOfNotEmptyStrings = 0;
            if (info.DateDeparture != null) countOfNotEmptyStrings++;
            if (info.DateDestination != null) countOfNotEmptyStrings++;
            if (info.FlightID != null) countOfNotEmptyStrings++;
            if (info.FlightStatus != "") countOfNotEmptyStrings++;
            if (info.TimeDeparture != null) countOfNotEmptyStrings++;
            return countOfNotEmptyStrings;
        }
        private int StatusEqualFields(StatusFlight status, FlightStatusInfo info)
        {
            int currentCountOfEqualFields = 0;
            if (status.date_departure.Equals(info.DateDeparture)) currentCountOfEqualFields++;
            if (status.date_destination.Equals(info.DateDestination)) currentCountOfEqualFields++;
            if (status.ID_flight == info.FlightID) currentCountOfEqualFields++;
            if (status.status_flight.Trim().Equals(info.FlightStatus.Trim())) currentCountOfEqualFields++;
            if (status.time_departure.Equals(info.TimeDeparture)) currentCountOfEqualFields++;
            return currentCountOfEqualFields;
        }
        public int getCountOfNotEmptyFieldsInSearchRequest(FlightInfo flightInfo)
        {
            int countOfNotEmptyStrings = 0;
            if (flightInfo.AirplaneID != null) countOfNotEmptyStrings++;
            if (flightInfo.AirportDeparture != null) countOfNotEmptyStrings++;
            if (flightInfo.AirportDestination != null) countOfNotEmptyStrings++;
            if (flightInfo.CityDeparture != null) countOfNotEmptyStrings++;
            if (flightInfo.CityDestination != null) countOfNotEmptyStrings++;
            if (flightInfo.FlightID != null) countOfNotEmptyStrings++;
            if (flightInfo.GateDeparture != null) countOfNotEmptyStrings++;
            if (flightInfo.GateDestination != null) countOfNotEmptyStrings++;
            if (flightInfo.MilesCount != null) countOfNotEmptyStrings++;
            if (flightInfo.TerminalDeparture != null) countOfNotEmptyStrings++;
            if (flightInfo.TerminalDestination != null) countOfNotEmptyStrings++;
            if (flightInfo.TimeFlight != null) countOfNotEmptyStrings++;
            return countOfNotEmptyStrings;
        }
        public int getCountOfEqualFields(Flight flight, FlightInfo flightInfo)
        {
            int currentCountOfEqualFields = 0;
            if (flightInfo.AirplaneID == flight.ID_airplane) currentCountOfEqualFields++;
            if (CheckForEqualStrings(flightInfo.AirportDeparture, flight.airport_departure)) currentCountOfEqualFields++;
            if (CheckForEqualStrings(flightInfo.AirportDestination, flight.airport_destination)) currentCountOfEqualFields++;
            if (CheckForEqualStrings(flightInfo.CityDeparture, flight.city_departure)) currentCountOfEqualFields++;
            if (CheckForEqualStrings(flightInfo.CityDestination, flight.city_destination)) currentCountOfEqualFields++;
            if (flightInfo.FlightID.Equals(flight.ID_flight)) currentCountOfEqualFields++;
            if (CheckForEqualStrings(flightInfo.GateDeparture, flight.gates_departure)) currentCountOfEqualFields++;
            if (CheckForEqualStrings(flightInfo.GateDestination, flight.gates_destination)) currentCountOfEqualFields++;
            if (flightInfo.MilesCount == flight.miles_count) currentCountOfEqualFields++;
            if (CheckForEqualStrings(flightInfo.TerminalDeparture, flight.terminal_departure)) currentCountOfEqualFields++;
            if (CheckForEqualStrings(flightInfo.TerminalDestination, flight.terminal_destination)) currentCountOfEqualFields++;
            if (flightInfo.TimeFlight.Equals(flight.time_flight)) currentCountOfEqualFields++;

            return currentCountOfEqualFields;
        }
        private bool PredicateToStatusSearch(StatusFlight status)
        {
            int neededCountOfInitializedFields = StatusEmptyFieldsCount(statusInfo);
            int currentCountOfEqualFields = StatusEqualFields(status, statusInfo);
            if (neededCountOfInitializedFields == 0) return false;
            if (currentCountOfEqualFields == 0) return false;
            if (neededCountOfInitializedFields == currentCountOfEqualFields) return true;
            return false;
        }
      
        private bool PredicateToFlightSearch(Flight flight)
        {
            int neededCountOfInitializedFields = getCountOfNotEmptyFieldsInSearchRequest(flightInfo);
            int currentCountOfEqualFields = getCountOfEqualFields(flight,flightInfo);
            if (neededCountOfInitializedFields == 0) return false;
            if (currentCountOfEqualFields == 0) return false;
            if (neededCountOfInitializedFields == currentCountOfEqualFields) return true;
            return false;

        }
        
      
    }
}
