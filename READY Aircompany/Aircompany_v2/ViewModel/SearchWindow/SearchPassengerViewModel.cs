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
    public class SearchPassengerViewModel : INotifyPropertyChanged
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
        private string birthDateInBox;
        private DateTime? birthDate;
        private string sex;
        private string[] SexWithIndex = new string[2] { "male", "female" };
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
        public DateTime? BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
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

        public SearchPassengerViewModel()
        {
            OnClickOK = new Command(arg => ButtonOkClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
            CheckText = new Command(arg => Check.OnlyText(arg));
        }
        private void Validation()
        {
            Check.IfOneStringInput(PassportID, Name, Surname, FamilyName, Sex, BirthDate.ToString());
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
            PassengerSearchContext searcher = new PassengerSearchContext();
            int passportID;
            if(!Int32.TryParse(PassportID, out passportID))
            {
                passportID = 0;
            }
            List<PassengerInfo> found = searcher.SearchPassenger(new PassengerInfo(passportID, Surname, Name, FamilyName, BirthDate, Sex));
            if (found.Count == 0)
            {
                MessageBox.Show("Такого пассажира в базе нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                PassengerViewModel.CurrentPassengers.Clear();
                found.ForEach(pas => PassengerViewModel.CurrentPassengers.Add(new PassengerInfo(pas.PassportID, pas.Surname, pas.Name, pas.FamilyName, pas.BirthDate, pas.Sex)));
    
            }
                }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }

    }
}
