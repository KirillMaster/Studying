using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class FlightLogic : Logic, ICrudErrorable
    {
        private AirportEntities databaseContext;
        public List<Flight> flights { get; private set; }
        Flight Flight;
        SeatLogic seatLogic;
        List<int> seats;
       
         public  FlightInfo Information { get; set; }
        AirplaneLogic airplane;


        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<Flight>> DataChanged;
         
        public FlightLogic()
        {
            databaseContext = new AirportEntities();
            flights = new List<Flight>();
            airplane = new AirplaneLogic();
           
        }
        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, flights);
        }
        public void SelectAll()
        {
         
            try
            {
                flights = databaseContext.Flight.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }
        public void Edit(FlightInfo oldInfo, FlightInfo newInfo)
        {

            try
            {
                Information = oldInfo;
                Flight oldFlight = SelectItemByPrimaryKey();
                databaseContext.Flight.Remove(oldFlight);
                Information = newInfo;
                Flight newFlight = CreateEntity();
                databaseContext.Flight.Add(newFlight);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if (EditError != null)
                    EditError(this, e);
            }
        }
        public void Insert()
        {

            try
            {
                Flight = CreateEntity();
                databaseContext.Flight.Add(Flight);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if (InsertionError != null)
                    InsertionError(this, e);
            }

        }
        private Flight CreateEntity()
        {
            return CreateEntity(Information);
        }
        private int getMaxId()
        {
            int max = 0;
            try
            {
                max = databaseContext.Flight.Max((flight) => flight.ID_flight);
            }
            catch(Exception e)
            {
                MessageBox.Show("GetmaxIdError");
            }
            return max;
        }
        private Flight CreateEntity(FlightInfo info)
        {
            if (isNotForeignKeyExist()) return null;

            try
            {
                Flight = new Flight();
                Flight.airport_departure = info.AirportDeparture;
                Flight.airport_destination = info.AirportDestination;
                Flight.city_departure = info.CityDeparture;
                Flight.city_destination = info.CityDestination;
                Flight.gates_destination = info.GateDestination;
                Flight.gates_departure = info.GateDeparture;
                Flight.miles_count = info.MilesCount;
                Flight.time_flight = info.TimeFlight;
                Flight.terminal_departure = info.TerminalDeparture;
                Flight.terminal_destination = info.TerminalDestination;
                Flight.ID_airplane = info.AirplaneID;
                Flight.ID_flight = getMaxId() + 1;
               
            }
            catch(Exception)
           {
               return null;
           }
            return Flight;
        }
        private bool isNotForeignKeyExist()
        {

            airplane.SetInformationOnlyByPrimaryKey(Information.AirplaneID.Value);
            Airplane FoundedAirplane = airplane.SelectItemByPrimaryKey();
            if (FoundedAirplane != null) return false;
            return true;
        }
        public void SetInformationOnlyByPrimaryKey(int? ID)
        {
            Information = new FlightInfo(ID, 0, "1", "1", "1", "1", "1", "1", "1", "1", new TimeSpan(0, 0, 0), 0);
        }
        public void DeleteByPrimaryKey()
        {
            if (isNotInformationExists()) return;
            
            Flight Flight = SelectItemByPrimaryKey();
            try
            {
                if (Flight == null)
                {
                    throw new NullReferenceException();
                }
               // CascadeDeleteSeatsByFlightID();
                databaseContext.Flight.Remove(Flight);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if (DeleteError != null)
                    DeleteError(this, e);
            }

        }
        private void CascadeDeleteSeatsByFlightID()
        {
            seatLogic = new SeatLogic();
            seats = databaseContext.Seat.Select(item => item.ID_seat).
                                         Where(id => (id == Information.FlightID)).
                                         ToList();
            foreach(int seatId in seats)
            {
                seatLogic.SetInformationOnlyByPrimaryKey(seatId);
                seatLogic.DeleteByPrimaryKey();
            }
        }
        public Flight SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                Flight Flight = databaseContext.Flight.Single(item => (item.ID_flight == Information.FlightID));
                return Flight;
            }
            catch (Exception e)
            {
                if (SelectionError != null)
                    SelectionError(this, e);
            }
            return null;
        }
        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }
        public void test()
        {
      

            Information = new FlightInfo(2, 1, "Симферополь", "Москва", "аэр1", "аэр2","1","2","A","B",new TimeSpan(10,20,30),2500);
           
            Insert();
          
         
        }
    }
}
