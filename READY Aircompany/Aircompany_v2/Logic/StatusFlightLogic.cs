using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class StatusFlightLogic : Logic, ICrudErrorable
    {
            private AirportEntities databaseContext;
        public List<StatusFlight> StatusFlights;
        StatusFlight StatusFlight;


        public FlightStatusInfo Information { get; set; }
        FlightLogic flight;


        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<StatusFlight>> DataChanged;


        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }
        public StatusFlightLogic()
        {
            databaseContext = new AirportEntities();
            StatusFlights = new List<StatusFlight>();
            flight = new FlightLogic();
           
        }

        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, StatusFlights);
        }
        public void SelectAll()
        {
         
            try
            {
                StatusFlights = databaseContext.StatusFlight.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }

        private StatusFlight CreateEntity()
        {
            return CreateEntity(Information);
        }

        private StatusFlight CreateEntity(FlightStatusInfo info)
        {
            if (isNotForeignKeyExist()) return null;
            try
            {
                StatusFlight = new StatusFlight();
                StatusFlight.date_departure = info.DateDeparture;
                StatusFlight.date_destination = info.DateDestination;
                StatusFlight.status_flight = info.FlightStatus;
                StatusFlight.time_departure = info.TimeDeparture;
                StatusFlight.ID_flight = info.FlightID.Value;

            
            }
            catch(Exception)
           {
               return null;
           }
            return StatusFlight;
        }
        private bool isNotForeignKeyExist()
        {
            
            flight.SetInformationOnlyByPrimaryKey(Information.FlightID);
            Flight FlightFoundedByForeignKey = flight.SelectItemByPrimaryKey();

            if (FlightFoundedByForeignKey != null) return false;
            return true;
        }
        public void Insert()
        {
           
            try
           {
                StatusFlight = CreateEntity();
                databaseContext.StatusFlight.Add(StatusFlight);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
           }
            catch(Exception e)
            {
                if(InsertionError!=null)
                InsertionError(this,e);
            }
           
        }
     
        public void SetInformationOnlyByPrimaryKey(int ID)
        {
            Information = new FlightStatusInfo(ID,"1",DateTime.Now,DateTime.Now,new TimeSpan(0,0,0));
        }
        public StatusFlight SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                StatusFlight StatusFlight = databaseContext.StatusFlight.Single(item => (item.ID_flight == Information.FlightID));
                return StatusFlight;
            }
           catch(Exception e)
            {
               if(SelectionError!=null)
                SelectionError(this, e);
            }
            return null;
        }
        public void DeleteByPrimaryKey()
        {
            if (isNotInformationExists()) return;
            StatusFlight StatusFlight = SelectItemByPrimaryKey();
            try
            {
                   if(StatusFlight == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.StatusFlight.Remove(StatusFlight);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(FlightStatusInfo oldInfo, FlightStatusInfo newInfo)
        {

            try
            {
                Information = oldInfo;
                StatusFlight oldStatusFlight = SelectItemByPrimaryKey();
                if(oldStatusFlight!=null)
                databaseContext.StatusFlight.Remove(oldStatusFlight);
                Information = newInfo;
                StatusFlight newStatusFlight = CreateEntity();
                databaseContext.StatusFlight.Add(newStatusFlight);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if(EditError!=null)
                EditError(this, e);
            }
        }
        public void test()
        {
        
            Information = new FlightStatusInfo(1, "задержан", DateTime.Now, DateTime.Now, new TimeSpan(0, 0, 0));
            Insert();
         
        }
    }
}
