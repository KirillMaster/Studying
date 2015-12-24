using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Airport
{
    class PassengerLogic : Logic, ICrudErrorable
    {
        private AirportEntities databaseContext;
        public  List<Passenger> passengers;
        Passenger passenger;
        public PassengerInfo Information { get; set; }



        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<Passenger>> DataChanged;

        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }

        public PassengerLogic()
        {
            databaseContext = new AirportEntities();
            passengers = new List<Passenger>();
        }

        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, passengers);
        }
        public void SelectAll()
        {
         
            try
            {
                passengers = databaseContext.Passenger.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }

        private Passenger CreateEntity()
        {
            return CreateEntity(Information);
        }
        
        private Passenger CreateEntity(PassengerInfo info)
        {
            try
            {
                passenger = new Passenger();
                passenger.BirthDate = info.BirthDate;
                passenger.fio = info.FamilyName + " " + info.Name + " " + info.Surname;
                passenger.ID_passport = info.PassportID.Value;
                passenger.sex = info.Sex;
            
                
            }
            catch(Exception)
           {
               return null;
           }
            return passenger;
        }
     
        public void Insert()
        {
            if (isNotInformationExists()) return;
            try
           {
                passenger = CreateEntity();
                databaseContext.Passenger.Add(passenger);
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
            Information = new PassengerInfo(ID, "1", "1", "1", DateTime.Now, "мужской");
        }
        public Passenger SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                Passenger passenger = databaseContext.Passenger.Single(item => (item.ID_passport== Information.PassportID));
                return passenger;
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
            Passenger passenger = SelectItemByPrimaryKey();
            try
            {
                   if(passenger == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.Passenger.Remove(passenger);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(PassengerInfo oldInfo,PassengerInfo newInfo)
        {
            try
            {
                Information = oldInfo;
                Passenger oldPassenger = SelectItemByPrimaryKey();
                databaseContext.Passenger.Remove(oldPassenger);
                Information = newInfo;
                Passenger newPassenger = CreateEntity();
                databaseContext.Passenger.Add(newPassenger);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if (EditError != null)
                    EditError(this, e);
            }

        }
        public void test()
        {

            DeleteByPrimaryKey();
        }
    }
}
