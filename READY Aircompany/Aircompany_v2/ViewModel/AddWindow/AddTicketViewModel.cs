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
    public class AddTicketViewModel: INotifyPropertyChanged
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
        private string flightID;
        private string ticketClassIndex;
        private string ticketClass;
        private string[] TicketClassWithIndex = new string[2] { "Бизнес", "Эконом" };
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
        public string FlightID
        {
            get { return flightID; }
            set 
            { 
                flightID = value;
                OnPropertyChanged("FlightID");
            }
        }        
        public string SelectedTicketIndex
        {
            get { return ticketClassIndex; }
            set
            {
                ticketClassIndex = value;
                OnPropertyChanged("SelectedSexIndex");
                TicketClass = TicketClassWithIndex[Int32.Parse(ticketClassIndex)];
            }
        }
        public string TicketClass
        {
            get { return ticketClass; }
            set { ticketClass = value; }
        }
        #endregion

        #region Command Declaration
        public ICommand OnClickOK { get; set; }
        public ICommand OnClickExit { get; set; }
        public ICommand CheckDigits { get; set; }
        #endregion

        public AddTicketViewModel()
        {
            OnClickOK = new Command(arg => ButtonOkClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
        }
        private void Validation()
        {
            Check.AllStringInput(PassportID.ToString(), FlightID.ToString(), TicketClass);
        }

        private bool IsPassengerFounded()
        {
            PassengerLogic passengerLogic = new PassengerLogic();
            passengerLogic.SelectAll();
            List<PassengerInfo> passengers;
            string[] mas;
            string family;
            string name;
            string surname;
            passengers = new List<PassengerInfo>();
            passengerLogic.passengers.ForEach(pas =>
                {
                    mas = pas.fio.Split(' ');
                    family = mas[0];
                    name = mas[1];
                    surname = mas[2];
                     passengers.Add(new PassengerInfo(pas.ID_passport, surname, name, family, pas.BirthDate, pas.sex));
                });
      
            foreach (PassengerInfo passenger in passengers)
            {
                if (passenger.PassportID == Int32.Parse(PassportID)) return true;
            }
            return false;
        }

        private bool IsFlightFounded()
        {
            FlightLogic flightLogic = new FlightLogic();
            flightLogic.SelectAll();
            List<FlightInfo> flights = new List<FlightInfo>();

            flightLogic.flights.ForEach(flight => flights.Add(new FlightInfo(
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
            foreach (FlightInfo flight in flights)
            {
                if (flight.FlightID == Int32.Parse(FlightID)) return true;
            }
            return false;
        }

        private void ButtonOkClicked(object window)
        {
            try
            {
                Validation();
                if (!IsPassengerFounded())
                {
                    MessageBoxResult result = MessageBox.Show("Данного пассажир не зарегистрирован в базе данных. Добавить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        AddPassenger addPassenger = new AddPassenger();
                        addPassenger.Show();
                    }
                    else return;
                }
                if (!IsFlightFounded())
                {
                    throw new Exception("Не найден соответствующий рейс!");
                }
                int passportId = 0;
                if(!Int32.TryParse(PassportID,out passportId))
                {
                    passportId = 0;
                }
                int flightId = 0;
                if(!Int32.TryParse(FlightID,out flightId))
                {
                    flightId = 0;
                }
                int ticketClass = 0;
                if(TicketClass.Equals("Бизнес"))  ticketClass = 1;
                TicketInfo info = new TicketInfo(0, passportId, flightId,  1000, DateTime.Now, ticketClass);
                TicketLogic logic = new TicketLogic();
                logic.AddNewTicket(info);
                ButtonExitClicked(window);
            //    TicketLogic ticketLogic = new TicketLogic();
            //    ticketLogic.Information = new TicketInfo(0,PassportID,FlightID)

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }

    }
}
