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
    public class SearchTypeViewModel : INotifyPropertyChanged
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

        #region Variable Declaration and Properties
        private string airplaneType;
        public string AirplaneType
        {
            get { return airplaneType; }
            set
            {
                airplaneType = value;
                OnPropertyChanged("AirplaneType");
            }
        }
        #endregion

        #region Command Declaration
        public ICommand OnClickSearch { get; set; }
        public ICommand OnClickExit { get; set; }
        public ICommand CheckTextAndDigits { get; set; }
        #endregion

        public SearchTypeViewModel()
        {
            OnClickSearch = new Command(arg => ButtonSearchClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            CheckTextAndDigits = new Command(arg => Check.OnlyTextAndDigits(arg));
        }
        private void Validation()
        {
            Check.IfOneStringInput(AirplaneType);
        }

        private void ButtonSearchClicked(object window)
        {
            try
            {
                Validation();
                ButtonExitClicked(window);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            AirplaneSearchContext searcher = new AirplaneSearchContext();
            List<AirplaneTypeInfo> found = searcher.AirplaneTypeSearchByTypeName(AirplaneType);
            if (found.Count == 0)
            {
                MessageBox.Show("Такого типа самолета в базе нет!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PlanesViewModel.CurrentAirplanesTypes.Clear();
                PlanesViewModel.CurrentAirplanesTypes.AddRange(found);
            }
           

        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }

    }
}
