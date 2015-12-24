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
    public class AddFlightViewModel : INotifyPropertyChanged
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
        private DateTime dateDeparture;
        private DateTime dateDestination;
        private string timeInWayInTextBox;
        private string timeDepartureInTextBox;
        private TimeSpan timeDeparture;
        private TimeSpan timeInWay;
        private string selectedAirplane;
        private string selectedAirplanesTypes;
        LogicPartsKeeper logicParts;
        List<AirplanesWithTypes> airplanesWithTypes;
        #endregion

        #region Properties
        public List<int?> currentAirplanesID { get; set; }
        public List<string> currentAirplanesTypes { get; set; }
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
        public DateTime DateDeparture
        {
            get { return dateDeparture; }
            set
            {
                dateDeparture = value;
                OnPropertyChanged("DateDeparture");
            }
        }
        public DateTime DateDestination
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
            get 
            {
                if (milesCount != null)
                    return milesCount;
                else return null;
            }
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
            get 
            {
                if (selectedAirplane != null)
                    return selectedAirplane;
                else return null;
            }
            set { 
                selectedAirplane = value;
                OnPropertyChanged("SelectedAirplane");
                AirplaneSearchContext searcher = new AirplaneSearchContext();
                airplanesWithTypes = searcher.getAirplanesJoinTypes(this, EventArgs.Empty); 

                foreach (AirplanesWithTypes plane in airplanesWithTypes )
                {
                    if (plane.ID_airplane == Int32.Parse(selectedAirplane))
                        SelectedAirplanesType = plane.name;
                }
                }
        }
        public string SelectedAirplanesType
        {
            get { return selectedAirplanesTypes; }
            set { 
                selectedAirplanesTypes = value;
                OnPropertyChanged("SelectedAirplanesType");
                currentAirplanesID = GetAirplanesID(airplanesWithTypes, selectedAirplanesTypes);
                
            }
        }
        public TimeSpan TimeDeparture
        {
            get { return timeDeparture; }
            set { timeDeparture = value; }
        }
        public TimeSpan TimeInWay
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
        public ICommand OnClickSave { get; set; }
        public ICommand OnClickExit { get; set; }
        public ICommand PutTime { get; set; }
        public ICommand CheckDigits { get; set; }
        public ICommand CheckText { get; set; }
        public ICommand CheckTextAndDigits { get; set; }
        #endregion

        public AddFlightViewModel()
        {
            PutTime = new Command(arg => Check.PutTimeHanlder(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
            CheckText = new Command(arg => Check.OnlyText(arg));
            CheckTextAndDigits = new Command(arg => Check.OnlyTextAndDigits(arg));
            OnClickSave = new Command(arg => ButtonSaveClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            AirplaneSearchContext searcher = new AirplaneSearchContext();
            airplanesWithTypes = searcher.getAirplanesJoinTypes(this, EventArgs.Empty);
            currentAirplanesID = GetAirplanesID(airplanesWithTypes);
            currentAirplanesTypes = GetAirplanesTypes(airplanesWithTypes);
            logicParts = LogicPartsKeeper.getInstance();
            DateDeparture = new DateTime(2016, 01, 01);
            DateDestination = new DateTime(2016, 01, 01);
            airplanesWithTypes = new List<AirplanesWithTypes>();
            
        }

        private void Validation()
        {
            if (!Check.IsTimeNull(TimeDepartureInTextBox)) 
            TimeDeparture = Check.GetTime(TimeDepartureInTextBox);
            if (!Check.IsTimeNull(TimeInWayInTextBox))
            TimeInWay = Check.GetTime(TimeInWayInTextBox);
            Check.AllStringInput(CityDeparture, CityDestination, AirportDeparture, AirportDestination,
                GateDeparture, GateDestination, TerminalDeparture, TerminalDestination,
                MilesCount, SelectedAirplane, SelectedAirplanesType, DateDeparture.ToString(),
                DateDestination.ToString(), TimeDeparture.ToString(), TimeInWay.ToString());
            Check.CorrectDate(DateDeparture, DateDestination);


        }

        private void ButtonSaveClicked(object window)
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
           
            FlightLogic logic = logicParts.flightLogic;
            AirplaneSearchContext airplaneSearcher = new AirplaneSearchContext();

            logic.Information = new FlightInfo(0, 
                Int32.Parse(selectedAirplane),
                cityDeparture,
                cityDestination, 
                airportDeparture, 
                airportDestination,
                terminalDeparture,
                terminalDestination,
                gateDeparture,
                gateDestination, 
                TimeInWay,
                Int32.Parse(milesCount));
            logic.Insert();
            
        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }

       



    }
}
