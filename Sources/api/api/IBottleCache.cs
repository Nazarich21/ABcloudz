using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api
{
    public interface IBottleCache
    {
        Bottle Get(int id);
        void Remove(int id);
        void Set(Bottle bottle);
    }
}
