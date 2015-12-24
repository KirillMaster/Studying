using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    class TicketLogic : Logic, ICrudErrorable
    {
        private AirportEntities databaseContext;
        public List<Ticket> Tickets;
        Ticket Ticket;
        FlightLogic flight;
        SeatLogic seat;
        PassengerLogic passenger;
        
        
        public TicketInfo Information { get; set; }



        public override event EventHandler<Exception> InsertionError;
        public override event EventHandler<Exception> SelectionError;
        public override event EventHandler<Exception> DeleteError;
        public override event EventHandler<Exception> EditError;
        public override event EventHandler<Exception> AllSelectedError;

        public event EventHandler<List<Ticket>> DataChanged;


        private bool isNotInformationExists()
        {
            if (Information == null) return true;
            else return false;
        }
        public TicketLogic()
        {
            databaseContext = new AirportEntities();
            Tickets = new List<Ticket>();
            passenger = new PassengerLogic();
            flight = new FlightLogic();
            seat = new SeatLogic();
            
        }

        public void SendDataChangedEvent()
        {
            SelectAll();
            if(DataChanged!=null)
                DataChanged(this, Tickets);
        }
        public void SelectAll()
        {
         
            try
            {
                Tickets = databaseContext.Ticket.ToList();
                databaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                if(AllSelectedError!=null)
                AllSelectedError(this, e);
            }

           
        }

        private Ticket CreateEntity()
        {
            return CreateEntity(Information);
        }
        
        private Ticket CreateEntity(TicketInfo info)
        {
            if (isNotForeignKeyExist()) return null;
            try
            {
                Ticket = new Ticket();
                Ticket.buy_date = info.DateOrder;
                Ticket.price_ticket = info.Price;
                Ticket.ID_flight = info.FlightID;
                Ticket.ID_passport = info.PassportID;
                Ticket.ID_ticket = getMaxId() + 1;
              

                
            }
            catch(Exception)
           {
               return null;
           }
            return Ticket;
        }

        private bool isNotForeignKeyInFlightExist()
        {
            flight.SetInformationOnlyByPrimaryKey(Information.FlightID.Value);
            Flight FlightFoundedByForeignKey = flight.SelectItemByPrimaryKey();

            if (FlightFoundedByForeignKey != null) return false;

            return true;
        }
        private bool isNotForeignKeyInSeatExist()
        {
           /*// seat.SetInformationOnlyByPrimaryKey(Information.SeatID.Value);
            Seat SeatFoundedByForeignKey = seat.SelectItemByPrimaryKey();

            if (SeatFoundedByForeignKey != null) return false;

            return true;*/
            return false;
        }
        private bool isNotForeignKeyInPassengerExist()
        {
            passenger.SetInformationOnlyByPrimaryKey(Information.PassportID.Value);
            Passenger PassengerFoundedByForeignKey = passenger.SelectItemByPrimaryKey();

            if (PassengerFoundedByForeignKey != null) return false;

            return true;
        }
        private bool isNotForeignKeyExist()
        {

            if (isNotForeignKeyInFlightExist() || isNotForeignKeyInPassengerExist() )
                return true;
            else return false;
        }
        public void AddNewTicket(TicketInfo information)
        {
            Information = information;
            Ticket = CreateEntity();
            Ticket.buy_date = DateTime.Now;
            Ticket.price_ticket = 1000;
            PermanentCoefficientsLogic coef = new PermanentCoefficientsLogic();
            coef.Information = new PermanentCoefficientInfo(0, 0, 0, 0, 0);
            PermanentCoefficients coeficients = coef.SelectItemByPrimaryKey();
           if(coeficients == null)
           {
               MessageBox.Show("Вы не указали коэффиценты для расчета стоимости");
               return;
           }
            AirplaneTypeLogic typeLogic = new AirplaneTypeLogic();
            FlightLogic flightLogic = new FlightLogic();
            FlightSearchContext flightSearcher = new FlightSearchContext();
            TicketLogic ticketLogic = new TicketLogic();

            List<FlightInfo> flight = flightSearcher.Search(new FlightInfo(information.FlightID, null, null, null, null, null, null, null, null, null, null, null));
           
            AirplaneSearchContext typeSearcher = new AirplaneSearchContext();
            List<AirplanesIDWithTypeName> airplane = typeSearcher.AirplaneSearch(new AirplanesIDWithTypeName(flight[0].AirplaneID.Value, null));
            string name = airplane[0].name;
            
            AirplaneType airplaneType = databaseContext.AirplaneType.First(type => type.name_.Equals(name));
            Ticket.price_ticket = (decimal)( (DateTime.Now - Ticket.buy_date.Value).Days * coeficients.buyDateCoefficient.Value + flight[0].MilesCount * coeficients.mileCoefficient);
            if (information.Class == 0)
            {
                Ticket.price_ticket += (decimal)coeficients.BusinessClassCoefficient.Value*1000;
            }
            else Ticket.price_ticket += (decimal)coeficients.economClassCoefficient.Value*5000;
            int bSeatsCountInThisFlight = airplaneType.business_seats_count.Value;
            int ecSeatsCountInThisFlight = airplaneType.econom_seats_count.Value;
            if (information.Class == 1) AddBusinessTicket(Ticket, information, bSeatsCountInThisFlight);
            if (information.Class == 0) AddEconomTicket(Ticket, information, ecSeatsCountInThisFlight);

            return;

        }
        private void AddBusinessTicket(Ticket entity,TicketInfo information,int businessSeatsInFlight)
        {
            TicketLogic ticketLogic = new TicketLogic();
            const int businessClass = 1;
            int currentCountOfBusinessSeats = 0;
            TicketSearchContext ticketSearcher = new TicketSearchContext();
            var res = from seat in databaseContext.Seat
                      join ticket in databaseContext.Ticket on
                      seat.ID_seat equals ticket.ID_ticket
                      join flight
                          in databaseContext.Flight on ticket.ID_flight equals flight.ID_flight
                      where (flight.ID_flight == information.FlightID) && (seat.@class == businessClass)
                      select new { seat.ID_seat, seat.number_seat, seat.status_seat, seat.@class };

            foreach (var item in res)
            {

                currentCountOfBusinessSeats++;
            }
            if ((businessSeatsInFlight - currentCountOfBusinessSeats) <= 0)
            {
                MessageBox.Show("Больше нет мест бизнес-класса");
                return;
            }
            else
            {
                Information = information;
              
                
                ticketLogic.Information = new TicketInfo(Ticket.ID_ticket,
                                                   Ticket.ID_passport,
                                                   Ticket.ID_flight,
                                                   
                                                   Ticket.price_ticket,
                                                   Ticket.buy_date,
                                                   1);
                ticketLogic.Insert();
                seat.Information = new SeatInfo(Ticket.ID_ticket, 0, 1, 1);
                seat.Insert();
               

            }
        }
        private void AddEconomTicket(Ticket entity, TicketInfo information,int economSeatsInFlight)
        {
            TicketLogic ticketLogic = new TicketLogic();
            const int economClass = 0;
            int currentCountOfEconomSeats = 0;
            TicketSearchContext ticketSearcher = new TicketSearchContext();

            var res= from seat in databaseContext.Seat
                         join ticket in databaseContext.Ticket on
                         seat.ID_seat equals ticket.ID_ticket
                         join flight
                             in databaseContext.Flight on ticket.ID_flight equals flight.ID_flight
                         where (flight.ID_flight == information.FlightID)&&(seat.@class == economClass)
                         select new { seat.ID_seat, seat.number_seat, seat.status_seat, seat.@class };
           foreach(var item in res)
           {

               currentCountOfEconomSeats++;
           }
         
            if ((economSeatsInFlight - currentCountOfEconomSeats) <= 0)
            {
                MessageBox.Show("Больше нет мест эконом-класса");
                return;
            }
            else
            {
                Information = information;

            
                ticketLogic.Information = new TicketInfo(Ticket.ID_ticket,
                                                        Ticket.ID_passport,
                                                        Ticket.ID_flight,
                                                      
                                                        Ticket.price_ticket,
                                                        Ticket.buy_date,
                                                        0);
                ticketLogic.Insert();
                seat.Information = new SeatInfo(Ticket.ID_ticket, 0, 1, 0);
                seat.Insert();
                
            }
        }
        public void Insert()
        {
           
            try
           {
                Ticket = CreateEntity();
                databaseContext.Ticket.Add(Ticket);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
           }
            catch(Exception e)
            {
                if(InsertionError!=null)
                InsertionError(this,e);
            }
           
        }
        private int getMaxId()
        {
            int max = 0;
            try
            {
                max = databaseContext.Ticket.Max((Ticket) => Ticket.ID_ticket);
            }
            catch(Exception e)
            {
              //  MessageBox.Show("getMaxIdError");
                max = 0;
            }
            return max;
        }
        public void SetInformationOnlyByPrimaryKey(int ID)
        {
            Information = new TicketInfo(ID, 0, 0, 0, DateTime.Now,null);
        }
        public Ticket SelectItemByPrimaryKey()
        {
            if (isNotInformationExists()) return null;
            try
            {
                Ticket Ticket = databaseContext.Ticket.Single(item => (item.ID_ticket == Information.TicketID));
                return Ticket;
            }
           catch(Exception e)
            {
               if(SelectionError!=null)
                SelectionError(this, e);
            }
            return null;
        }
        public void DeleteByPrimaryKey()
        {
            if (isNotInformationExists()) return;
            Ticket Ticket = SelectItemByPrimaryKey();
            try
            {
                   if(Ticket == null)
                        {
                            throw new NullReferenceException();
                        }
                databaseContext.Ticket.Remove(Ticket);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch(Exception e)
            {
                if(DeleteError!=null)
                DeleteError(this, e);
            }
            
        }
        public void Edit(TicketInfo oldInfo,TicketInfo newInfo)
        {

            try
            {
                Information = oldInfo;
                Ticket oldTicket = SelectItemByPrimaryKey();
                databaseContext.Ticket.Remove(oldTicket);
                Information = newInfo;
                Ticket newTicket = CreateEntity();
                databaseContext.Ticket.Add(newTicket);
                databaseContext.SaveChanges();
                SendDataChangedEvent();
            }
            catch (Exception e)
            {
                if(EditError!=null)
                EditError(this, e);
            }
        }
        
        public void test()
        {
            DataChanged += (obj, list) => MessageBox.Show("Data succesfully changed! :)"); 
   
            InsertionError += (o, e) => MessageBox.Show("InsertionError "+ e.Message);
            DeleteError += (o, e) => MessageBox.Show("DeleteError " + e.Message);
            EditError += (o, e) => MessageBox.Show("EditError " + e.Message);
            SelectionError += (o, e) => MessageBox.Show("SelectionError " + e.Message);
            AllSelectedError += (o, e) => MessageBox.Show("AllSelectionError " + e.Message);

            Information = new TicketInfo(1, 123, 1, 2000, DateTime.Now,null);

            Insert();
         
        }
    }
}
