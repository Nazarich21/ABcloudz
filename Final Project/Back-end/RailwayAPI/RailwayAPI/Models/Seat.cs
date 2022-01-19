using System;
using System.Collections.Generic;
using System.Data.Entity;

#nullable disable

namespace RailwayAPI
{
    public partial class Seat
    {
        public int Id { get; set; }
        public string SeatNo { get; set; }
        public string Status { get; set; }

       
    }
}
