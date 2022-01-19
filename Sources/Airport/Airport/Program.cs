using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Airport
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Extension search");
            Console.WriteLine("Enten airport code and press ENTER UUS");
            string ac = Console.ReadLine();
            ExtensionSearch(ac);
            Console.WriteLine();
            Console.WriteLine("Sql search");
            Console.WriteLine("Amount of Booked seats by date");
            Console.WriteLine("Enten date for search and press ENTER. Example-'2017-07-05'");
            string ac1 = Console.ReadLine();
            SqlSearch(ac1);
            Console.WriteLine();

            Console.WriteLine("Amount of planes by model.");
            PlanesCount(); Console.WriteLine();

            Console.WriteLine("Amount of passengers.");
            Passengers(); Console.WriteLine();

            Console.WriteLine("Amount of routs with duration between 2 and 5 hours .");
            FlightsBerweenTwoAndFive(); Console.WriteLine();

            Console.WriteLine("Airports above 55 latitude");
            Above55Latitude(); Console.WriteLine();

            Console.WriteLine("Flights on wednesday");
            FlightsOnWednesday(); Console.WriteLine();

            Console.WriteLine("Amount of airports by city");
            AirportsByCity(); Console.WriteLine();

            Console.WriteLine("Amount of short-haul planes");
            ShortHaulPlanes(); Console.WriteLine();

            Console.WriteLine("Amount of mid-haul planes");
            MidHaulPlanes(); Console.WriteLine();

            Console.WriteLine("Amount of long-haul planes");
            LongHaulPlanes();
            Console.ReadLine();

        }
        public static void ExtensionSearch(string airportCode)
        {
            var airports = demoContext.Instance.Airports.Select(p => p.AirportCode).ToList();

            if (airports.Contains(airportCode))
            {
                var airplanes = demoContext.Instance.Flights
                    .Where(a => a.ArrivalAirport == airportCode || a.DepartureAirport == airportCode)
                    .Select(p => p.AircraftCode)
                    .Distinct()
                    .ToList();

                Console.WriteLine("Plane codes:");
                foreach (var air in airplanes)
                {
                    Console.WriteLine($" {air}");
                }
            }
            else
            {
                Console.WriteLine("There is no such airport");
            }
        }
        public static void SqlSearch(string bookDate)
        {
            try
            {
                DateTime dt = DateTime.Parse(bookDate);
                var param1 = new NpgsqlParameter("start_date", NpgsqlTypes.NpgsqlDbType.TimestampTz);
                param1.Value = dt;
                var param2 = new NpgsqlParameter("end_date", NpgsqlTypes.NpgsqlDbType.TimestampTz);
                param2.Value = dt.AddDays(1);
                var sum = demoContext.Instance.Bookings.
                    FromSqlRaw("SELECT * FROM bookings.bookings where book_date BETWEEN @start_date AND @end_date", param1, param2).
                    Sum(p => p.TotalAmount);
                if (true)
                {
                    Console.WriteLine($"Total amount of bookings: {sum}");
                }
                else
                {
                    Console.WriteLine("There is no bookings on this date");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n***  wrong input data***");
                Console.WriteLine("Type: {0}", e.GetType().ToString());
                Console.WriteLine("Message: {0}", e.Message);
                Console.WriteLine("Source: {0}", e.Source);

            }
        }

        public static void PlanesCount()
        {
            var query = demoContext.Instance.AircraftsData
               .GroupBy(p => p.Model)
               .Select(g => new { name = g.Key, count = g.Count() });
            foreach (var q in query)
            {
                Console.WriteLine($"{q.count}   {q.name}");
            }
        }

        public static void FlightsBerweenTwoAndFive()
        {
            var count = demoContext.Instance.FlightsVs.FromSqlRaw("select distinct (f.flight_no) from bookings.flights f where  (f.scheduled_arrival - f.scheduled_departure) between '2 hours'::interval and '5 hours'::interval");
            int res = count.Count();
            Console.WriteLine(res);
        }
        public static void FlightsOnWednesday()
        {
            var flights1 = demoContext.Instance.Routes
                 .Where(g => g.DaysOfWeek.Contains(3));
            foreach (var f in flights1)
            {
                Console.WriteLine(f.FlightNo);
            }
        }

        public static void Above55Latitude()
        {
            var airports = demoContext.Instance.AirportsData.FromSqlRaw("select*from bookings.airports_data ad where  ad.coordinates[1]>55");
            foreach (var air in airports)
            {
                Console.WriteLine(air.AirportName);
            }
        }

        public static void Passengers()
        {
            var people = demoContext.Instance.Tickets
                .Select(p => p.PassengerId.Distinct()).Count();
            Console.WriteLine(people);
        }

        public static void AirportsByCity()
        {
            var sel = demoContext.Instance.AirportsData
                .GroupBy(p => p.City)
                .Select(g => new { Name = g.Key, Count = g.Count() });
            foreach (var s in sel)
            {
                Console.WriteLine($"{s.Count}  {s.Name}");
            }
        }

        public static void ShortHaulPlanes()
        {
            var planes = demoContext.Instance.AircraftsData
                .Where(p => p.Range < 2500).Count();
            Console.WriteLine(planes);
        }

        public static void MidHaulPlanes()
        {
            var planes = demoContext.Instance.AircraftsData
                .Where(p => p.Range > 2500 && p.Range < 6000).Count();
            Console.WriteLine(planes);
        }

        public static void LongHaulPlanes()
        {
            var planes = demoContext.Instance.AircraftsData
                .Where(p => p.Range > 6000).Count();
            Console.WriteLine(planes);
        }
    }
}
