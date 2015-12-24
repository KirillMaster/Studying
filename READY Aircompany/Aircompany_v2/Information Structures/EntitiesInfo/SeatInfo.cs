using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class SeatInfo : Info
    {
        public int SeatID { get; set; }
        public int SeatNumber { get; set; }
     
        public int Status { get; set; }
        public int Class { get; set; }

        public SeatInfo(int seatID, int SeatNumber, int status, int seatClass)
        {
            this.SeatNumber = SeatNumber;
            this.SeatID = seatID;
         
            this.Status = status;
            this.Class = seatClass;
        }
    }
}
