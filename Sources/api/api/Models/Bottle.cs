using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{

    public class Bottle
    {
        public string Material { get; set; }
        public string KindOfDrink { get; set; }
        public double Size { get; set; }
        public override int GetHashCode()
        {
            return Material.GetHashCode() ^ KindOfDrink.GetHashCode() ^ Size.GetHashCode();
        }

        public int GetId()
        {
            return GetHashCode();
        }

    }


}
