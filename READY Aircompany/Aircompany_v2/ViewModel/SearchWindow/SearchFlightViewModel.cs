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
    public class SearchFlightViewModel : INotifyPropertyChanged
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
        private string cityDeparture;
        private string cityDestination;
        private string airportDeparture;
        private string airportDestination;
        private string terminalDeparture;
        private string terminalDestination;
        private string gateDeparture;
        private string gateDestination;
        private string milesCount;
        private DateTime? dateDeparture;
        private DateTime? dateDestination;
        private string timeInWayInTextBox;
        private string timeDepartureInTextBox;
        private TimeSpan? timeDeparture;
        private TimeSpan? timeInWay;
        private string selectedAirplane;
        private string selectedAirplanesTypes;
        private string flightNumber;
        List<FlightInfo> found;
        List<FlightStatusInfo> foundStatusInfo;
        FlightSearchContext searcher;
        public List<int?> currentAirplanesID { get; set; }
        public List<string> currentAirplanesTypes { get; set; }
        #endregion

        #region Properties
        public string FlightNumber
        {
            get { return flightNumber; }
            set
            {
                flightNumber = value.ToString();
                OnPropertyChanged("FlightNumber");
            }
        }
        public string CityDeparture
        {
            get { return cityDeparture; }
            set
            {
                cityDeparture = value;
                OnPropertyChanged("CityDeparture");
            }
        }
        public string CityDestination
        {
            get { return cityDestination; }
            set
            {
                cityDestination = value;
                OnPropertyChanged("CityDestination");
            }
        }
        public string AirportDeparture
        {
            get { return airportDeparture; }
            set
            {
                airportDeparture = value;
                OnPropertyChanged("AirportDeparture");
            }
        }
        public string AirportDestination
        {
            get { return airportDestination; }
            set
            {
                airportDestination = value;
                OnPropertyChanged("AirportDestination");
            }
        }
        public string TerminalDeparture
        {
            get { return terminalDeparture; }
            set
            {
                terminalDeparture = value;
                OnPropertyChanged("TerminalDeparture");
            }
        }
        public string TerminalDestination
        {
            get { return terminalDestination; }
            set
            {
                terminalDestination = value;
                OnPropertyChanged("TerminalDestination");
            }
        }
        public string GateDeparture
        {
            get { return gateDeparture; }
            set
            {
                gateDeparture = value;
                OnPropertyChanged("GateDeparture");
            }
        }
        public string GateDestination
        {
            get { return gateDestination; }
            set
            {
                gateDestination = value;
                OnPropertyChanged("GateDestination");
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
        
        public string MilesCount
        {
            get { return milesCount; }
            set
            {
                milesCount = value;
                OnPropertyChanged("MilesCount");
            }
        }
        public string TimeInWayInTextBox
        {
            get { return timeInWayInTextBox; }
            set
            {
                timeInWayInTextBox = value;
                OnPropertyChanged("TimeInWayInTextBox");
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
        public string SelectedAirplane
        {
            get { return selectedAirplane; }
            set
            {
                selectedAirplane = value;
                OnPropertyChanged("SelectedAirplane");
                foreach (AirplanesWithTypes plane in App.AirplanesWithTypes)
                {
                    if (plane.ID_airplane == Int32.Parse(selectedAirplane))
                        SelectedAirplanesType = plane.name;
                }
            }
        }
        public string SelectedAirplanesType
        {
            get { return selectedAirplanesTypes; }
            set
            {
                selectedAirplanesTypes = value;
                OnPropertyChanged("SelectedAirplanesType");
                currentAirplanesID = GetAirplanesID(App.AirplanesWithTypes, selectedAirplanesTypes);
            }
        }
        public TimeSpan? TimeDeparture
        {
            get { return timeDeparture; }
            set { timeDeparture = value; }
        }
        public TimeSpan? TimeInWay
        {
            get { return timeInWay; }
            set { timeInWay = value; }
        }
        #endregion

        #region GetNamesFromList
        private List<int?> GetAirplanesID(List<AirplanesWithTypes> list)
        {
            List<int?> airplanesID = new List<int?>();
            foreach (AirplanesWithTypes plane in list)
            {
                airplanesID.Add(plane.ID_airplane);
            }
            return airplanesID;
        }
        private List<string> GetAirplanesTypes(List<AirplanesWithTypes> list)
        {
            List<string> airplanesTypes = new List<string>();
            foreach (AirplanesWithTypes type in list)
            {
                airplanesTypes.Add(type.name);
            }
            return airplanesTypes;
        }
        private List<int?> GetAirplanesID(List<AirplanesWithTypes> list, string type)
        {
            List<int?> airplanesID = new List<int?>();
            foreach (AirplanesWithTypes plane in list)
            {
                if (plane.name == type)
                    airplanesID.Add(plane.ID_airplane);
            }
            return airplanesID;
        }
        #endregion

        #region Command Declaration
        public ICommand OnClickSearch { get; set; }
        public ICommand OnClickExit { get; set; }
        public ICommand PutTime { get; set; }
        public ICommand CheckDigits { get; set; }
        public ICommand CheckText { get; set; }
        public ICommand CheckTextAndDigits { get; set; }
        #endregion

        public SearchFlightViewModel()
        {
            PutTime = new Command(arg => Check.PutTimeHanlder(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
            CheckText = new Command(arg => Check.OnlyText(arg));
            CheckTextAndDigits = new Command(arg => Check.OnlyTextAndDigits(arg));
            OnClickSearch = new Command(arg => ButtonSearchClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            List<AirplanesWithTypes> airplanesWithTypes = new List<AirplanesWithTypes>();
            AirplaneSearchContext searcherAirplane = new AirplaneSearchContext();
            airplanesWithTypes = searcherAirplane.getAirplanesJoinTypes(this, EventArgs.Empty);
            currentAirplanesID = GetAirplanesID(airplanesWithTypes);
            currentAirplanesTypes = GetAirplanesTypes(airplanesWithTypes);
            searcher = new FlightSearchContext();
            List<FlightInfo> found = new List<FlightInfo>();
            List<FlightStatusInfo> foundStatusInfo = new List<FlightStatusInfo>();
        }

         private void TryParseTime()
         {
             if (!Check.IsTimeNull(TimeDepartureInTextBox))
                 TimeDeparture = Check.GetTime(TimeDepartureInTextBox);
             else TimeDeparture = null;
             if (!Check.IsTimeNull(TimeInWayInTextBox))
                 TimeInWay = Check.GetTime(TimeInWayInTextBox);
             else TimeInWay = null;
         }

         private void Validation()
         {
             TryParseTime();
             Check.IfOneStringInput(CityDeparture, CityDestination, AirportDeparture, AirportDestination, TerminalDeparture, 
                 TerminalDestination, GateDeparture, GateDestination, TimeDepartureInTextBox, TimeInWayInTextBox, FlightNumber, 
                 DateDestination.ToString(), DateDeparture.ToString(),
                 SelectedAirplane, SelectedAirplanesType);
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
                return;
            }
            FlightInfo infoToFound = createInfoToSearch();
            List<FlightInfo> result = new List<FlightInfo>();
         
            found = searcher.Search(infoToFound);
            foundStatusInfo = searcher.SearchByStatusInfo(new FlightStatusInfo(infoToFound.FlightID, "", DateDeparture, DateDestination, TimeDeparture));
            if (!isAllStatusFieldsAreEmpty() && isAllFlightInfoFieldsAreEmpty(infoToFound))
            {
                result.Clear();
                result = getInfoFromStatus();
            }
            else if (isAllStatusFieldsAreEmpty() && !isAllFlightInfoFieldsAreEmpty(infoToFound))
            {
                result.Clear();
                result = found;
            }
            else
            {
                result = JoinFoundByStatusAndFlightInfo(found, foundStatusInfo);
            }
            
            if (result.Count == 0)
            {
                MessageBox.Show("Данный рейс в базе не существует! ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (FlightsViewModel.CurrentFlights != null)
                {
                    FlightsViewModel.CurrentFlights.Clear();
                    result.ForEach(flight => FlightsViewModel.CurrentFlights.Add(flight));
                }
                if (TicketsViewModel.CurrentFlights != null)
                {
                    TicketsViewModel.CurrentFlights.Clear();
                    result.ForEach(flight => TicketsViewModel.CurrentFlights.Add(flight));
                }
            }
        }
        private List<FlightInfo> getInfoFromStatus()
        {
            List<FlightInfo> result = new List<FlightInfo>();
            foreach (FlightStatusInfo status in foundStatusInfo)
            {
                foreach (FlightInfo flight in FlightsViewModel.CurrentFlights)
                {
                    if (flight.FlightID == status.FlightID)
                    {
                        result.Add(flight);
                    }
                }
            }
            return result;
        }
        private FlightInfo createInfoToSearch()
        {
            int? selectedAirplaneId;
            int? milesCountInWayNullable;
            int? flightIdNullable;

            try
            { selectedAirplaneId = Int32.Parse(selectedAirplane); }
            catch { selectedAirplaneId = null; }
            try
            { milesCountInWayNullable = Int32.Parse(milesCount); }
            catch
            { milesCountInWayNullable = null; }
            try { flightIdNullable = Int32.Parse(flightNumber); }
            catch { flightIdNullable = null; }

            FlightInfo infoToFound = new FlightInfo(flightIdNullable, selectedAirplaneId,
            cityDeparture,
            cityDestination,
            airportDeparture,
            airportDestination,
            terminalDeparture,
            terminalDestination,
            gateDeparture,
            gateDestination,
            timeInWay,
            milesCountInWayNullable
                        );

            return infoToFound;
        }
        private bool isAllFlightInfoFieldsAreEmpty(FlightInfo info)
        {
            FlightSearchContext searcher = new FlightSearchContext();
           int count = searcher.getCountOfNotEmptyFieldsInSearchRequest(info);
           if (count == 0) return true;
           else return false;
        }
        private bool isAllStatusFieldsAreEmpty()
        {
           const int countOfFields = 5;
            int current = 1;//по значению статуса искать все равно нельзя, поэтому сразу 1
            try{
                Int32.Parse(flightNumber);
            }
            catch
            {
                current++;
            }
            if (dateDeparture == null) current++;
            if (dateDestination == null) current++;
            if (timeDeparture == null) current++;
            if (current == countOfFields) return true;
            else return false;
            
        }
        public List<FlightInfo> JoinFoundByStatusAndFlightInfo(List<FlightInfo> flightInfoList,List<FlightStatusInfo> statusInfoList)
        {
            List<FlightInfo> result = new List<FlightInfo>();
            foreach(FlightInfo flightInfo in flightInfoList)
            {
                foreach(FlightStatusInfo statusInfo in statusInfoList)
                {
                    if (flightInfo.FlightID == statusInfo.FlightID)
                        result.Add(flightInfo);
                }
            }
            return result;
        }
        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }


    }
}
