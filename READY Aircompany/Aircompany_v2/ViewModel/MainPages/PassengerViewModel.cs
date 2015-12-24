using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Airport.ViewModel
{
    public class PassengerViewModel : INotifyPropertyChanged
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
        public static ObservableAirportCollection<PassengerInfo> CurrentPassengers { get; set; }
        public static ObservableAirportCollection<TicketInfo> CurrentTickets { get; set; }
        private List<Passenger> passengers;
        private List<Ticket> tickets;
        static private int countSelectedItemsInListTickets { get; set; }
        static private int countSelectedItemsInListPassengers { get; set; }
        private bool isButtonDeletePassengerEnabled;
        private bool isButtonDeleteTicketEnabled;
        private bool isButtonDeleteEnabled;
        AirportEntities databaseContext;
        PassengerLogic repository;
        TicketLogic ticketRepository;
        #endregion

        #region Properties
        public bool IsButtonDeletePassengerEnabled
        {
            get { return isButtonDeletePassengerEnabled; }
            set
            {
                isButtonDeletePassengerEnabled = value;
                OnPropertyChanged("IsButtonDeletePassengerEnabled");
            }
        }
        public bool IsButtonDeleteTicketEnabled
        {
            get { return isButtonDeleteTicketEnabled; }
            set
            {
                isButtonDeleteTicketEnabled = value;
                OnPropertyChanged("IsButtonDeleteTicketEnabled");
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

        #endregion

        #region Command Declaration
        public ICommand OnClickDeletePassenger { get; set; }
        public ICommand OnClickDeleteTicket { get; set; }
        public ICommand OnClickSearchPassenger { get; set; }
        public ICommand OnClickSearchTicket { get; set; }
        public ICommand OnClickAddPassenger { get; set; }
        public ICommand OnSelectionChangedInListPassengers { get; set; }
        public ICommand OnSelectionChangedInListTickets { get; set; }
        public ICommand OnClickBack { get; set; }
    
        #endregion

        public PassengerViewModel()
        {
            OnClickAddPassenger = new Command(arg => AddPassengerClicked());
            OnClickDeletePassenger = new Command(arg => DeletePassengerClicked(arg));
            OnClickDeleteTicket = new Command(arg => DeleteTicketClicked(arg));
            OnClickSearchPassenger = new Command(arg => SearchPassengerClicked());
            OnClickSearchTicket = new Command(arg => SearchTicketClicked());
            OnSelectionChangedInListPassengers = new Command(arg => SelectionChangedInListPassengers(arg));
            OnSelectionChangedInListTickets = new Command(arg => SelectionChangedInListTickets(arg));
            OnClickBack = new Command(arg => ButtonBackClicked());

            CurrentTickets = new ObservableAirportCollection<TicketInfo>();
            CurrentPassengers = new ObservableAirportCollection<PassengerInfo>();
        
            countSelectedItemsInListPassengers = 0;
            countSelectedItemsInListTickets = 0;
            databaseContext = new AirportEntities();
            repository = new PassengerLogic();
            ticketRepository = new TicketLogic();
            passengers = new List<Passenger>();
            tickets = new List<Ticket>();

            RefreshPassengerList();
            RefreshPassengerTicketsList();
            CheckButtons();

            if (!MainViewModel.Admin) CloseAccess();
            
        }
        private void ButtonBackClicked()
        {
            RefreshPassengerList();
            RefreshPassengerTicketsList();
        }

        private void CloseAccess()
        {
            IsButtonDeleteEnabled = false;
            IsButtonDeletePassengerEnabled = false;
        }

        private void CheckButtons()
        {
            if (countSelectedItemsInListTickets >= 1) IsButtonDeleteTicketEnabled = true;
            else IsButtonDeleteTicketEnabled = false;
            if (countSelectedItemsInListPassengers >= 1) IsButtonDeletePassengerEnabled = true;
            else IsButtonDeletePassengerEnabled = false;
            if (countSelectedItemsInListPassengers == 0 && countSelectedItemsInListTickets == 0) IsButtonDeleteEnabled = false;
            else if ((countSelectedItemsInListTickets >= 1) || (countSelectedItemsInListPassengers >= 1)) IsButtonDeleteEnabled = true;
        }

        private void DeletePassengerClicked(object selectedItems)
        {
            System.Collections.IList selectedPassengersToDelete;
            List<PassengerInfo> ImplementedListOfPassengersToDelete = new List<PassengerInfo>();
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить выбранных пассажиров?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                 selectedPassengersToDelete = (System.Collections.IList)selectedItems;
                 PassengerInfo[] array = new PassengerInfo[selectedPassengersToDelete.Count];
                 selectedPassengersToDelete.CopyTo(array, 0);
                 ImplementedListOfPassengersToDelete.AddRange(array);
            }
            PassengerLogic logic = new PassengerLogic();
            foreach(PassengerInfo info in ImplementedListOfPassengersToDelete)
            {
                logic.Information = info;
                logic.DeleteByPrimaryKey();
            }
            RefreshPassengerList();
            RefreshPassengerTicketsList();
        }

        private void DeleteTicketClicked(object selectedItems)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить выбранные билеты?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                System.Collections.IList selectedTicketsToDelete = (System.Collections.IList)selectedItems;
            }
        }

        private void SearchPassengerClicked()
        {
            SearchPassenger searchPassenger = new SearchPassenger();
            searchPassenger.ShowDialog();
        }
        private void SearchTicketClicked()
        {
            SearchTicket searchTicket = new SearchTicket();
            searchTicket.ShowDialog();
        }
        private void AddPassengerClicked()
        {
            AddPassenger addPassenger = new AddPassenger();
            addPassenger.ShowDialog();
            RefreshPassengerList();
        }
        private void SelectionChangedInListPassengers(object selectedItems)
        {
            System.Collections.IList selectedPassengers = (System.Collections.IList)selectedItems;
            ObservableAirportCollection<TicketInfo> selectedTickets = new ObservableAirportCollection<TicketInfo>();

            countSelectedItemsInListPassengers = selectedPassengers.Count;

            foreach (PassengerInfo passenger in selectedPassengers)
            {
                if (passenger != null)
                {
                    foreach (Ticket ticket in tickets)
                    {
                        if (ticket.ID_passport == passenger.PassportID)
                        {
                            selectedTickets.Add(new TicketInfo(ticket.ID_ticket,
                                ticket.ID_passport,
                                ticket.ID_flight,
                            
                                ticket.price_ticket,
                                ticket.buy_date,
                                ticket.Seat.status_seat));
                        }
                    }
                    CurrentTickets.Clear();
                    CurrentTickets.ReplaceRange(selectedTickets);
;                }
            }
            if (!MainViewModel.Admin) CloseAccess();
            else CheckButtons();
        }

        private void SelectionChangedInListTickets(object selectedItems)
        {
            System.Collections.IList selectedTickets = (System.Collections.IList)selectedItems;
            countSelectedItemsInListTickets = selectedTickets.Count;
            if (!MainViewModel.Admin) CloseAccess(); 
            else CheckButtons();
        }
        private void RefreshPassengerList()
        {
            repository.SelectAll();
            passengers.Clear();
            passengers = repository.passengers;
            CurrentPassengers.Clear();
            string[] mas;
            string family;
            string name;
            string surname;
            repository.passengers.ForEach(pas =>
                {
                    mas = pas.fio.Split(' ');
                    family = mas[0];
                    name = mas[1];
                    surname = mas[2];
                    CurrentPassengers.Add(new PassengerInfo(pas.ID_passport, surname, name, family, pas.BirthDate, pas.sex));
                });
        }

        private void RefreshPassengerTicketsList()
        {
            ticketRepository.SelectAll();
            tickets.Clear();
            tickets = ticketRepository.Tickets;
            CurrentTickets.Clear();
            ticketRepository.Tickets.ForEach(t => CurrentTickets.Add(new TicketInfo(
                t.ID_ticket,
                t.ID_passport,
                t.ID_flight, 
                
                t.price_ticket,
                t.buy_date,
                t.Seat.status_seat)
                     )
                );

        }

    }
}
