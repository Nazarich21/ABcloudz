using System;
using System.Collections.Generic;



namespace Sum_of_sequence
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your numbers in exponential notation (a.aaaaae+num) one by one and press 'enter' button after each number, enter '=' if you want to count sum ");
            Console.WriteLine("Number example: 4.22222e+10");
            List<Exponent> numbersList = new List<Exponent>();
            bool func = true;
            Exponent result;
            while (func)
            {
                try
                {
                    string temp = Console.ReadLine();
                    if (temp == "=")
                    {
                        func = false;
                    }
                    else
                    {
                        numbersList.Add(new Exponent(temp));
                    }
                }
                catch
                {

                Console.WriteLine("Oops, something went wrong. Check input format, example: 4.22222e+10");

                }
            }

            if (numbersList.Count == 0)
            {
                Console.WriteLine("Result: 0 ");
            }
            else if (numbersList.Count == 1)
            {
               
                Console.WriteLine(numbersList[0].ToString());
            }
            else
            {
                try
                {
                    Exponent res = Exponent.Zero;

                    foreach (Exponent str in numbersList)
                    {
                        res += str;
                    }
                    Console.WriteLine(res.ToString());

                   
                }
                catch
                {
                    Console.WriteLine("Oops, something went wrong. Check input format, example: 4.22222e+10");
                }
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
           


        }

    }



}
