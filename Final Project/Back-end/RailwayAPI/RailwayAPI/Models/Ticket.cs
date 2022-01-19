using System;
using System.Collections.Generic;

#nullable disable

namespace RailwayAPI
{
    public partial class Ticket
    {
        public int Ticketid { get; set; }
        public string Seat { get; set; }
        public int TrainId { get; set; }
        public int CarriageId { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureStationId { get; set; }
        public string ArrivalStationId { get; set; }
        public DateTime OperationDate { get; set; }
        public int UserId { get; set; }
        public string DocumentType { get; set; }
        public decimal Price { get; set; }
        public string DocumentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
    }
}
