using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class IMainLogic
    {
        public event EventHandler<List<AirplanesWithTypes>> AirplaneWithTypesResponse;

        public event EventHandler<Exception> InsertionError;
        public event EventHandler<Exception> SelectionError;
        public event EventHandler<Exception> DeleteError;
        public event EventHandler<Exception> EditError;
        public event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<Airplane>> AirplanesDataChanged;
        public event EventHandler<List<AirplaneType>> AirplaneTypeDataChanged;
        public event EventHandler<List<AirplaneTypeCoefficient>> AirplaneTypesCoefficientDataChanged;
        public event EventHandler<List<Accounts>> AccountsDataChanged;
        public event EventHandler<List<Flight>> FlightDataChanged;
        public event EventHandler<List<Passenger>> PassengerDataChanged;
        public event EventHandler<List<PermanentCoefficients>> PermanentCoefficientsDataChanged;
        public event EventHandler<List<Seat>> SeatDataChanged;
        public event EventHandler<List<StatusFlight>> StatusFlightDataChanged;
        public event EventHandler<List<Ticket>> TicketDataChanged;
    }
}
