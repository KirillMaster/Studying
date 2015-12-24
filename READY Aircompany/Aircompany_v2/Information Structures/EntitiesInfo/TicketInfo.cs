using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class TicketInfo : Info
    {
        private DateTime? dateOrder;
        public int? TicketID { get; set; }
        public int? PassportID { get; set; }
        public int? FlightID { get; set; }
   
        public decimal? Price { get; set; }

        public int? Class { get; set; }
        public DateTime? DateOrder
        {
            get
            {
                if (dateOrder.HasValue)
                    return dateOrder.Value.Date;
                else return null;
            }
            set { dateOrder = value; }
        }

        public TicketInfo(int? ticketID, int? passportID, int? flightID,decimal? price, DateTime? dateOrder, int? Class)
        {
            TicketID = ticketID;
            PassportID = passportID;
            FlightID = flightID;
          
            Price = price;
            DateOrder = dateOrder;
            this.Class = Class;
        }
    }
}
