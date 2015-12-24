using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Airport;
using System.ComponentModel;

namespace Airport.ViewModel
{
    public class MainViewModel: INotifyPropertyChanged
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
        //#region Constructor
        public static List<AirplaneTypeInfo> AirplaneTypes = new List<AirplaneTypeInfo>();
        public static List<AirplaneInfo> Airplanes = new List<AirplaneInfo>();
        public static List<FlightInfo> Fligts = new List<FlightInfo>();
        public static List<FlightStatusInfo> StatusFlights = new List<FlightStatusInfo>();
        public static List<PassengerInfo> Passengers = new List<PassengerInfo>();
        public static List<TicketInfo> Tickets = new List<TicketInfo>();
        public static List<AirplanesWithTypes> AirplanesWithTypes = new List<AirplanesWithTypes>();
        Uri planesUri;
       
        private Uri _source;
        public Uri Source
        {
            get
            { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged("Source");
            }
        }

        public static bool Admin { get; set; }
        public MainViewModel()
        {
          
            ClickTickets = new Command(arg => TicketsClicked());
            ClickExit = new Command(arg => ExitClicked());
            ClickAirplanes = new Command(arg => AirplanesClicked());
            ClickCoef = new Command(arg => CoefClicked());
            ClickFlights = new Command(arg => FlightsClicked());
            ClickPassengers = new Command(arg => PassengersClicked());

            if (Admin)
            {
                Source = new Uri("Planes.xaml", UriKind.Relative);
            }
            else if (!Admin)
            {
                Source = new Uri("Tickets.xaml", UriKind.Relative);
            }
            planesUri = new Uri("Planes.xaml", UriKind.Relative);

        }

        public ICommand ClickTickets {get; set;}
        public ICommand ClickFlights {get; set;}
        public ICommand ClickPassengers {get; set;}
        public ICommand ClickCoef {get; set;}
        public ICommand ClickExit {get; set;}
        public ICommand ClickAirplanes {get; set;}

         private void TicketsClicked()
         {
             Source = new Uri("Tickets.xaml", UriKind.Relative);         
         }
         private void FlightsClicked()
         {
             Source = new Uri("Flights.xaml", UriKind.Relative);
         }
         private void PassengersClicked()
         {
             Source = new Uri("Passengers.xaml", UriKind.Relative);
         }
         private void CoefClicked()
         {
             AdminCoef coef = new AdminCoef();
             coef.ShowDialog();
         }
         private void ExitClicked()
         {
             Application.Current.MainWindow.Close();
         }
         private void AirplanesClicked()
         {
             Source = planesUri;
         }

      

    //        ClickCommand = new Command(arg => ClickMethod());
    //        ChangeItemText = new Command(arg => CheckInput(arg));
    //        PutTime = new Command(arg => PutTimeHanlder(arg));

    //        People = new PeopleModel
    //        {
                
    //            FirstName = "1234",
    //            LastName = "Last name",
    //        };

    //    }

    //    #endregion
    //    public ICommand ClickCommand { get; set; }
    //    public ICommand ChangeItemText { get; set; }
    //    public ICommand PutTime { get; set; }
 
    //    /// <summary>
    //    /// Get or set people.
    //    /// </summary>
    //    public PeopleModel People { get; set; }


    //    private void ClickMethod()
    //    {
    //        People.FirstName = "privet";
    //        MessageBox.Show("This is click command.");
    //    }

    //    private void CheckInput(object textComposition)
    //    {
    //        TextCompositionEventArgs textCompoisitonEventArgs = textComposition as TextCompositionEventArgs;
           
           
    //        if (!Char.IsDigit(textCompoisitonEventArgs.Text, 0))
    //        {
    //            textCompoisitonEventArgs.Handled = true;
                
            
    //        }
            
        
    //    }
    //private void PutTimeHanlder(object sender)
    //    {
    //        string inputText = (sender as TextBox).Text;
           
    //            if (inputText.Length == 2)
    //            {
    //                (sender as TextBox).Text = inputText + ":";
    //                (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
    //            }
           
           
        }
     
    }

