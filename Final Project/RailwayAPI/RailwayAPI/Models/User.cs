using System;
using System.Collections.Generic;

#nullable disable

namespace RailwayAPI
{
    public partial class User
    {
        public int id { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public bool Blocked { get; set; } 

    }
}
