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
    public class SearchPlaneViewModel : INotifyPropertyChanged
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
        private string airplaneID;
        private string selectedAirplanesTypes;
        public List<string> currentAirplanesTypes { get; set; }
        #endregion

        #region Properties
        public string AirplaneID
        {
            get { return airplaneID; }
            set
            {
                airplaneID = value;
                OnPropertyChanged("AirplaneID");
            }
        }
        public string SelectedAirplanesType
        {
            get { return selectedAirplanesTypes; }
            set
            {
                selectedAirplanesTypes = value;
                OnPropertyChanged("SelectedAirplanesType");
            }
        }
        #endregion

        #region GetNamesFromList
        private List<string> GetAirplanesTypes(List<AirplanesWithTypes> list)
        {
            List<string> airplanesTypes = new List<string>();
            foreach (AirplanesWithTypes type in list)
            {
                airplanesTypes.Add(type.name);
            }
            return airplanesTypes;
        }
        #endregion

        #region Command Declaration
        public ICommand OnClickSearch { get; set; }
        public ICommand OnClickExit { get; set; }
        public ICommand CheckDigits { get; set; }
        #endregion

        public SearchPlaneViewModel()
        {
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
            OnClickSearch = new Command(arg => ButtonSearchClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            currentAirplanesTypes = new List<string>();
           PlanesViewModel.CurrentAirplanesTypes.ToList().ForEach(type => currentAirplanesTypes.Add(type.TypeName));

        }

        private void Validation()
        {
            Check.IfOneStringInput(AirplaneID, SelectedAirplanesType);
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
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            AirplaneSearchContext searcher = new AirplaneSearchContext();
            PlanesViewModel.CurrentAirplanes.Clear();

            List<AirplanesIDWithTypeName> found = searcher.AirplaneSearch(new AirplanesIDWithTypeName(Int32.Parse(AirplaneID), SelectedAirplanesType));
            List<AirplaneTypeInfo> foundType = searcher.AirplaneTypeSearchByTypeName(SelectedAirplanesType);
            List<AirplaneInfo> itemsToShow = new List<AirplaneInfo>();
            AirplaneInfo info;
            foreach(AirplanesIDWithTypeName item in found)
            {
                if(foundType[0].TypeName.Equals(item.name))
                {
                    info = new AirplaneInfo(item.ID_airplane, foundType[0].ID.Value);
                    itemsToShow.Add(info);
                }
            }
            PlanesViewModel.CurrentAirplanes.AddRange(itemsToShow);
        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }




    }
}
