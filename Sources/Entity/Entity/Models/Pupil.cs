using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Entity
{
    public partial class Pupil
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Class { get; set; }

        public virtual Class ClassNavigation { get; set; }
        public static void Show()
        {
            Console.Clear();
            using (entityContext db = new entityContext())
            {
                var pupils = db.Pupils.ToList();

                foreach (var pupil in pupils)
                {
                    Console.WriteLine($"{pupil.Id}. Name:{pupil.Name} Surname:{pupil.Surname} Class:{pupil.Class}.");
                }
            }

        }
        public static void Add()
        {
            Console.Clear();
            Console.WriteLine("Enter 'Name':");
            string name = Console.ReadLine();
            Console.WriteLine("Enter 'Surname':");
            string surname = Console.ReadLine();
            Console.WriteLine("Enter 'Class':");
            int clas = Convert.ToInt32(Console.ReadLine());


            using (entityContext db = new entityContext())
            {
                try
                {
                    Pupil add = new Pupil();
                    int max = db.Pupils.Max(u => u.Id);
                    add.Id = max + 1;
                    add.Name = name;
                    add.Surname = surname;
                    add.Class = clas;
                    db.Pupils.Add(add);
                    db.SaveChanges();
                }
                catch
                {
                    throw new ArgumentException("Primary key dublicate");
                }
            }
        }
        public static void Delete()
        {
            Console.Clear();
            Console.WriteLine("Write down pupil id ");
            int pupilId = Convert.ToInt32(Console.ReadLine());

            using (entityContext db = new entityContext())
            {
                try
                {
                    Pupil delete = db.Pupils.Find(pupilId);
                    db.Pupils.Remove(delete);
                    db.SaveChanges();
                }
                catch
                {
                    throw new ArgumentException("There is no such record");
                }
            }

        }
       
        public static void Edit()
        {
            Console.Clear();
            Console.WriteLine("Enter Id of record for modification.");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Press 1 to edit 'Name' or 2 to edit 'Surname' and 3 to edit 'Class'.");
            var propery = Console.ReadKey();
            Console.WriteLine("Enter new value:");
            string newValue = Console.ReadLine();


            using (entityContext db = new entityContext())
            {
                Pupil p = db.Pupils.Find(id);
                if (p != null)
                {
                    switch (propery.KeyChar)
                    {
                        case '1': p.Name = newValue; break;
                        case '2': p.Surname = newValue; break;
                        case '3': p.Class = Convert.ToInt32(newValue); break;
                        default: throw new ArgumentException("There is no such property");
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
