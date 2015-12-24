using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class AirplaneLogic : Logic, ICrudErrorable
    {
        public AirportEntities databaseContext { get; private set; }
        public List<Airplane> airplanes { get; private set; }
        Airplane Airplane;
        
        AirplaneTypeLogic airplaneTypeLogic;
        public AirplaneInfo Information { get; set; }



        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<Airplane>> DataChanged;


        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
           
        }
        public AirplaneLogic()
        {
            databaseContext = new AirportEntities();
            airplanes = new List<Airplane>();
            airplaneTypeLogic = new AirplaneTypeLogic();
        }

        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, airplanes);
        }
        public void SelectAll()
        {
         
            try
            {
                airplanes = databaseContext.Airplane.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }

        private Airplane CreateEntity()
        {
            return CreateEntity(Information);
        }
        
        private Airplane CreateEntity(AirplaneInfo info)
        {
            if (isNotForeignKeyExist()) return null;
            try
            {
                Airplane = new Airplane();
                Airplane.airplane_type = info.AirplaneType;
                Airplane.ID_airplane = getMaxId() + 1;
            }
            catch(Exception)
           {
               return null;
           }
            return Airplane;
        }
        private bool isNotForeignKeyExist()
        {
            airplaneTypeLogic.SetInformationOnlyByPrimaryKey(Information.AirplaneType.Value);
            AirplaneType airplaneTypeFindedByForeignKey = airplaneTypeLogic.SelectItemByPrimaryKey();

            if (airplaneTypeFindedByForeignKey != null) return false;
            return true;
        }
        public void Insert()
        {
           
            try
           {
                Airplane = CreateEntity();
                databaseContext.Airplane.Add(Airplane);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
           }
            catch(Exception e)
            {
                if(InsertionError!=null)
                InsertionError(this,e);
            }
           
        }
        private int getMaxId()
        {
            int max = 0;
            try
            {
                max = databaseContext.Airplane.Max((airplane) => airplane.ID_airplane);
            }
            catch (Exception e)
            {
                MessageBox.Show("GetMaxIdError!");
            }
            return max;
        }
        public void SetInformationOnlyByPrimaryKey(int ID)
        {
            Information = new AirplaneInfo(ID, 0);
        }
        public Airplane SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                Airplane Airplane = databaseContext.Airplane.Single(item => (item.ID_airplane == Information.AirplaneID));
                return Airplane;
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
            Airplane Airplane = SelectItemByPrimaryKey();
            try
            {
                   if(Airplane == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.Airplane.Remove(Airplane);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(AirplaneInfo oldInfo,AirplaneInfo newInfo)
        {

            try
            {
                Information = oldInfo;
                Airplane oldAirplane = SelectItemByPrimaryKey();
                databaseContext.Airplane.Remove(oldAirplane);
                Information = newInfo;
                Airplane newAirplane = CreateEntity();
                databaseContext.Airplane.Add(newAirplane);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if(EditError!=null)
                EditError(this, e);
            }
        }
      
        public void DeleteRange(List<AirplaneInfo> range) 
        {
        
           List<Airplane> airplane = new List<Airplane>();
            try
            {
                if (range == null)
                {
                    throw new NullReferenceException();
                }
                foreach(AirplaneInfo plane in range)
                {
                    int ID = plane.AirplaneID.Value;
                    Information = new AirplaneInfo(ID, 0);
                     airplane.Add( SelectItemByPrimaryKey());
                    
                     databaseContext.Airplane.RemoveRange(airplane);
                }

                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if (DeleteError != null)
                    DeleteError(this, e);
            }        
        }
        
        public void test()
        {


            Information = new AirplaneInfo(1222, 0);

            Insert();
         
        }
    }
}
