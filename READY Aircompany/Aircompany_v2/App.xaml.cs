using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace Airport
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        public static List<AirplaneTypeInfo> AirplaneTypes = new List<AirplaneTypeInfo>();
        public static List<AirplaneInfo> Airplanes = new List<AirplaneInfo>();
        public static List<FlightInfo> Fligts = new List<FlightInfo>();
        public static List<FlightStatusInfo> StatusFlights = new List<FlightStatusInfo>();
        public static ObservableCollection<PassengerInfo> Passengers = new ObservableCollection<PassengerInfo>();
        public static ObservableCollection<TicketInfo> Tickets = new ObservableCollection<TicketInfo>();
        public static List<AirplanesWithTypes> AirplanesWithTypes = new List<AirplanesWithTypes>();

        //  AirplaneInfo airplane1 = new AirplaneInfo(1234, 1);
        //  AirplaneInfo airplane2 = new AirplaneInfo(452, 2);

        AirplanesWithTypes airplane1 = new AirplanesWithTypes(123, 1, 12, 23, 12, 23, "123name");
        AirplanesWithTypes airplane2 = new AirplanesWithTypes(456, 2, 34, 3, 2, 1, "456name");

     // //  TicketInfo ticket1 = new TicketInfo(2144, 123, 789, 33, 234, new DateTime(1999, 10, 10));
      //  TicketInfo ticket2 = new TicketInfo(23434, 123, 789, 1, 3, new DateTime(1999, 12, 12));
      //  TicketInfo ticket3 = new TicketInfo(1233, 456, 890, 12, 34, new DateTime(2000, 01, 01));

        PassengerInfo passenger1 = new PassengerInfo(123, "fsdf", "fdf", "fdsf", new DateTime(1989, 12, 12), "женский");
        PassengerInfo passenger2 = new PassengerInfo(456, "3242", "gdfg", "sfd", new DateTime(1999, 12, 12), "мужской");

        AirplaneInfo airplane11 = new AirplaneInfo(123, 1);
        AirplaneInfo airplane12 = new AirplaneInfo(456, 2);

        AirplaneTypeInfo type1 = new AirplaneTypeInfo(1, "ttt", 123, 24, 12, 12);
        AirplaneTypeInfo type2 = new AirplaneTypeInfo(2, "444", 43, 42, 41, 12);

        FlightInfo flight1 = new FlightInfo(789, 123, "ujdsfsdfhsjf", "234234234", "2342", "24234", "34234", "2342342", "234234", "dgfdfg", new TimeSpan(12, 12, 12), 3434);
        FlightInfo flight2 = new FlightInfo(890, 456, "sdfsf", "dfsdfsdf", "wfwfef", "ewefwefr", "2342424", "sdfsfds", "sdfsdfs", "2234", new TimeSpan(12, 25, 25), 435);

        FlightStatusInfo status1 = new FlightStatusInfo(789, "Задержан", new DateTime(2016, 12, 12), new DateTime(2016, 12, 13), new TimeSpan(15, 12, 13));
        FlightStatusInfo status2 = new FlightStatusInfo(890, "Не активен", new DateTime(2017, 12, 12), new DateTime(2017, 12, 14), new TimeSpan(21, 21, 21));
        public static bool Admin { get; set; }
        public App()
        {
            AirplanesWithTypes.Clear();
            AirplanesWithTypes.Add(airplane1);
            AirplanesWithTypes.Add(airplane2);

           // Tickets.Add(ticket1);
         //   Tickets.Add(ticket2);
         //   Tickets.Add(ticket3);

            Passengers.Add(passenger1);
            Passengers.Add(passenger2);

            AirplaneTypes.Add(type1);
            AirplaneTypes.Add(type2);

            Fligts.Add(flight1);
            Fligts.Add(flight2);

            StatusFlights.Add(status1);
            StatusFlights.Add(status2);

            //airplane11.AirplaneID = airplane1.ID_airplane;
            //airplane11.AirplaneType = airplane1.airplaneTypeId;

            //airplane12.AirplaneID = airplane2.ID_airplane;
            //airplane12.AirplaneType = airplane2.airplaneTypeId;

            Airplanes.Add(airplane11);
            Airplanes.Add(airplane12);

            Admin = true;
            //MainWindow mainWindow = new MainWindow
            //{
            //    DataContext = new MainViewModel()
            //};

            //mainWindow.Show();
            //MainViewModel mainView = new MainViewModel();

        }





    }
}

