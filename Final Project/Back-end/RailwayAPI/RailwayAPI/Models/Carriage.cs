using System;
using System.Collections.Generic;

#nullable disable

namespace RailwayAPI
{
    public partial class Carriage
    {
        public int Id { get; set; }
        public int TrainId { get; set; }
        public string Type { get; set; }
        public int Guide1Id { get; set; }
        public int Guide2Id { get; set; }

    }
}
