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
    public class SearchTicketViewModel : INotifyPropertyChanged
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
        private string ticketID;
        private string passportID;
        private string surname;
        private string name;
        private string familyName;
        private string ticketClassIndex;
        private DateTime? dateOrder;
        private string dateOrderInBox;
        private string ticketClass;
        private string[] TicketClassWithIndex = new string[2] { "Бизнес", "Эконом" };
        #endregion

        #region Properties
        public string TicketID
        {
            get { return ticketID; }
            set
            {
                ticketID = value;
                OnPropertyChanged("TicketID");
            }
        }
        public string PassportID
        {
            get { return passportID; }
            set
            {
                passportID = value;
                OnPropertyChanged("PassportID");
            }
        }
        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string FamilyName
        {
            get { return familyName; }
            set
            {
                familyName = value;
                OnPropertyChanged("FamilyName");
            }
        }
        public string DateOrderInBox
        {
            get { return dateOrderInBox; }
            set
            {
                dateOrderInBox = value;
                OnPropertyChanged("DateOrderInBox");
            }
        }
        public DateTime? DateOrder
        {
            get
            {
                if (dateOrder != null)
                    return dateOrder;
                else return null;
            }
            set { dateOrder = value; }
        }
        public string SelectedTicketClassIndex
        {
            get { return ticketClassIndex; }
            set
            {
                ticketClassIndex = value;
                OnPropertyChanged("SelectedTicketClassIndex");
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
        public ICommand CheckText { get; set; }
        #endregion

        public SearchTicketViewModel()
        {
            OnClickOK = new Command(arg => ButtonOkClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
            CheckText = new Command(arg => Check.OnlyText(arg));
        }
        private void Validation()
        {
            Check.IfOneStringInput(TicketID, PassportID, Name, Surname, FamilyName, TicketClass, DateOrderInBox);
        }

        private void ButtonOkClicked(object window)
        {
            try
            {
                Validation();
                ButtonExitClicked(window);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TicketSearchContext searcher = new TicketSearchContext();
            AirportEntities databaseContext = new AirportEntities();
            int x;
            int ticketId = 0;
            if(!Int32.TryParse(TicketID,out ticketId))
                ticketId =0;
            int? passportId = 0;
            if (!Int32.TryParse(PassportID, out x))
                passportId = null;
            else passportId = x;
            
            int? ticketClass = 0;
            if (TicketClass == "Эконом") ticketClass = 0;
            else if (TicketClass == "Бизнес") ticketClass = 1;
            else ticketClass = null;

            List<TicketInfo> info = searcher.Search(new TicketInfo(ticketId, passportId, null, null, DateOrder, ticketClass));
            List<TicketInfo> result = new List<TicketInfo>();
            if (ticketClass != null)
            {
                List<Seat> seats = databaseContext.Seat.Select(item => item).Where(item => ticketClass.Value == item.@class).ToList();
                foreach (TicketInfo i in info)
                {
                    foreach (Seat s in seats)
                    {
                        if (i.TicketID == s.ID_seat) result.Add(i);
                    }
                }
            }
            if(info.Count == 0)
            {
                List<Seat> seats = databaseContext.Seat.Select(item => item).Where(item => ticketClass.Value == item.@class).ToList();
                List<Ticket> tickets = databaseContext.Ticket.ToList();
                foreach(Ticket t in tickets)
                {
                    foreach(Seat seat in seats)
                    {
                        if (t.ID_ticket == seat.ID_seat) result.Add(new TicketInfo(t.ID_ticket, t.ID_passport, t.ID_flight, t.price_ticket, t.buy_date, 0));
                    }
                }
            }
            else result = info;
            TicketsViewModel.CurrentTickets.Clear();
            TicketsViewModel.CurrentFlights.Clear();
            result.ForEach(ticket => TicketsViewModel.CurrentTickets.Add(new TicketInfo(ticket.TicketID,ticket.PassportID,ticket.FlightID,ticket.Price,ticket.DateOrder,ticket.Class)));
           
            
        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }



    }
}
