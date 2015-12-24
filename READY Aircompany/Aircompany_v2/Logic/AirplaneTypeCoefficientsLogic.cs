using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class AirplaneTypeCoefficientsLogic : Logic, ICrudErrorable
    {
            private AirportEntities databaseContext;
        private List<AirplaneTypeCoefficient> AirplaneTypeCoefficients;
        AirplaneTypeCoefficient AirplaneTypeCoefficient;
        AirplaneType airplaneType;
       
        AirplaneTypeCoefficientInfo Information { get; set; }
   
  

        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<AirplaneTypeCoefficient>> DataChanged;


        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }
        public AirplaneTypeCoefficientsLogic()
        {
            databaseContext = new AirportEntities();
            AirplaneTypeCoefficients = new List<AirplaneTypeCoefficient>();
            airplaneType = new AirplaneType();
           
        }

        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, AirplaneTypeCoefficients);
        }
        public void SelectAll()
        {
         
            try
            {
                AirplaneTypeCoefficients = databaseContext.AirplaneTypeCoefficient.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }

        private AirplaneTypeCoefficient CreateEntity()
        {
            return CreateEntity(Information);
        }
        
        private AirplaneTypeCoefficient CreateEntity(AirplaneTypeCoefficientInfo info)
        {
            if (isNotForeignKeyExist()) return null;
            try
            {
                AirplaneTypeCoefficient = new AirplaneTypeCoefficient();
                AirplaneTypeCoefficient.airplaneTypeId = info.AirplaneType;
                AirplaneTypeCoefficient.coefficient = info.AirplaneTypeCoefficient;
            }
            catch(Exception)
           {
               return null;
           }
            return AirplaneTypeCoefficient;
        }
        private bool isNotForeignKeyExist()
        {
            AirplaneTypeLogic airplaneTypeLogic = new AirplaneTypeLogic();
            airplaneTypeLogic.SetInformationOnlyByPrimaryKey(Information.AirplaneType);
            AirplaneType AirplaneTypeFoundedByForeignKey = airplaneTypeLogic.SelectItemByPrimaryKey();

            if (AirplaneTypeFoundedByForeignKey != null) return false;
            return true;
        }
        public void Insert()
        {
           
            try
           {
                AirplaneTypeCoefficient = CreateEntity();
                databaseContext.AirplaneTypeCoefficient.Add(AirplaneTypeCoefficient);
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
            Information = new AirplaneTypeCoefficientInfo(ID,0.25f);
        }
        public AirplaneTypeCoefficient SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                AirplaneTypeCoefficient AirplaneTypeCoefficient = databaseContext.AirplaneTypeCoefficient.Single(item => (item.airplaneTypeId == Information.AirplaneType));
                return AirplaneTypeCoefficient;
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
            AirplaneTypeCoefficient AirplaneTypeCoefficient = SelectItemByPrimaryKey();
            try
            {
                   if(AirplaneTypeCoefficient == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.AirplaneTypeCoefficient.Remove(AirplaneTypeCoefficient);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(AirplaneTypeCoefficientInfo oldInfo,AirplaneTypeCoefficientInfo newInfo)
        {

            try
            {
                Information = oldInfo;
                AirplaneTypeCoefficient oldAirplaneTypeCoefficient = SelectItemByPrimaryKey();
                databaseContext.AirplaneTypeCoefficient.Remove(oldAirplaneTypeCoefficient);
                Information = newInfo;
                AirplaneTypeCoefficient newAirplaneTypeCoefficient = CreateEntity();
                databaseContext.AirplaneTypeCoefficient.Add(newAirplaneTypeCoefficient);
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

            Information = new AirplaneTypeCoefficientInfo(0, 0.25f);
            Insert();
         
        }
       
    }
}
