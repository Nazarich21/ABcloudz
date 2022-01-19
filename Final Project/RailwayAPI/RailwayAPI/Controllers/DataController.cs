using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RailwayAPI.Controllers
{
    [Route("data")]
    [ApiController]
    public class DataController : Controller
    {
        private readonly railwayContext _context;

        public DataController(railwayContext context)
        {
            _context = context;
        }

        [HttpGet("reportTrain")]
        public async Task<ActionResult> GetTrainRepoertAmountOfPassengers(DateTime beginDate, DateTime endDate, int train)
        {
            var tickets = await (from a in _context.Tickets
                                 where a.DepartureDate <= endDate && a.DepartureDate >= beginDate && a.TrainId == train
                                 select a).ToListAsync();

            var revenue = tickets.Sum(a => a.Price);
            var amountOfPassengers = tickets.Count();
            var result = new { revenue, amountOfPassengers };
            return Ok(result);
        }

        [HttpGet("reportStation")]
        public async Task<ActionResult> GetStationReportAmountOfPassengers(DateTime beginDate, DateTime endDate, string station)
        {

            var amountArrivalPassengers = await (from a in _context.Tickets
                                                 where a.DepartureDate <= endDate && a.DepartureDate >= beginDate && a.ArrivalStationId == station
                                                 select a).CountAsync();

            var amountDepartPassengers = await (from a in _context.Tickets
                                                where a.DepartureDate <= endDate && a.DepartureDate >= beginDate && a.DepartureStationId == station
                                                select a).CountAsync();

            var list = new { amountArrivalPassengers, amountDepartPassengers };

            return Ok(list);
        }

        [HttpGet("allStations")]
        public async Task<ActionResult> GetAllStationsOfTrain(int train)
        {
            var result = await (from a in _context.RouteStations
                                join b in _context.Routes on a.Id equals b.Id
                                join t in _context.Trains on b.Id equals t.RouteId
                                join c in _context.Stations on a.StationId equals c.Id
                                where t.Id == train
                                orderby a.Order
                                select new
                                {
                                    c.Name,
                                    ArrivalTime = a.ArrivalTime.ToString(@"HH\:mm"),
                                    DepartureTime = a.DepartureTime.ToString(@"HH\:mm")
                                }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("passingTrains")]
        public async Task<ActionResult> GetPassingTrainsThoughStation(string station)
        {
            var result = await (from rs in _context.RouteStations
                                join r in _context.Routes on rs.Id equals r.Id
                                join t in _context.Trains on r.Id equals t.RouteId
                                where rs.StationId == station
                                select new
                                {
                                    t.Id,
                                    t.Type,
                                    ArrivalTime = rs.ArrivalTime.ToString(@"HH\:mm"),
                                    DepartureTime = rs.DepartureTime.ToString(@"HH\:mm")
                                }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("findTrains")]
        public async Task<ActionResult> FindTrains(string firstStation, string lastStation, DateTime departure)
        {
            string station1Id = firstStation;
            string station2Id = lastStation;
            int dayNunber = departure.Day;

            int dayOfWeek = (int)departure.DayOfWeek == 0 ? 7 : (int)departure.DayOfWeek;
            bool even;

            if (dayNunber % 2 == 0)
            {
                even = true;
            }
            else
            {
                even = false;
            }

            var trainsContainsFirstStation = await (from rs in _context.RouteStations
                                                    join r in _context.Routes on rs.Id equals r.Id
                                                    join t in _context.Trains on r.Id equals t.RouteId
                                                    where rs.StationId == station1Id
                                                    select new
                                                    {
                                                        TrainId = t.Id,
                                                        StationOrder = rs.Order,
                                                        t.Days,
                                                        t.EvenDays,
                                                        t.EveryDay,
                                                        rs.DepartureTime,
                                                        t.Type
                                                    }).ToListAsync();

            var trainsContainsSecondStation = await (from rs in _context.RouteStations
                                                     join r in _context.Routes on rs.Id equals r.Id
                                                     join t in _context.Trains on r.Id equals t.RouteId
                                                     where rs.StationId == station2Id
                                                     select new
                                                     {
                                                         TrainId = t.Id,
                                                         StationOrder = rs.Order,
                                                         rs.ArrivalTime
                                                     }).ToListAsync();

            // номер поезда с уходом с 1 станции и приходом на 2 
            var trainsWithScedule = from a in trainsContainsFirstStation
                                    join b in trainsContainsSecondStation on a.TrainId equals b.TrainId
                                    where a.StationOrder < b.StationOrder
                                    select new
                                    {
                                        a.TrainId,
                                        b.ArrivalTime,
                                        a.DepartureTime,
                                        FirstStationOrder = a.StationOrder,
                                        LastStationOrder = b.StationOrder,
                                        a.Days,
                                        a.EvenDays,
                                        a.EveryDay,
                                        a.Type
                                    };

            var result = from a in trainsWithScedule
                         where a.Days != null && a.Days.Contains(dayOfWeek) || a.EveryDay == true || a.EvenDays != null && a.EvenDays == even
                         select new
                         {
                             a.TrainId,
                             ArrivalTime = a.ArrivalTime.ToString(@"hh\:mm"),
                             DepartureTime = a.DepartureTime.ToString(@"hh\:mm"),
                             RouteTime = (string)(a.ArrivalTime.Subtract(a.DepartureTime)).ToString(@"hh\:mm")
                         };
            return Ok(result);
        }

        static int priceBusinessExpress = 200;
        static int priceEconomExpress = 150;
        static int priceBusinessStandart = 100;
        static int priceEconomStandart = 50;


        Dictionary<Price, int> price = new Dictionary<Price, int>()
        {
            [new Price { CarriageType = "business", TrainType = "express"  }] = priceBusinessExpress,
            [new Price { CarriageType = "econom", TrainType = "express"  }] = priceEconomExpress,
            [new Price { CarriageType = "business", TrainType = "standart" }] = priceBusinessStandart,
            [new Price { CarriageType = "econom", TrainType = "standart" }] = priceEconomStandart
        };

        [HttpGet("findSeats")]
        public async Task<IEnumerable<object>> FindSeats(int trainId, string firstStation, string lastStation, DateTime departureDate)
        {
            var route = await _context.Trains.Where(p => p.Id == trainId).Select(p => p.RouteId).FirstOrDefaultAsync();
            var firstStationOrder = await _context.RouteStations.Where(p => p.Id == route && p.StationId == firstStation).Select(p => p.Order).FirstOrDefaultAsync();
            var lastStationOrder = await _context.RouteStations.Where(p => p.Id == route && p.StationId == lastStation).Select(p => p.Order).FirstOrDefaultAsync();
            int priceCoef = lastStationOrder - firstStationOrder;

            var allTicketsOfTrain = await (from t in _context.Tickets
                                           where t.TrainId == trainId && t.DepartureDate.Year == departureDate.Date.Year && t.DepartureDate.Month == departureDate.Date.Month && t.DepartureDate.Day == departureDate.Date.Day
                                           select new
                                           {
                                               t.Ticketid,
                                               t.TrainId,
                                               t.CarriageId,
                                               t.Seat,
                                               t.ArrivalStationId
                                           }).ToListAsync();

            var allTicketsOfTrainWithOrder = from t in allTicketsOfTrain
                                             join tr in _context.Trains on t.TrainId equals tr.Id
                                             join r in _context.Routes on tr.RouteId equals r.Id
                                             join rs in _context.RouteStations on r.Id equals rs.Id
                                             join c in _context.Carriages on tr.Id equals c.TrainId
                                             where tr.RouteId == t.TrainId && rs.StationId == t.ArrivalStationId
                                             select new
                                             {
                                                 t.Ticketid,
                                                 t.TrainId,
                                                 TrainType = tr.Type,
                                                 CarriageType = c.Type,
                                                 t.CarriageId,
                                                 t.Seat,
                                                 t.ArrivalStationId,
                                                 rs.Order
                                             };

            var allSeats = await (from a in _context.Trains
                                  join b in _context.Carriages on a.Id equals b.TrainId
                                  join c in _context.Seats on b.Id equals c.Id
                                  where a.Id == trainId
                                  select new
                                  {
                                      TrainId = a.Id,
                                      TrainType = a.Type,
                                      CarriageId = b.Id,
                                      CarriageType = b.Type,
                                      Seat = c.SeatNo,
                                  }).ToListAsync();


            var allSeatsWithPrice = allSeats.Select(a => new { TrainId = a.TrainId,
                                                               TrainType = a.TrainType,
                                                               CarriageId = a.CarriageId,
                                                               CarriageType = a.CarriageType,
                                                               Seat = a.Seat,
                                                               //Price = CountPrice(a.CarriageType, a.TrainType) * priceCoef
                                                               Price = price [ new Price { CarriageType = a.CarriageType, TrainType = a.TrainType }] * priceCoef
            });

            var busySeats = (from all in allTicketsOfTrainWithOrder
                             where all.Order >= firstStationOrder
                             select new
                             {
                                 all.TrainId,
                                 all.TrainType,
                                 all.CarriageId,
                                 all.CarriageType,
                                 all.Seat,

                             }).ToList().Select(a => new
                             {
                                 TrainId = a.TrainId,
                                 TrainType = a.TrainType,
                                 CarriageId = a.CarriageId,
                                 CarriageType = a.CarriageType,
                                 Seat = a.Seat,
                                 //Price = CountPrice(a.CarriageType, a.TrainType) * priceCoef
                                 Price = price[new Price { CarriageType = a.CarriageType, TrainType = a.TrainType }] * priceCoef
                             });

            var freeSeats = allSeatsWithPrice.Except(busySeats);

            return freeSeats.Select(a => new { a.TrainId, a.CarriageId, a.CarriageType, a.Seat, a.Price });
        }

        [HttpGet("usertickets")]
        public async Task<ActionResult> FindUsersTickets(int userId)
        {
            var result = await (from t in _context.Tickets
                                where t.UserId == userId
                                select new
                                {
                                    t.Ticketid,
                                    t.TrainId,
                                    t.CarriageId,
                                    t.Seat,
                                    t.Status,
                                    date = (t.DepartureDate.Date).ToString("d")
                                }).ToListAsync();

            return Ok(result);
        }
    }
}
