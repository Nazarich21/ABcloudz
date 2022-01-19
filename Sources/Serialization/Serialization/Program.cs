
using System;
using System.Collections.Generic;
using System.IO;


namespace Serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProgramOptions option1 = new ProgramOptions(args);

                if (option1.Serialize == true)
                {
                    try
                    {
                        var directory = new DirectoryInfo(option1.Catalog);
                        var items = Node.SearchDirectory(directory);
                        Serialization.Serialize(items, option1.FileName, option1.Format);
                        DisplaySuccess();
                    }
                    catch
                    {
                        Console.WriteLine("Oops, something went wrong. Check input data Example: C:/Users/nazarenko/ Filename json ");
                    }
                }
                else if (option1.Serialize == false)
                {
                    try
                    {
                        List<Node> resList = Serialization.Deserialize(option1.FileName, option1.Format);
                        Console.WriteLine(" ");
                        Console.WriteLine($"{option1.Format} deserialization");
                        Console.WriteLine(" ");
                        Output(resList);
                    }
                    catch
                    {
                        Console.WriteLine("Oops, something went wrong. Check please input data, it must be file name with extension. Example: file.json");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Wrong input type.");
                Console.WriteLine("Input for serialization. Example : C:/Users/nazarenko/File.json ");
                Console.WriteLine("Input for deserialization.  Example : C:/Users/nazarenko/File.json");
            }

            Console.WriteLine("Press any key to exit ");
            Console.ReadKey();

        }
        public static void DisplaySuccess()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Data has been  successfully serializated");
            Console.WriteLine("File with data you can find in /bin/Debug/net5.0 folder");

        }
        public static void Output(List<Node> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
