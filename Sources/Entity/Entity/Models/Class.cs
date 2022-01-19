using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Entity
{
    public partial class Class
    {
        public Class()
        {
            Pupils = new HashSet<Pupil>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; }

        public virtual ICollection<Pupil> Pupils { get; set; }

        public static void Add()
        {
            Console.Clear();
            Console.WriteLine("Enter 'Name':");
            string name = Console.ReadLine();
            Console.WriteLine("Enter 'RoomNumber':");
            int roomNumber = Convert.ToInt32(Console.ReadLine());

            using (entityContext db = new entityContext())
            {
                try
                {
                    Class add = new Class();
                    int max = db.Classes.Max(u => u.Id);
                    add.Id = max + 1;
                    add.Name = name;
                    add.RoomNumber = roomNumber;
                    db.Classes.Add(add);
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
            Console.WriteLine("Write down class id ");
            int classId = Convert.ToInt32(Console.ReadLine());

            using (entityContext db = new entityContext())
            {
                try
                {
                    Class delete = db.Classes.Find(classId);
                    db.Classes.Remove(delete);
                    db.SaveChanges();
                }
                catch 
                {
                    throw new ArgumentException("There is no such record");
                }
            }
        }
        
        public static void Show() 
        {
            Console.Clear();
            using (entityContext db = new entityContext())
            {
                var classes = db.Classes.ToList();

                foreach (var clas in classes) 
                {
                    Console.WriteLine($"{clas.Id}. RoomNumber:{clas.RoomNumber} Name:{clas.Name}.");
                }
            }

        }

        public static void Edit()
        {
            Console.Clear();
            Console.WriteLine("Enter Id of record for modification");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Press 1 to edit 'Name' or 2 to edit 'RoomNumber'.");
            var propery = Console.ReadKey();
            Console.WriteLine("Enter new value");
            string newValue = Console.ReadLine();

            using (entityContext db = new entityContext())
            {
                Class p = db.Classes.Find(id);
                db.SaveChanges();
                if (p != null)
                {
                    switch (propery.ToString())
                    {
                        case "1": p.Name = newValue; break;
                        case "2": p.RoomNumber = Int32.Parse(newValue); break;
                        default: throw new ArgumentException("There is no such property");
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
