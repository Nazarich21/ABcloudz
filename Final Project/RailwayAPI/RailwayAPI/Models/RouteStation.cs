using System;
using System.Collections.Generic;

#nullable disable

namespace RailwayAPI
{
    public partial class RouteStation
    {
        public int Id { get; set; }
        public string StationId { get; set; }
        public int Order { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
