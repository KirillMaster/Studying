using Airport.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Airport.ViewModel
{
    public class EntryViewModel : INotifyPropertyChanged
    {
        #region Implement INotyfyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private string loadingVisibility;
        private string login;
        static public bool Admin { get; private set; }

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public string LoadingVisibility
        {
            get { return loadingVisibility; }
            set
            {
                loadingVisibility = value;
                OnPropertyChanged("LoadingVisibility");
            }
        }


        public ICommand OnClickEntry { get; set; }

        public EntryViewModel()
        {
            OnClickEntry = new Command(arg => ButtonEntryClicked(arg));
            LoadingVisibility = "0";
        }


        private void ButtonEntryClicked(object pBox)
        {
            //LoadingVisibility = "1";
            var passwordBox = pBox as PasswordBox;
            var password = passwordBox.Password;
            AirportEntities databaseContext = new AirportEntities();
            Accounts acc = null;
            try
            {
                acc = databaseContext.Accounts.First(item => (item.Login.Trim().Equals(Login.Trim())) && (item.Password.Trim().Equals(password.Trim())));
            }
            catch
            {
                
            }
               
            if(acc == null)
            {
                MessageBox.Show("Неправильный логин или пароль");
                return;
            }
            else if(acc.Type == 0)//admin
            {
                MainViewModel.Admin = true;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = mainWindow; 
            }
            else if(acc.Type == 1)//user
            {
                MainViewModel.Admin = false;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = mainWindow; 
            }
        }
    
    
    }

    
}
