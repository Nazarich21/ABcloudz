using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApi_Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Menu();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public enum httpVerb
        {
            GET,
            GETALL,
            POST,
            PUT,
            DELETE,

        }

        public static async Task Menu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Choose option:");
                Console.WriteLine("1) GET");
                Console.WriteLine("2) POST");
                Console.WriteLine("3) GET{id}");
                Console.WriteLine("4) PUT");
                Console.WriteLine("5) Delete");
                Console.WriteLine("6) Exit");
                Console.Write("\r\nSelect an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        await CRUD.GetRequest(httpVerb.GETALL.ToString());
                        break;
                    case "2":
                        await CRUD.GetRequest(httpVerb.POST.ToString());
                        break;
                    case "3":
                        await CRUD.GetRequest(httpVerb.GET.ToString());
                        break;
                    case "4":
                        await CRUD.GetRequest(httpVerb.PUT.ToString());
                        break;
                    case "5":
                        await CRUD.GetRequest(httpVerb.DELETE.ToString());
                        break;
                    case "6":
                        return;
                    default:
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey(true);
            } while (true);
        }
    }
}
