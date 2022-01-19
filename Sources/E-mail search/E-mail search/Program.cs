using System;
using System.IO;
using System.Net;
using System.Threading;


namespace E_mail_search 
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press 1 for search in file or 2 for website ");
            ConsoleKeyInfo result = Console.ReadKey();

            if (result.KeyChar.ToString() == "1")
            {
                Console.WriteLine("Enter absolute path and press enter button");
                string link = Console.ReadLine();
                try
                {
                    if (File.Exists(link))
                    {
                        string data = File.ReadAllText(link);
                        SearchMain(data);
                    }
                    else
                    {
                        Console.WriteLine("File does not exist, check input. Example: C:/Users/nazarenko.a/Desktop/Test/Doc1.txt");
                    }
                }
                catch
                {
                    Console.WriteLine("Oops, something went wrong, check input data. Example: C:/Users/nazarenko.a/Desktop/Test/Doc1.txt");
                }
            }
            else if (result.KeyChar.ToString() == "2")
            {
                try
                {
                    Console.WriteLine("Enter link for searching and press enter button");
                    string link = Console.ReadLine();
                    WebClient client = new WebClient();
                    string data = client.DownloadString(link);

                    SearchMain(data);
                }
                catch
                {
                    Console.WriteLine("Oops, something went wrong, check input data. Example: https://www.site.com/");
                }
            }
            else
            {
                Console.WriteLine("Wrong number");
            }
        }

        public static void SearchMain(string data)
        {
            Search serchemail = Search.Email(data);
            serchemail.NewResult += Serchemail_NewResult;
          

            Search serchlink = Search.Link(data);
            serchlink.NewResult += Serchemail_NewResult;

            Thread myThreadEmail = new Thread(new ParameterizedThreadStart(serchemail.SearchObj));
            myThreadEmail.Start(serchemail);


            Thread myThreadLink = new Thread(new ParameterizedThreadStart(serchlink.SearchObj));
            myThreadLink.Start(serchlink);

            myThreadEmail.Join();
            myThreadLink.Join();

            if (serchlink.Length == 0 && serchemail.Length == 0)
            {
                Console.WriteLine("There is no links and e-mails ");
            }
        }

        public static void Serchemail_NewResult(object sender, CustomEventArgs args)
        {
            Console.WriteLine($"{args.Nameev} has been found: {args.Valueev}");
        }
    }
}
 