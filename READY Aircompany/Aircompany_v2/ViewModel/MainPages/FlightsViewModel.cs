using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airport;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Airport.ViewModel
{
    public class FlightsViewModel: INotifyPropertyChanged
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


        #region Variable declaration
        private TimeSpan? timeDeparture;
        private string timeDepartureInTextBox;
        private DateTime? dateDestination;
        private DateTime? dateDeparture;
        private int status;
        private string[] StatusItems = new string[5] { "Задержан", "Отправлен", "Завершен", "Отменен", "Не активен" };        
        private Selector selectedItem;
        private bool isButtonSaveChangesEnabled;
        private bool isTimeDepartureEnabled;
        private bool isStatusComboBoxEnabled;
        private bool isDateDepartureEnabled;
        private bool isDateDestinationEnabled;        
        private bool isButtonDeleteFlightEnabled;
        private bool isButtonAddFlightEnabled;
        FlightInfo selectedFlight;
        FlightLogic flightLogic;
        StatusFlightLogic statusFlightLogic;
        #endregion

        #region Properties
        public static ObservableAirportCollection<FlightInfo> CurrentFlights { get; set; }
        public static ObservableAirportCollection<FlightStatusInfo> CurrentFlightStatus { get; set; }
        public string GetStatus
        {
            get
            {
                for (int i = 0; i < StatusItems.Length; i++)
                {
                    if (i == Status)
                        return StatusItems[i];
                }
                return null;
            }            
        }
        public int Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        public DateTime? DateDeparture
        {
            get { return dateDeparture; }
            set 
            { 
                dateDeparture = value;
                OnPropertyChanged("DateDeparture");
            }
        }
        public DateTime? DateDestination
        {
            get { return dateDestination; }
            set
            {
                dateDestination = value;
                OnPropertyChanged("DateDestination");
            }
        }
        public string TimeDepartureInTextBox
        {
            get { return timeDepartureInTextBox; }
            set
            {
                timeDepartureInTextBox = value;
                OnPropertyChanged("TimeDepartureInTextBox");
            }
        }
        public TimeSpan? TimeDeparute
        {
            get { return timeDeparture; }
            set { timeDeparture = value; }
        }               
        public Selector SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }        
        public bool IsButtonDeleteFlightEnabled
        {
            get { return isButtonDeleteFlightEnabled; }
            set
            {
                isButtonDeleteFlightEnabled = value;
                OnPropertyChanged("IsButtonDeleteFlightEnabled");
            }
        }
        public bool IsButtonAddFlightEnabled
        {
            get { return isButtonAddFlightEnabled; }
            set
            {
                isButtonAddFlightEnabled = value;
                OnPropertyChanged("IsButtonAddFlightEnabled");
            }
        }
        public bool IsButtonSaveChangesEnabled
        {
            get { return isButtonSaveChangesEnabled; }
            set
            {
                isButtonSaveChangesEnabled = value;
                OnPropertyChanged("IsButtonSaveChangesEnabled");
            }
        }
        public bool IsTimeDepartureEnabled
        {
            get { return isTimeDepartureEnabled; }
            set
            {
                isTimeDepartureEnabled = value;
                OnPropertyChanged("IsTimeDepartureEnabled");
            }
        }
        public bool IsDateDepartureEnabled
        {
            get { return isDateDepartureEnabled; }
            set
            {
                isDateDepartureEnabled = value;
                OnPropertyChanged("IsDateDepartureEnabled");
            }
        }
        public bool IsDateDestinationEnabled
        {
            get { return isDateDestinationEnabled; }
            set
            {
                isDateDestinationEnabled = value;
                OnPropertyChanged("IsDateDestinationEnabled");
            }
        }
        public bool IsStatusComboBoxEnabled
        {
            get { return isStatusComboBoxEnabled; }
            set
            {
                isStatusComboBoxEnabled = value;
                OnPropertyChanged("IsStatusComboBoxEnabled");
            }
        }

        #endregion

        #region Command Declaration
        public ICommand OnClickAddFlight { get; set; }
        public ICommand OnClickDeleteFlight { get; set; }
        public ICommand OnClickSearchFlight { get; set; }
        public ICommand OnClickSaveChangesInStatus { get; set; }
        public ICommand ChangeItemText { get; set; }
        public ICommand PutTime { get; set; }
        public ICommand OnSelectionChangedInListFlights { get; set; }
        public ICommand OnClickBack { get; set; }
        #endregion

        public FlightsViewModel()
        {
            OnClickAddFlight = new Command(arg => AddFlightClicked());
            OnClickDeleteFlight = new Command(arg => DeleteFlightClicked(arg));
            OnClickSaveChangesInStatus = new Command(arg => SaveChangesInStatusClicked());
            OnClickSearchFlight = new Command(arg => SearchFlightClicked());
            ChangeItemText = new Command(arg => Check.OnlyDigits(arg));
            PutTime = new Command(arg => Check.PutTimeHanlder(arg));
            OnSelectionChangedInListFlights = new Command(arg => SelectionChangedInListFlights(arg));
            LogicPartsKeeper logicParts = LogicPartsKeeper.getInstance();
            flightLogic = logicParts.flightLogic;
            statusFlightLogic = logicParts.statusFlightLogic;
            CurrentFlights = new ObservableAirportCollection<FlightInfo>();
            CurrentFlightStatus = new ObservableAirportCollection<FlightStatusInfo>();
            Status = -1;
            OnClickBack = new Command(arg => ButtonBackClicked());
        
            CloseEnableToStatusFlight();
            IsButtonDeleteFlightEnabled = false;
            RefreshFlightsList();
            RefreshFlightStatusList();

            if (MainViewModel.Admin)
            {
                OpenAccess();
            }
            else CloseAccess();
        }

        private void OpenAccess()
        {
            IsButtonAddFlightEnabled = true;            
        }

        private void CloseAccess()
        {
            IsButtonDeleteFlightEnabled = false;
            IsButtonAddFlightEnabled = false;
            CloseEnableToStatusFlight();
        }

        public void ButtonBackClicked()
        {
            RefreshFlightsList();
            RefreshFlightStatusList();
        }
        private void GetDefaultStatus()
        {
            Status = -1;
            DateDeparture = null;
            DateDestination = null;
            TimeDepartureInTextBox = "";
        }


        private void CloseEnableToStatusFlight()
        {
            IsButtonSaveChangesEnabled = false;
            IsTimeDepartureEnabled = false;
            IsDateDepartureEnabled = false;
            IsDateDestinationEnabled = false;
            IsStatusComboBoxEnabled = false;
        }

        private void OpenEnableToStatusFlight()
        {
            IsButtonSaveChangesEnabled = true;
            IsTimeDepartureEnabled = true;
            IsDateDepartureEnabled = true;
            IsDateDestinationEnabled = true;
            IsStatusComboBoxEnabled = true;
        }

        

  

        private void AddFlightClicked()
        {
            AddFlight addFlight = new AddFlight();
            addFlight.ShowDialog();
            RefreshFlightsList();
            
        }

        private void DeleteFlightClicked(object selectedValue)
        {
            FlightInfo selectedFlight;
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить выбранный рейс?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                selectedFlight = selectedValue as FlightInfo;

            }
            else return;
            flightLogic.Information = selectedFlight;
            flightLogic.DeleteByPrimaryKey();
            RefreshFlightsList();
            RefreshFlightStatusList();

        }

        private void SearchFlightClicked()
        {
            SearchFlight searchFlight = new SearchFlight();
            searchFlight.ShowDialog();
        }

        private void Validation()
        {
            timeDeparture = Check.GetTime(TimeDepartureInTextBox);
            Check.AllStringInput(Status.ToString());
            Check.CorrectDate(DateDeparture, DateDestination);
        }                 

        private void SaveChangesInStatusClicked()
        {
            try
            {
                Validation();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            statusFlightLogic.Information = new FlightStatusInfo(selectedFlight.FlightID, GetStatus, DateDeparture, DateDestination, TimeDeparute);
            statusFlightLogic.Edit(new FlightStatusInfo(selectedFlight.FlightID.Value,"",DateTime.Now,DateTime.Now,new TimeSpan(10,20,30)),statusFlightLogic.Information);
            RefreshFlightStatusList();

        }

        private void SelectionChangedInListFlights(object selectedValue)
        {
            bool IsStatusExicts = false;
            selectedFlight = selectedValue as FlightInfo;
            if (MainViewModel.Admin)
            {
                OpenEnableToStatusFlight();
                IsButtonDeleteFlightEnabled = true;
            }
            else CloseAccess();
            
            if (selectedFlight == null) return;
            foreach (FlightStatusInfo status in CurrentFlightStatus)
            {
                if (selectedFlight.FlightID == status.FlightID)
                {
                    IsStatusExicts = true;
                    TimeDepartureInTextBox = status.TimeDeparture.Value.Hours.ToString() + ":" + status.TimeDeparture.Value.Minutes.ToString();
                    DateDeparture = status.DateDeparture;
                    DateDestination = status.DateDestination;
                    if (status.FlightStatus == null)
                    {
                        Status = -1;
                    }
                    for (int i = 0; i < StatusItems.Length; i++)
                    {
                        if (status.FlightStatus == StatusItems[i]) Status = i;
                    }
                }

            }
            if (!IsStatusExicts)
            {
                GetDefaultStatus();
            }
        }

        private void RefreshFlightsList()
        {

            CurrentFlights.Clear();
            flightLogic.SelectAll();

           
            flightLogic.flights.ForEach(flight => CurrentFlights.Add(new FlightInfo(
                    flight.ID_flight,
                    flight.ID_airplane,
                    flight.city_departure,
                    flight.city_destination,
                    flight.airport_departure,
                    flight.airport_destination,
                    flight.terminal_departure,
                    flight.terminal_destination,
                    flight.gates_departure,
                    flight.gates_destination,
                    flight.time_flight,
                    flight.miles_count)));
        }
        private void RefreshFlightStatusList()
        {
            statusFlightLogic.SelectAll();
            CurrentFlightStatus.Clear();
            statusFlightLogic.StatusFlights.ForEach(status => CurrentFlightStatus.Add(new FlightStatusInfo(
                 status.ID_flight,
                 status.status_flight,
                 status.date_departure.Value,
                 status.date_destination.Value,
                 status.time_departure.Value)));
        }


       

    }
}
