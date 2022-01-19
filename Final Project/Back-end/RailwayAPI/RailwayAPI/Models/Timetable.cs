using System;
using System.Collections.Generic;

#nullable disable

namespace RailwayAPI
{
    public partial class Timetable
    {
        public int? Train { get; set; }
        public string Status { get; set; }
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? DepartureTime { get; set; }
    }
}
