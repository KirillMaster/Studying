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
    public class AddPlaneViewModel : INotifyPropertyChanged
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
        private int planesCount;
        private string planesType;
        private List<string> currentAirplaneTypes;
        
        #endregion

        #region Properties
        public List<string> CurrentAirplanesTypes
        {
            get { return currentAirplaneTypes; }
            set
            {
                currentAirplaneTypes = value;
                OnPropertyChanged("CurrentAirplanesTypes");
            }
        }
        public int PlanesCount
        {
            get { return planesCount; }
            set
            {
                planesCount = value;
                OnPropertyChanged("PlanesCount");
            }
        }
        public string SelectedAirplanesType
        {
            get { return planesType; }
            set
            {
                planesType = value;
                OnPropertyChanged("SelectedAirplanesType");
            }
        }
        #endregion

        #region Command Declaration
        public ICommand OnClickAdd { get; set; }
        public ICommand OnClickExit { get; set; }
        public ICommand CheckDigits { get; set; }
        #endregion

        public AddPlaneViewModel()
        {
            OnClickAdd = new Command(arg => ButtonAddClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));

            CurrentAirplanesTypes = GetTypesNamesFromList(PlanesViewModel.CurrentAirplanesTypes.ToList());
        }

        private List<string> GetTypesNamesFromList(List<AirplaneTypeInfo> list)
        {
            List<string> typesNames = new List<string>();
            foreach (AirplaneTypeInfo type in list)
            {
                typesNames.Add(type.TypeName);
            }
            return typesNames;
        }

        private void Validation()
        {
            Check.AllStringInput(PlanesCount.ToString(), SelectedAirplanesType);
        }

        private void ButtonAddClicked(object window)
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
            
            LogicPartsKeeper logicParts = LogicPartsKeeper.getInstance();
            AirplaneLogic airplaneLogic = logicParts.airplaneLogic;
            List<AirplaneTypeInfo> types  = new List<AirplaneTypeInfo>();
            AirplaneSearchContext searcher = new AirplaneSearchContext();
            for(int i = 0;i<PlanesCount;i++)
            {
                types = searcher.AirplaneTypeSearchByTypeName(planesType);
                airplaneLogic.Information = new AirplaneInfo(0,types[0].ID.Value);
                airplaneLogic.Insert();
            }
            
        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }


    }
}
