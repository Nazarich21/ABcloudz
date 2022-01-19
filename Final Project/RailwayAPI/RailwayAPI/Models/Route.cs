using System;
using System.Collections.Generic;

#nullable disable

namespace RailwayAPI
{
    public partial class Route
    {
        public int Id { get; set; }
        public string LastStationId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public string FirstStationId { get; set; }
    }
}
