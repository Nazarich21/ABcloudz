using System;
using System.Collections.Generic;

#nullable disable

namespace Airport
{
    public partial class Ticket
    {
        public Ticket()
        {
            TicketFlights = new HashSet<TicketFlight>();
        }

        public string TicketNo { get; set; }
        public string BookRef { get; set; }
        public string PassengerId { get; set; }
        public string PassengerName { get; set; }
        public string ContactData { get; set; }

        public virtual Booking BookRefNavigation { get; set; }
        public virtual ICollection<TicketFlight> TicketFlights { get; set; }
    }
}
