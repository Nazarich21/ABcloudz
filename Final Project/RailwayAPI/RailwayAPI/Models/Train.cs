using System;
using System.Collections.Generic;

#nullable disable

namespace RailwayAPI
{
    public partial class Train
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int DriverId { get; set; }
        public int DriverAssistId { get; set; }
        public int TrainmasterId { get; set; }
        public string Type { get; set; }
        public bool EveryDay { get; set; }
        public bool? EvenDays { get; set; }
        public int[] Days { get; set; }
    }
}
