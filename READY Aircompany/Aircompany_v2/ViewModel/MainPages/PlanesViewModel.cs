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
    public class PlanesViewModel : INotifyPropertyChanged
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
        public static ObservableAirportCollection<AirplaneTypeInfo> CurrentAirplanesTypes { get; set; }
        public static ObservableAirportCollection<AirplaneInfo> CurrentAirplanes { get; set; }
        private AirplaneLogic airplaneLogic;
        private AirplaneTypeLogic airplaneTypeLogic;
        AirportEntities databaseContext;
        private List<Airplane> airplanesList;
        private List<AirplaneType> airplaneTypeList;
        PlanesViewModel instance;
        private object selectedItemInListPlaneTypes;
        
        static private int countSelectedItemsInListAirplanesTypes { get; set; }
        static private int countSelectedItemsInListAirplanes { get; set; }
        private bool isButtonDeleteAirplanesTypesEnabled;
        private bool isButtonDeleteAirplanesEnabled;
        private bool isButtonDeleteEnabled;
        private bool isButtonChangeTypeEnabled;
        private bool isButtonAddTypeEnabled;
        private bool isButtonAddAirplaneEnabled;
        private bool isButtonAddEnabled;
        #endregion

        #region Properties

        public bool IsButtonAddTypeEnabled
        {
            get { return isButtonAddTypeEnabled; }
            set
            {
                isButtonAddTypeEnabled = value;
                OnPropertyChanged("IsButtonAddTypeEnabled");
            }
        }
        public bool IsButtonAddAirplaneEnabled
        {
            get { return isButtonAddAirplaneEnabled; }
            set
            {
                isButtonAddAirplaneEnabled = value;
                OnPropertyChanged("IsButtonAddAirplaneEnabled");
            }
        }
        public bool IsButtonAddEnabled
        {
            get { return isButtonAddEnabled; }
            set
            {
                isButtonAddEnabled = value;
                OnPropertyChanged("IsButtonAddEnabled");
            }
        }
        public object SelectedItemInListPlaneTypes
        {
            get { return selectedItemInListPlaneTypes; }
            set
            {
                selectedItemInListPlaneTypes = value;
                OnPropertyChanged("SelectedItemInListPlaneTypes");
            }
        }
        public bool IsButtonDeleteAirplanesTypesEnabled
        {
            get { return isButtonDeleteAirplanesTypesEnabled; }
            set
            {
                isButtonDeleteAirplanesTypesEnabled = value;
                OnPropertyChanged("IsButtonDeleteAirplanesTypesEnabled");
            }
        }
        public bool IsButtonDeleteAirplanesEnabled
        {
            get { return isButtonDeleteAirplanesEnabled; }
            set
            {
                isButtonDeleteAirplanesEnabled = value;
                OnPropertyChanged("IsButtonDeleteAirplanesEnabled");
            }
        }
        public bool IsButtonDeleteEnabled
        {
            get { return isButtonDeleteEnabled; }
            set
            {
                isButtonDeleteEnabled = value;
                OnPropertyChanged("IsButtonDeleteEnabled");
            }
        }
        public bool IsButtonChangeTypeEnabled
        {
            get { return isButtonChangeTypeEnabled; }
            set
            {
                isButtonChangeTypeEnabled = value;
                OnPropertyChanged("IsButtonChangeTypeEnabled");
            }
        }
        #endregion

        #region Command Declaration
        public ICommand OnSelectionChangedInListPlaneTypes { get; set; }
        public ICommand OnSelectionChangedInListPlanes { get; set; }
        public ICommand OnClickAddType { get; set; }
        public ICommand OnClickAddAirplane { get; set; }
        public ICommand OnClickDeleteType { get; set; }
        public ICommand OnClickDeleteAirplane { get; set; }
        public ICommand OnClickSearchType { get; set; }
        public ICommand OnClickBack { get; set; }
        #endregion
         
        
        public PlanesViewModel()
        {
            
            OnSelectionChangedInListPlanes = new Command(arg => SelectionChangedInListPlanes(arg));
            OnSelectionChangedInListPlaneTypes = new Command(arg => SelectionChangedInListPlaneTypes(arg));
            OnClickAddAirplane = new Command(arg => AddPlaneClicked());
            OnClickAddType = new Command(arg => AddTypeClicked());
            OnClickDeleteAirplane = new Command(arg => DeletePlaneCLicked(arg));
            OnClickDeleteType = new Command(arg => DeleteTypeClicked(arg));
            OnClickSearchType = new Command(arg => SearchTypeClicked());
            OnClickBack = new Command(arg => ButtonBackClicked());

            CurrentAirplanes = new ObservableAirportCollection<AirplaneInfo>();
            CurrentAirplanesTypes = new ObservableAirportCollection<AirplaneTypeInfo>();

            countSelectedItemsInListAirplanes = 0;
            countSelectedItemsInListAirplanesTypes = 0;
            databaseContext = new AirportEntities();
            airplaneLogic = new AirplaneLogic();
            airplanesList = new List<Airplane>();
            airplaneTypeList = new List<AirplaneType>();
            airplaneTypeLogic = new AirplaneTypeLogic();
            RefreshAirplaneList();
            RefreshAirplaneTypeList();
            if (!MainViewModel.Admin) CloseAccess();
            else  CheckButtons();
            
           
        }

        private void CloseAccess()
        {
            IsButtonAddAirplaneEnabled = false;
            IsButtonAddTypeEnabled = false;
            IsButtonDeleteAirplanesEnabled = false;
            IsButtonDeleteAirplanesTypesEnabled = false;
            IsButtonAddEnabled = false;

        }

        private void CheckButtons()
        {
            IsButtonAddAirplaneEnabled = true;
            IsButtonAddEnabled = true;
            IsButtonAddTypeEnabled = true;
            if ((countSelectedItemsInListAirplanes == 0) && (countSelectedItemsInListAirplanesTypes == 0)) IsButtonDeleteEnabled = false;
            else if ((countSelectedItemsInListAirplanes > 0) || (countSelectedItemsInListAirplanesTypes > 0)) IsButtonDeleteEnabled = true;
            if (countSelectedItemsInListAirplanesTypes == 1) IsButtonChangeTypeEnabled = true;
            else IsButtonChangeTypeEnabled = false;
            if (countSelectedItemsInListAirplanesTypes >= 1) IsButtonDeleteAirplanesTypesEnabled = true;
            else  IsButtonDeleteAirplanesTypesEnabled = false;
            if (countSelectedItemsInListAirplanes >= 1) IsButtonDeleteAirplanesEnabled = true;
            else IsButtonDeleteAirplanesEnabled = false;
        }

        private void ButtonBackClicked()
        {
            RefreshAirplaneList();
            RefreshAirplaneTypeList();
        }
        private void SelectionChangedInListPlaneTypes(object selectedItems)
        {
            System.Collections.IList selectedTypes = (System.Collections.IList)selectedItems;
            ObservableAirportCollection<AirplaneInfo> selectedPlanes = new ObservableAirportCollection<AirplaneInfo>();

            countSelectedItemsInListAirplanesTypes = selectedTypes.Count;

            foreach (AirplaneTypeInfo type in selectedTypes)
            {
                if (type != null)
                {
                    foreach (Airplane plane in airplanesList)
                    {
                        if (plane.airplane_type.Value == type.ID)
                        {
                            selectedPlanes.Add(new AirplaneInfo(plane.ID_airplane,plane.airplane_type.Value));                            
                        }
                    }
                    CurrentAirplanes.Clear();
                    CurrentAirplanes.ReplaceRange(selectedPlanes);
                    
                }
            }
            if (!MainViewModel.Admin) CloseAccess();
            else CheckButtons();
        }

        private void SelectionChangedInListPlanes(object selectedItems)
        {
            System.Collections.IList selectedPlanes = (System.Collections.IList)selectedItems;
            countSelectedItemsInListAirplanes = selectedPlanes.Count;
            if (!MainViewModel.Admin) CloseAccess();
            else CheckButtons();
        }

        private void AddTypeClicked()
        {
            AddPlaneType addPlaneType = new AddPlaneType();
            addPlaneType.ShowDialog();
            RefreshAirplaneTypeList();
            RefreshAirplaneList();
            
        }

        private void AddPlaneClicked()
        {
            AddPlane addPlane = new AddPlane();
            addPlane.ShowDialog();
            RefreshAirplaneList();
            RefreshAirplaneTypeList();
        }

        private void DeleteTypeClicked(object selectedItems)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить выбранные типа самолетов?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            System.Collections.IList selectedTypesToDelete;
            List<AirplaneTypeInfo> ImplementedListOfPlanesTypesToDelete = new List<AirplaneTypeInfo>();
            if (result == MessageBoxResult.Yes)
            {
                selectedTypesToDelete = (System.Collections.IList)selectedItems;
                AirplaneTypeInfo[] array = new AirplaneTypeInfo[selectedTypesToDelete.Count];
                selectedTypesToDelete.CopyTo(array, 0);
                ImplementedListOfPlanesTypesToDelete.AddRange(array);
            }
            else return;
            airplaneTypeLogic.DeleteRange(ImplementedListOfPlanesTypesToDelete);
            RefreshAirplaneTypeList();
            RefreshAirplaneList();

        }

        private void DeletePlaneCLicked(object selectedItems)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить выбранные самолеты?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            System.Collections.IList selectedPlanesToDelete;
            List<AirplaneInfo> ImplementedListOfPlanesToDelete = new List<AirplaneInfo>();
            
            if (result == MessageBoxResult.Yes)
            {
                selectedPlanesToDelete = (System.Collections.IList)selectedItems;
                AirplaneInfo[] array = new AirplaneInfo[selectedPlanesToDelete.Count];
                selectedPlanesToDelete.CopyTo(array, 0);
                ImplementedListOfPlanesToDelete.AddRange(array);

            }
            else return;
       
    
            airplaneLogic.DeleteRange(ImplementedListOfPlanesToDelete);
            RefreshAirplaneList();
        }
        private void SearchTypeClicked()
        {
            SearchType searchType = new SearchType();
            searchType.ShowDialog();
            SelectionChangedInListPlaneTypes(CurrentAirplanesTypes);
        }
 
        private void RefreshAirplaneList()
        {
            airplaneLogic.SelectAll();
             CurrentAirplanes.Clear();
             airplanesList.Clear();
            airplanesList = airplaneLogic.airplanes;
            airplanesList.ForEach(plane => CurrentAirplanes.Add(new AirplaneInfo(plane.ID_airplane, plane.airplane_type.Value)));
        }
        private void RefreshAirplaneTypeList()
        {
            airplaneTypeLogic.SelectAll();

            CurrentAirplanesTypes.Clear();
            airplaneTypeList.Clear();

            airplaneTypeList = airplaneTypeLogic.airplaneTypeList;
            airplaneTypeList.ForEach(type => CurrentAirplanesTypes.Add(new AirplaneTypeInfo(
                type.airplane_type,
                type.name_, 
                type.carrying,
                type.crew_count,
                type.business_seats_count,
                type.econom_seats_count)
                )
             );

        }


    }
}
