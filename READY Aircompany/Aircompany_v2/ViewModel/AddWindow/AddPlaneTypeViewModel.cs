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
    public class AddPlaneTypeViewModel : INotifyPropertyChanged
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
        private string typeName;
        private int carrying;
        private int crewCount;
        private int economCount;
        private int bussinesCount;
        #endregion

        #region Properties
        public string TypeName
        {
            get { return typeName; }
            set
            {
                typeName = value;
                OnPropertyChanged("TypeName");
            }
        }
        public int Carrying
        {
            get { return carrying; }
            set
            {
                carrying = value;
                OnPropertyChanged("Carrying");
            }
        }
        public int CrewCount
        {
            get { return crewCount; }
            set
            {
                crewCount = value;
                OnPropertyChanged("CrewCount");
            }
        }
        public int EconomCount
        {
            get { return economCount; }
            set
            {
                economCount = value;
                OnPropertyChanged("EconomCount");
            }
        }
        public int BussinesCount
        {
            get { return bussinesCount; }
            set
            {
                bussinesCount = value;
                OnPropertyChanged("BussinesCount");
            }
        }
        #endregion

        #region Command Declaration
        public ICommand OnClickOK { get; set; }
        public ICommand OnClickExit { get; set; }
        public ICommand CheckDigits { get; set; }
        public ICommand CheckTextAndDigits { get; set; }
        #endregion

        public AddPlaneTypeViewModel()
        {
            OnClickOK = new Command(arg => ButtinOKClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
            CheckTextAndDigits = new Command(arg => Check.OnlyTextAndDigits(arg));
        }

        private void Validation()
        {
            Check.AllStringInput(TypeName, Carrying.ToString(), CrewCount.ToString(), BussinesCount.ToString(), EconomCount.ToString());
        }

        private void ButtinOKClicked(object window)
        {
            try
            {
                Validation();
                ButtonExitClicked(window);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            AirplaneTypeLogic airplaneLogic = new AirplaneTypeLogic();
            airplaneLogic.Information = new AirplaneTypeInfo(0, TypeName, Carrying, CrewCount, BussinesCount, EconomCount);
            airplaneLogic.Insert();
        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }
    }
}
