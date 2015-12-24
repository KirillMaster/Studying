using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class AccountsLogic : Logic, ICrudErrorable
    {
        private AirportEntities databaseContext;
        private List<Accounts> Accounts;
        Accounts Account;
        
       
        AccountInfo Information { get; set; }
        FlightLogic flight;


        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<Accounts>> DataChanged;


        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }
        public AccountsLogic()
        {
            databaseContext = new AirportEntities();
            Accounts = new List<Accounts>();
            flight = new FlightLogic();
           
        }

        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, Accounts);
        }
        public void SelectAll()
        {
         
            try
            {
                Accounts = databaseContext.Accounts.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }

        private Accounts CreateEntity()
        {
            return CreateEntity(Information);
        }
        
        private Accounts CreateEntity(AccountInfo info)
        {
           
            try
            {
                Account = new Accounts();
                Account.Id = info.ID;
                Account.Login = info.Login;
                Account.Password = info.Password;
                Account.Type = info.Type;
            }
            catch(Exception)
           {
               return null;
           }
            return Account;
        }
 
        public void Insert()
        {
            try
           {
                Account = CreateEntity();
                databaseContext.Accounts.Add(Account);
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
            Information = new AccountInfo(ID, "1", "1", 0);
        }
        public Accounts SelectItemByPrimaryKey()
        {
            
            try
            {
                Accounts Accounts = databaseContext.Accounts.Single(item => (item.Id == Information.ID));
                return Accounts;
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
            Accounts Accounts = SelectItemByPrimaryKey();
            try
            {
                   if(Accounts == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.Accounts.Remove(Accounts);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(AccountInfo oldInfo,AccountInfo newInfo)
        {

            try
            {
                Information = oldInfo;
                Accounts oldAccounts = SelectItemByPrimaryKey();
                databaseContext.Accounts.Remove(oldAccounts);
                Information = newInfo;
                Accounts newAccounts = CreateEntity();
                databaseContext.Accounts.Add(newAccounts);
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

           
         
        }
    }
}
