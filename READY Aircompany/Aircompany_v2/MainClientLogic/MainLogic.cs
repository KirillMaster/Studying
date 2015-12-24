using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class MainLogic : IMainLogic
    {
        AirportEntities databaseContext;
        LogicPartsKeeper logicParts;
        ErrorEventsRedirector errorEventsRedirector;
        public event EventHandler<List<AirplanesWithTypes>> AirplaneWithTypesResponse;

        #region сущности поиска данных
        AirplaneSearchContext airplaneSearcher;
        PassengerSearchContext passengerSearcher;
        TicketSearchContext ticketSearcher;
        FlightSearchContext flightSearcher;
        #endregion
        #region события CRUD операций
        public event EventHandler<Exception> InsertionError;
        public event EventHandler<Exception> SelectionError;
        public event EventHandler<Exception> DeleteError;
        public event EventHandler<Exception> EditError;
        public event EventHandler<Exception> AllSelectedError;
        #endregion
        #region События изменения данных из всех сущностей
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
        #endregion

        public MainLogic()
        {
            logicParts = LogicPartsKeeper.getInstance();
          //  testSubscribes();
            RedirectAllEventsExceptDataChanged();
            RedirectAllDataChangedEvents();
            airplaneSearcher = new AirplaneSearchContext();
            passengerSearcher = new PassengerSearchContext();
            ticketSearcher = new TicketSearchContext();
            flightSearcher = new FlightSearchContext();
        }

        private void RedirectAllEventsExceptDataChanged()
        {
            errorEventsRedirector = new ErrorEventsRedirector(AllSelectedError, DeleteError, EditError, InsertionError, SelectionError);
            errorEventsRedirector.Subscribe();
        }
        private void testSubscribes()
        {
            InsertionError += (o, e) => MessageBox.Show("InsertionError " + e.Message + " " + o.GetType().ToString());
            DeleteError += (o, e) => MessageBox.Show("DeleteError " + e.Message + e.Message + " " + o.GetType().ToString());
            EditError += (o, e) => MessageBox.Show("EditError " + e.Message + e.Message + " " + o.GetType().ToString());
            SelectionError += (o, e) => MessageBox.Show("SelectionError " + e.Message + e.Message + " " + o.GetType().ToString());
            AllSelectedError += (o, e) => MessageBox.Show("AllSelectionError " + e.Message + e.Message + " " + o.GetType().ToString());
            AccountsDataChanged += (o,list) => MessageBox.Show("Data Succesfully changed :) "  + " " + o.GetType().ToString());
            AirplanesDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
            AirplaneTypeDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
            AirplaneTypesCoefficientDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
            FlightDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
            PassengerDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
            PermanentCoefficientsDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
            StatusFlightDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
            TicketDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
            SeatDataChanged += (o, list) => MessageBox.Show("Data Succesfully changed :) " + " " + o.GetType().ToString());
        }
        private void RedirectAllDataChangedEvents()
        {
            DataChangedEventsRedirector creator = new DataChangedEventsRedirector(AirplanesDataChanged,
                                                                                  AccountsDataChanged,
                                                                                  AirplaneTypesCoefficientDataChanged,
                                                                                  AirplaneTypeDataChanged,
                                                                                  FlightDataChanged,
                                                                                  PassengerDataChanged,
                                                                                  PermanentCoefficientsDataChanged,
                                                                                  SeatDataChanged,
                                                                                  StatusFlightDataChanged,
                                                                                  TicketDataChanged
                                                                                 );

            creator.Subscribe();
        }
            public void test()
        {/*
            for (int i = 0; i < 10; i++)
            {
                StatusFlightLogic logic = new StatusFlightLogic();
                logic.Information = new FlightStatusInfo(i, "Задержан", DateTime.Now, DateTime.Now.AddHours(12), new TimeSpan(10, 12, 13));
                logic.Insert();
            }
          */
        }
          
    }
}
