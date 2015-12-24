using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class DataChangedEventsRedirector
    {
        LogicPartsKeeper logicParts = LogicPartsKeeper.getInstance();

        public EventHandler<List<Airplane>> airplaneHandler;
        public EventHandler<List<AirplaneType>> airplaneTypeHandler;
        public EventHandler<List<AirplaneTypeCoefficient>> airplaneTypeCoefficientHandler;
        public EventHandler<List<Accounts>> accountsHandler;
        public EventHandler<List<Flight>> flightHandler;
        public EventHandler<List<Passenger>> passengerHandler;
        public EventHandler<List<PermanentCoefficients>> permanentCoefficientsHandler;
        public EventHandler<List<Seat>> seatHandler;
        public EventHandler<List<StatusFlight>> statusFlightHandler;
        public EventHandler<List<Ticket>> ticketHandler;



        public DataChangedEventsRedirector(     EventHandler<List<Airplane>> airplaneHandler,
                                                EventHandler<List<Accounts>> accountHandler,
                                                EventHandler<List<AirplaneTypeCoefficient>> airplaneTypeCoefficientHandler,
                                                EventHandler<List<AirplaneType>> airplaneTypeHandler,
                                                EventHandler<List<Flight>> flightHandler,
                                                EventHandler<List<Passenger>> passengerHandler,
                                                EventHandler<List<PermanentCoefficients>> permanentCoefficientHandler,
                                                EventHandler<List<Seat>> seatHandler,
                                                EventHandler<List<StatusFlight>> statusFlightHandler,
                                                EventHandler<List<Ticket>> ticketHandler
                                          )
        {

            this.airplaneHandler = airplaneHandler;
            this.airplaneTypeHandler = airplaneTypeHandler;
            this.airplaneTypeCoefficientHandler = airplaneTypeCoefficientHandler;
            this.accountsHandler = accountHandler;
            this.flightHandler = flightHandler;
            this.passengerHandler = passengerHandler;
            this.permanentCoefficientsHandler = permanentCoefficientHandler;
            this.seatHandler = seatHandler;
            this.statusFlightHandler = statusFlightHandler;
            this.ticketHandler = ticketHandler;
        }
        public void Subscribe()
        {
            if (airplaneHandler != null)
                logicParts.airplaneLogic.DataChanged += airplaneHandler;
            if (accountsHandler != null)
                logicParts.accountLogic.DataChanged += accountsHandler;
            if (airplaneTypeCoefficientHandler != null)
                logicParts.airplaneTypeCoefficientLogic.DataChanged += airplaneTypeCoefficientHandler;
            if (airplaneTypeHandler != null)
                logicParts.airplaneTypeLogic.DataChanged += airplaneTypeHandler;
            if (flightHandler != null)
                logicParts.flightLogic.DataChanged += flightHandler;
            if (permanentCoefficientsHandler != null)
                logicParts.permanentCoefficientLogic.DataChanged += permanentCoefficientsHandler;
            if (seatHandler != null)
                logicParts.seatLogic.DataChanged += seatHandler;
            if (statusFlightHandler != null)
                logicParts.statusFlightLogic.DataChanged += statusFlightHandler;
            if (ticketHandler != null)
                logicParts.ticketLogic.DataChanged += ticketHandler;
            if (passengerHandler != null)
                logicParts.passengerLogic.DataChanged += passengerHandler;

        }
    }

}
