using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Entity
{
    class Program
    {
        static void Main(string[] args)
        {
          MainMenu();
          Console.Read();
        }

        public static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose table :");
            Console.WriteLine("1) Pupil");
            Console.WriteLine("2) Class");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    PupilMenu();
                    return true;
                case "2":
                    ClassMenu();
                    return true;
                case "3":
                    Environment.Exit(0);
                    return false;
                default:
                    return true;
            }
        }

        public static bool PupilMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose option:");
            Console.WriteLine("1) Show");
            Console.WriteLine("2) Add");
            Console.WriteLine("3) Edit");
            Console.WriteLine("4) Delete");
            Console.WriteLine("5) Exit");
            Console.Write("\r\nSelect an option: "); 

            switch (Console.ReadLine())
            {
                case "1":
                    Pupil.Show();
                    return true;
                case "2":
                    Pupil.Add();
                    return true;
                case "3":
                    Pupil.Edit();
                    return false;
                case "4":
                    Pupil.Delete();
                    return false;
                case "5":
                    Environment.Exit(0);
                    return false;
                default:
                    return true;
            }
        }

        public static bool ClassMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose option:");
            Console.WriteLine("1) Show");
            Console.WriteLine("2) Add");
            Console.WriteLine("3) Edit");
            Console.WriteLine("4) Delete");
            Console.WriteLine("5) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Class.Show();
                    return true;
                case "2":
                    Class.Add();
                    return true;
                case "3":
                    Class.Edit();
                    return false;
                case "4":
                    Class.Delete();
                    return false;
                case "5":
                    Environment.Exit(0);
                    return false;
                default:
                    return true;
            }
        }
    }
}
