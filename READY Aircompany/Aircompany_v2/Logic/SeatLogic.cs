using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class SeatLogic : Logic, ICrudErrorable
    {
         private AirportEntities databaseContext;
        private List<Seat> Seats;
        Seat Seat;
        
       
       public SeatInfo Information { get; set; }
        FlightLogic flight;


        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<Seat>> DataChanged;


        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }
        public SeatLogic()
        {
            databaseContext = new AirportEntities();
            Seats = new List<Seat>();
            flight = new FlightLogic();
           
        }

        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, Seats);
        }
        public void SelectAll()
        {
         
            try
            {
                Seats = databaseContext.Seat.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }

        private Seat CreateEntity()
        {
            return CreateEntity(Information);
        }
        
        private Seat CreateEntity(SeatInfo info)
        {
           // if (isNotForeignKeyExist()) return null;
            try
            {
                Seat = new Seat();
                Seat.ID_seat = info.SeatID;
                Seat.number_seat = info.SeatNumber;
                Seat.@class = info.Class;
                Seat.status_seat = info.Status;

                

                   
            }
            catch(Exception)
           {
               return null;
           }
            return Seat;
        }
      /*  private bool isNotForeignKeyExist()
        {
            
            flight.SetInformationOnlyByPrimaryKey(Information.FlightID);
            Flight FlightFoundedByForeignKey = flight.SelectItemByPrimaryKey();

            if (FlightFoundedByForeignKey != null) return false;
            return true;
        }*/
        public void Insert()
        {
           
            try
           {
                Seat = CreateEntity();
                databaseContext.Seat.Add(Seat);
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
            Information = new SeatInfo(ID, 0, 1,1);
        }
        public Seat SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                Seat Seat = databaseContext.Seat.Single(item => (item.ID_seat == Information.SeatID));
                return Seat;
            }
           catch(Exception e)
            {
               if(SelectionError!=null)
                SelectionError(this, e);
            }
            return null;
        }
        public int getMaxId()
        {
            int max = 0;
            try
            {
                max = databaseContext.Seat.Max((seat) => seat.ID_seat);
            }
            catch (Exception e)
            {
                //MessageBox.Show("GetMaxIdError!");
                max = 0;
            }
            return max;
        }
        public void DeleteByPrimaryKey()
        {
            if (isNotInformationExists()) return;
            Seat Seat = SelectItemByPrimaryKey();
            try
            {
                   if(Seat == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.Seat.Remove(Seat);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(SeatInfo oldInfo,SeatInfo newInfo)
        {

            try
            {
                Information = oldInfo;
                Seat oldSeat = SelectItemByPrimaryKey();
                databaseContext.Seat.Remove(oldSeat);
                Information = newInfo;
                Seat newSeat = CreateEntity();
                databaseContext.Seat.Add(newSeat);
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
  
            Information = new SeatInfo(6, 1,  1, 1);
            Insert();
         
        }
    }
}
