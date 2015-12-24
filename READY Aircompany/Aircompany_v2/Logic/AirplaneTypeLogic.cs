using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class AirplaneTypeLogic : Logic, ICrudErrorable
    {
        private AirportEntities databaseContext;
        public List<AirplaneType> airplaneTypeList { get; private set; }
        AirplaneType type;
        public AirplaneTypeInfo Information { get; set; }



        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<AirplaneType>> DataChanged;


        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }
        public void SetInformationOnlyByPrimaryKey(int typeId)
        {
            Information = new AirplaneTypeInfo(typeId, "0", 0, 0, 0, 0);
        }
        public AirplaneTypeLogic()
        {
            databaseContext = new AirportEntities();
            airplaneTypeList = new List<AirplaneType>();
        }

        public void SendDataChangedEvent()
        {
            if (isNotInformationExists()) return;
            SelectAll();
            if(DataChanged!=null)
            DataChanged(this, airplaneTypeList);
        }
        public List<AirplaneType> SelectAll()
        {
           
            try
            {
                airplaneTypeList = databaseContext.AirplaneType.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

            return airplaneTypeList;
        }

        private AirplaneType CreateEntity()
        {
            return CreateEntity(Information);
        }
        
        private AirplaneType CreateEntity(AirplaneTypeInfo info)
        {
            try
            {
                type = new AirplaneType();

                type.airplane_type = getMaxId() +1;
                type.name_ = info.TypeName;
                type.business_seats_count = info.BussinessCount;
                type.carrying = info.Carrying;
                type.crew_count = info.CrewCount;
                type.econom_seats_count = info.EconomCount;
            }
            catch(Exception)
           {
               return null;
           }
            return type;
        }
        private int getMaxId()
        {
            int max = 0;
            try
            {
                max = databaseContext.AirplaneType.Max(type => type.airplane_type);
            }
            catch (Exception)
            {
                MessageBox.Show("GetMaxIdError!");
            }
            return max;
        }
        public void Insert()
        { 
            try
           {
               if (isNotInformationExists()) return;
                type = CreateEntity();
                databaseContext.AirplaneType.Add(type);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
           }
            catch(Exception e)
            {
                if(InsertionError!=null)
                InsertionError(this,e);
            }
           
        }
       
        public AirplaneType SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                AirplaneType type = databaseContext.AirplaneType.Single(item => (item.airplane_type == Information.ID));
                return type;
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
            AirplaneType type = SelectItemByPrimaryKey();
            try
            {
                   if(type == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.AirplaneType.Remove(type);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(AirplaneTypeInfo oldInfo,AirplaneTypeInfo newInfo)
        {
            try
            {
                Information = oldInfo;
                AirplaneType oldAirplaneType = SelectItemByPrimaryKey();
                databaseContext.AirplaneType.Remove(oldAirplaneType);
                Information = newInfo;
                AirplaneType newAirplaneType = CreateEntity();
                databaseContext.AirplaneType.Add(newAirplaneType);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if (EditError != null)
                    EditError(this, e);
            }


        }
        public void DeleteRange(List<AirplaneTypeInfo> range)
        {

            AirplaneType airplane = new AirplaneType();
            try
            {
                if (range == null)
                {
                    throw new NullReferenceException();
                }
                foreach (AirplaneTypeInfo plane in range)
                {
                    Information = new AirplaneTypeInfo(plane.ID, plane.TypeName, plane.Carrying, plane.CrewCount, plane.BussinessCount, plane.EconomCount);
                    airplane = SelectItemByPrimaryKey();
                    databaseContext.AirplaneType.Remove(airplane);

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

            Information = new AirplaneTypeInfo(0, "TU-31", 12, 14, 14, 15);
            Insert();
            
           
   
            
        }
     

        
        
    }
}
