using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WindSpeed
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            string url = @"http://api.aerisapi.com/observations/kharkiv,?client_id=Um44SiagDc91Dj153tZDQ&client_secret=bE65GvB4SSkxvvO0Cca18exgbmloGhS0iIRT8P5Q";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                var jsonString = await response.Content.ReadAsStringAsync();
                JObject resultJson = (JObject)JsonConvert.DeserializeObject<object>(jsonString);

                int windSpeed = (int)resultJson["response"]["ob"]["windSpeedKPH"];
                string city = (string)resultJson["response"]["place"]["name"];

                if (windSpeed > 8)
                {
                    Console.WriteLine($"The weather in {city} is too windy for bycicling");
                }
                else
                {
                    Console.WriteLine($"The weather in {city} is perfect for bycicling");
                }
                Console.ReadKey();
            }
        }
    }
}
