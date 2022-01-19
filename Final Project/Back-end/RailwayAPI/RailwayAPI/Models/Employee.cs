using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

#nullable disable

namespace RailwayAPI
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public string HomeAdress { get; set; }
        public JsonDocument Phones { get; set; }
        public string Position { get; set; }
        public string  Photo { get; set; }
    }
   
}
