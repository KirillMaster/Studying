using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class TicketSearchContext
    {
        TicketInfo ticketInfo;
        List<TicketInfo> ticketInfoList;
        AirportEntities databaseContext;
        public TicketSearchContext()
        {
            ticketInfoList = new List<TicketInfo>();
            databaseContext = new AirportEntities();
        }
        private bool PredicateToTicketSearch(Ticket ticket)
        {
            int neededCountOfInitializedFields = 0;
            int currentCountOfEqualFields = 0;
            if (ticketInfo.DateOrder != null) neededCountOfInitializedFields++;
            if (ticketInfo.FlightID != null) neededCountOfInitializedFields++;
            if (ticketInfo.PassportID != null) neededCountOfInitializedFields++;
            if (ticketInfo.Price != null) neededCountOfInitializedFields++;
          
            if (ticketInfo.TicketID != null) neededCountOfInitializedFields++;

            if (ticketInfo.DateOrder.Equals(ticket.buy_date)) currentCountOfEqualFields++;
            if (ticketInfo.FlightID == ticket.ID_flight) currentCountOfEqualFields++;
            if (ticketInfo.PassportID == ticket.ID_passport) currentCountOfEqualFields++;
            if (ticketInfo.Price == ticket.price_ticket) currentCountOfEqualFields++;
          
            if (ticketInfo.TicketID == ticket.ID_ticket) currentCountOfEqualFields++;
            if (neededCountOfInitializedFields == 0) return false;
            if (currentCountOfEqualFields == 0) return false;
            if (neededCountOfInitializedFields == currentCountOfEqualFields) return true;
            return false;
        }
        public List<TicketInfo> Search(TicketInfo info)
        {
            ticketInfo = info;
            ticketInfoList.Clear();
            Func<Ticket, bool> predicate = PredicateToTicketSearch;
            var res = databaseContext.Ticket.Select(item => item).Where(predicate);
            foreach (var item in res)
            {
                ticketInfoList.Add(new TicketInfo(item.ID_ticket,
                                                 item.ID_passport,
                                                 item.ID_flight,
                                               
                                                 item.price_ticket,
                                                 item.buy_date,
                                                 item.Seat.status_seat));
            }
            return ticketInfoList;
        }
    }
}
