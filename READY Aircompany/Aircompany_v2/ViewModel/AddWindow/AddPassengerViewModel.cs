using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Airport.ViewModel
{
    public class AddPassengerViewModel : INotifyPropertyChanged
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

        #region Variable Declaration

        private string passportID;
        private string surname;
        private string name;
        private string familyName;
        private string sexIndex;
        private DateTime birthDate;
        private string birthDateInBox;
        private string sex;
        private string[] SexWithIndex = new string[2] { "male", "female" };
        AirportEntities databaseContext;
        PassengerLogic logic;

        #endregion

        #region Properties
        public string PassportID
        {
            get { return passportID; }
            set
            {
                passportID = value;
                OnPropertyChanged("PassportID");
            }
        }
        public string Surname
        {
            get { return surname; }
            set 
            { 
                surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string FamilyName
        {
            get { return familyName; }
            set
            {
                familyName = value;
                OnPropertyChanged("FamilyName");
            }
        }
        public string BirthDateInBox
        {
            get { return birthDateInBox; }
            set
            {
                birthDateInBox = value;
                OnPropertyChanged("BirthDateInBox");
                BirthDate = DateTime.Parse(birthDateInBox);
            }
        }
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }
        public string SelectedSexIndex
        {
            get { return sexIndex; }
            set
            {
                sexIndex = value;
                OnPropertyChanged("SelectedSexIndex");
                Sex = SexWithIndex[Int32.Parse(sexIndex)];
            }
        }
        public string Sex 
        {
            get { return sex; }
            set { sex = value; }
        }
        #endregion

        #region Command Declaration
        public ICommand OnClickOK { get; set; }
        public ICommand OnClickExit { get; set; }
        public ICommand CheckDigits { get; set; }
        public ICommand CheckText { get; set; }
        #endregion

        public AddPassengerViewModel()
        {
            OnClickOK = new Command(arg => ButtonOkClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
            CheckText = new Command(arg => Check.OnlyText(arg));
            databaseContext = new AirportEntities();
            logic = new PassengerLogic();
        }
        private void Validation()
        {
            Check.AllStringInput(PassportID.ToString(), Name, Surname, FamilyName, Sex, BirthDate.ToString());
        }

        private void ButtonOkClicked(object window)
        {
            try
            {
                Validation();
                ButtonExitClicked(window);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            logic.Information = new PassengerInfo(Int32.Parse(PassportID), Surname, Name, FamilyName, BirthDate, Sex);
            logic.Insert();
            
        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }

    }
}
