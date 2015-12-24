using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class LogicPartsKeeper
    {
        public AccountsLogic accountLogic { get; private set; }
        public AirplaneLogic airplaneLogic { get; private set; }
        public AirplaneTypeCoefficientsLogic airplaneTypeCoefficientLogic { get; private set; }
        public AirplaneTypeLogic airplaneTypeLogic { get; private set; }
        public FlightLogic flightLogic { get; private set; }
        public PassengerLogic passengerLogic { get; private set; }
        public PermanentCoefficientsLogic permanentCoefficientLogic { get; private set; }
        public SeatLogic seatLogic { get; private set; }
        public StatusFlightLogic statusFlightLogic { get; private set; }
        public TicketLogic ticketLogic { get; private set; }

        public List<Logic> LogicPartsList { get; private set; }
        private LogicPartsKeeper()
        {
            Instance = this;
            InitializeLogics();
            InitializeList();
        }
        private void InitializeList()
        {
            LogicPartsList = new List<Logic>();
            LogicPartsList.Add(accountLogic);
            LogicPartsList.Add(airplaneLogic);
            LogicPartsList.Add(airplaneTypeCoefficientLogic);
            LogicPartsList.Add(airplaneTypeLogic);
            LogicPartsList.Add(flightLogic);
            LogicPartsList.Add(passengerLogic);
            LogicPartsList.Add(permanentCoefficientLogic);
            LogicPartsList.Add(seatLogic);
            LogicPartsList.Add(statusFlightLogic);
            LogicPartsList.Add(ticketLogic);
        }
        private void InitializeLogics()
        {
            accountLogic = new AccountsLogic();
            airplaneLogic = new AirplaneLogic();
            airplaneTypeCoefficientLogic = new AirplaneTypeCoefficientsLogic();
            airplaneTypeLogic = new AirplaneTypeLogic();
            flightLogic = new FlightLogic();
            passengerLogic = new PassengerLogic();
            permanentCoefficientLogic = new PermanentCoefficientsLogic();
            seatLogic = new SeatLogic();
            statusFlightLogic = new StatusFlightLogic();
            ticketLogic = new TicketLogic();
        }
       public  static LogicPartsKeeper Instance { get; private set; }
        public static LogicPartsKeeper getInstance()
        {
            if (Instance == null) 
                return new LogicPartsKeeper();
            else return Instance;
        }
    }
}
