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
    public class TicketsViewModel : INotifyPropertyChanged
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
        static private int countSelectedItemsInListTickets;
        private bool isButtonDeleteTicketEnabled;
        TicketLogic ticketRepository;
        private List<Ticket> tickets;
        FlightInfo selectedFlight;
        FlightLogic flightLogic;
        private bool isButtonAddTicketEnabled;
     

        #endregion

        #region Properties
        public static ObservableAirportCollection<FlightInfo> CurrentFlights { get; set; }
        public static ObservableAirportCollection<TicketInfo> CurrentTickets { get; set; }
        public bool IsButtonDeleteTicketEnabled
        {
            get { return isButtonDeleteTicketEnabled; }
            set
            {
                isButtonDeleteTicketEnabled = value;
                OnPropertyChanged("IsButtonDeleteTicketEnabled");
            }
        }
        public bool IsButtonAddTicketEnabled
        {
            get { return isButtonAddTicketEnabled; }
            set
            {
                isButtonAddTicketEnabled = value;
                OnPropertyChanged("IsButtonAddTicketEnabled");
            }
        }

        #endregion

        #region Command Declaration
        public ICommand OnSelectionChangedInListFlights { get; set; }
        public ICommand OnSelectionChangedInListTickets { get; set; }
        public ICommand OnClickDeleteTicket { get; set; }
        public ICommand OnClickSearchFlight { get; set; }
        public ICommand OnClickSearchTicket { get; set; }
        public ICommand OnClickAddTicket { get; set; }
        public ICommand OnClickBack { get; set; }
        #endregion


        public TicketsViewModel()
        {
            OnSelectionChangedInListFlights = new Command(arg => SelectionChangedInListFLights(arg));
            OnSelectionChangedInListTickets = new Command(arg => SelectionChangedInListTickes(arg));
            OnClickAddTicket = new Command(arg => AddTicketClicked());
            OnClickDeleteTicket = new Command(arg => DeleteTicketClicked(arg));
            OnClickSearchFlight = new Command(arg => SearchFlightClicked());
            OnClickSearchTicket = new Command(arg => SearchTicketClicked());
            OnClickBack = new Command(arg => ButtonBackClicked());

            CurrentFlights = new ObservableAirportCollection<FlightInfo>();
            CurrentTickets = new ObservableAirportCollection<TicketInfo>();
            tickets = new List<Ticket>();
            ticketRepository = new TicketLogic();
            flightLogic = new FlightLogic();
            countSelectedItemsInListTickets = 0;
            RefreshTicketsList();
            RefreshFlightsList();
            if (!MainViewModel.Admin) IsButtonAddTicketEnabled = true;
            else CheckButtons();
        }

        private void CheckButtons()
        {
            IsButtonAddTicketEnabled = false;
            if (countSelectedItemsInListTickets == 0) IsButtonDeleteTicketEnabled = false;
            else if (countSelectedItemsInListTickets >= 1) IsButtonDeleteTicketEnabled = true;
        }

        private void ButtonBackClicked()
        {
            RefreshFlightsList();
            RefreshTicketsList();
        }

        private void SelectionChangedInListFLights(object selectedItems)
        {
            System.Collections.IList selectedFlights = (System.Collections.IList)selectedItems;
            ObservableAirportCollection<TicketInfo> selectedTickets = new ObservableAirportCollection<TicketInfo>();

            foreach (FlightInfo flight in selectedFlights)
            {
                if (flight != null)
                {
                    foreach (Ticket ticket in tickets)
                    {
                        if (ticket.ID_flight== flight.FlightID)
                        {
                            selectedTickets.Add(new TicketInfo(ticket.ID_ticket,ticket.ID_passport,ticket.ID_flight,ticket.price_ticket,ticket.buy_date,0));
                        }
                    }
                    CurrentTickets.Clear();
                    CurrentTickets.ReplaceRange(selectedTickets);
                }
            }
        }
        private void SelectionChangedInListTickes(object selectedItems)
        {
            System.Collections.IList selectedTickets = (System.Collections.IList)selectedItems;
            countSelectedItemsInListTickets = selectedTickets.Count;
            if (!MainViewModel.Admin) IsButtonAddTicketEnabled = true;
            else CheckButtons();
        }
        private void DeleteTicketClicked(object selectedItems)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить выбранный рейс?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            System.Collections.IList selectedTypesToDelete;
            List<TicketInfo> ImplementedListOfTicketsDelete = new List<TicketInfo>();
            if (result == MessageBoxResult.Yes)
            {
                selectedTypesToDelete = (System.Collections.IList)selectedItems;
                TicketInfo[] array = new TicketInfo[selectedTypesToDelete.Count];
                selectedTypesToDelete.CopyTo(array, 0);
                ImplementedListOfTicketsDelete.AddRange(array);
            }
            TicketLogic logic = new TicketLogic();
         
            AirportEntities databaseContext = new AirportEntities();

            foreach (TicketInfo info in ImplementedListOfTicketsDelete)
            {
                /*Seat seat = databaseContext.Seat.First(item => item.ID_seat == info.TicketID);
                databaseContext.Seat.Remove(seat);
                databaseContext.SaveChanges();*/
                Ticket ticket = databaseContext.Ticket.First(t => t.ID_ticket == info.TicketID);
                databaseContext.Ticket.Remove(ticket);
                databaseContext.SaveChanges();
                
            }
            RefreshTicketsList();
            RefreshFlightsList();

        }
        private void SearchFlightClicked()
        {
            SearchFlight searchFlight = new SearchFlight();
            searchFlight.ShowDialog();
            SelectionChangedInListFLights(CurrentFlights);
        }
        private void SearchTicketClicked()
        {
            SearchTicket searchTicket = new SearchTicket();
            searchTicket.ShowDialog();
        }
        private void AddTicketClicked()
        {
            AddTicket addTicket = new AddTicket();
            addTicket.ShowDialog();
            RefreshTicketsList();
        }
        private void RefreshTicketsList()
        {
            ticketRepository.SelectAll();
            tickets.Clear();
            tickets = ticketRepository.Tickets;
            CurrentTickets.Clear();
            tickets.ForEach(t => CurrentTickets.Add(new TicketInfo(
                t.ID_ticket,
                t.ID_passport,
                t.ID_flight,
                
                t.price_ticket,
                t.buy_date,
                t.Seat.status_seat
                )
                     )
                );

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
    }
}
