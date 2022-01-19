using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayAPI.Helpers
{
    public class Price
    {
        public string TrainType { get; set; }
        public string CarriageType{ get; set; }

        public override bool Equals(object other)
        {
            var otherFoo = other as Price;
            if (otherFoo == null)
                return false;
            return TrainType == otherFoo.TrainType && CarriageType == otherFoo.CarriageType;
        }
        public override int GetHashCode()
        {
            return 17 * TrainType.GetHashCode() + CarriageType.GetHashCode();
        }
    }

}
