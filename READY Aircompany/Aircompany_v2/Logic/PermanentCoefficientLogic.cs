using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace Airport
{
    class PermanentCoefficientsLogic : Logic, ICrudErrorable
    {

            private AirportEntities databaseContext;
        public List<PermanentCoefficients> PermanentCoefficientss;
        PermanentCoefficients PermanentCoefficients;
        AirplaneType airplaneType;
       
        public PermanentCoefficientInfo Information { get; set; }



        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<PermanentCoefficients>> DataChanged;


        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }
        public PermanentCoefficientsLogic()
        {
            databaseContext = new AirportEntities();
            PermanentCoefficientss = new List<PermanentCoefficients>();
            airplaneType = new AirplaneType();
           
        }

        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, PermanentCoefficientss);
        }
        public void SelectAll()
        {
         
            try
            {
                PermanentCoefficientss = databaseContext.PermanentCoefficients.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }

        private PermanentCoefficients CreateEntity()
        {
            return CreateEntity(Information);
        }

        private int getMaxId()
        {
            int max = 0;
            try
            {
                max = databaseContext.PermanentCoefficients.Max((coef) => coef.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show("GetmaxIdError");
            }
            return max;
        }
        private PermanentCoefficients CreateEntity(PermanentCoefficientInfo info)
        {
            
            try
            {
                PermanentCoefficients = new PermanentCoefficients();
                PermanentCoefficients.BusinessClassCoefficient = info.BusinessClass;
                PermanentCoefficients.economClassCoefficient = info.EconomClass;
                PermanentCoefficients.buyDateCoefficient = info.BuyDateCoefficient;
                PermanentCoefficients.mileCoefficient = info.PerMile;
                PermanentCoefficients.Id = getMaxId() + 1;
            }
            catch(Exception)
           {
               return null;
           }
            return PermanentCoefficients;
        }
    
        public void Insert()
        {
           
            try
           {
                PermanentCoefficients = CreateEntity();
                databaseContext.PermanentCoefficients.Add(PermanentCoefficients);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
           }
            catch(Exception e)
            {
                if(InsertionError!=null)
                InsertionError(this,e);
            }
           
        }
     

        public PermanentCoefficients SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                PermanentCoefficients PermanentCoefficients = databaseContext.PermanentCoefficients.Single(item => (item.Id == Information.ID));
                return PermanentCoefficients;
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
            PermanentCoefficients PermanentCoefficients = SelectItemByPrimaryKey();
            try
            {
                   if(PermanentCoefficients == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.PermanentCoefficients.Remove(PermanentCoefficients);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(PermanentCoefficientInfo oldInfo,PermanentCoefficientInfo newInfo)
        {

            try
            {
                Information = oldInfo;
                PermanentCoefficients oldPermanentCoefficients = SelectItemByPrimaryKey();
                databaseContext.PermanentCoefficients.Remove(oldPermanentCoefficients);
                Information = newInfo;
                PermanentCoefficients newPermanentCoefficients = CreateEntity();
                databaseContext.PermanentCoefficients.Add(newPermanentCoefficients);
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
        
            Information = new PermanentCoefficientInfo(0, 1000, 1000, 1000, 1000);
            Insert();
         
        }
    }
}
