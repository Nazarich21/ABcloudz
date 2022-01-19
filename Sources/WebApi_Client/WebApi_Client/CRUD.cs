using api.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApi_Client
{
    public class CRUD
    {
        const string APP_PATH = "http://192.168.12.88:80";

        public static async Task GetRequest(string ID)
        {
            switch (ID)
            {
                case "GETALL":
                    Console.Clear();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APP_PATH);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response;
                        response = await client.GetAsync("api/bottles");

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                        }
                    }
                    break;
                //Get Request    
                case "GET":
                    Console.Clear();
                    Console.WriteLine("Enter id:");
                    int id = 0;
                    try
                    {
                        id = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\n***  wrong input data***");
                        Console.WriteLine("Type: {0}", e.GetType().ToString());
                        Console.WriteLine("Message: {0}", e.Message);
                        Console.WriteLine("Source: {0}", e.Source);
                        break;
                    }
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APP_PATH);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response;

                        response = await client.GetAsync("api/bottles/" + id);
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                        }
                        else
                        {
                            Console.WriteLine("id does not exist");
                        }
                    }
                    break;

                //Post Request    
                case "POST":
                    Bottle bottlePost = new Bottle();
                    Console.Clear();
                    try
                    {
                        Console.WriteLine("Enter material and press ENTER.");
                        bottlePost.Material = Console.ReadLine();
                        Console.WriteLine("Enter kindOfDrink and press ENTER.");
                        bottlePost.KindOfDrink = Console.ReadLine();
                        Console.WriteLine("Enter size and press ENTER. Example-'1.5'");
                        bottlePost.Size = Convert.ToDouble(Console.ReadLine());

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\n***  wrong input data***");
                        Console.WriteLine("Type: {0}", e.GetType().ToString());
                        Console.WriteLine("Message: {0}", e.Message);
                        Console.WriteLine("Source: {0}", e.Source);
                        break;
                    }
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APP_PATH);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.PostAsJsonAsync("api/bottles", bottlePost);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Data succesfully added");
                        }
                        else
                        {
                            Console.WriteLine("An error has occurred, check input data");
                        }
                    }

                    break;

                //Delete Request    
                case "DELETE":
                    Console.Clear();
                    Console.WriteLine("Enter id:");
                    int delete = 0; //Convert.ToInt32(Console.ReadLine());
                    try
                    {
                        delete = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\n***  wrong input data***");
                        Console.WriteLine("Type: {0}", e.GetType().ToString());
                        Console.WriteLine("Message: {0}", e.Message);
                        Console.WriteLine("Source: {0}", e.Source);
                        break;
                    }
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APP_PATH);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.DeleteAsync("api/bottles/" + delete);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Record Deleted");
                        }
                        else
                        {
                            Console.WriteLine("An error has occurred, id does not exist");
                        }
                    }

                    break;

                case "PUT":
                    Console.Clear();
                    Bottle bottlePut = new Bottle();
                    string put = null;
                    try
                    {
                        Console.WriteLine("Enter id and press ENTER.");
                        put = Console.ReadLine();
                        Console.WriteLine("Enter material and press ENTER.");
                        bottlePut.Material = Console.ReadLine();
                        Console.WriteLine("Enter kindOfDrink and press ENTER.");
                        bottlePut.KindOfDrink = Console.ReadLine();
                        Console.WriteLine("Enter size and press ENTER.");
                        bottlePut.Size = Convert.ToDouble(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\n***  wrong input data***");
                        Console.WriteLine("Type: {0}", e.GetType().ToString());
                        Console.WriteLine("Message: {0}", e.Message);
                        Console.WriteLine("Source: {0}", e.Source);
                        break;
                    }
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(APP_PATH);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.PutAsJsonAsync("api/bottles/" + put, bottlePut);
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Data successfully modified");

                        }
                        else
                        {
                            Console.WriteLine("An error has occurred,id does not exist");
                        }
                    }

                    break;
            }
        }
    }
}
