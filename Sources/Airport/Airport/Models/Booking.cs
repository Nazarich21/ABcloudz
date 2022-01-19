using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Airport
{
    public partial class Booking
    {
        public Booking()
        {
            Tickets = new HashSet<Ticket>();
        }
        
        public string BookRef { get; set; }
        public DateTime BookDate { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
