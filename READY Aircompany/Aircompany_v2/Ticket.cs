//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Airport
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public int ID_ticket { get; set; }
        public Nullable<int> ID_passport { get; set; }
        public Nullable<int> ID_flight { get; set; }
        public Nullable<decimal> price_ticket { get; set; }
        public Nullable<System.DateTime> buy_date { get; set; }
    
        public virtual Flight Flight { get; set; }
        public virtual Passenger Passenger { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
