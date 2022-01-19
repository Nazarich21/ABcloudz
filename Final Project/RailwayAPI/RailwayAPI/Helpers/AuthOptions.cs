using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayAPI.Helpers
{
    public class AuthOptions
    {
        //public string Issuer { get; set; }
        //public string Audience { get; set; }
        //public string Secret { get; set; }
        //public int TokenLifeTime { get; set; }
        //public SymmetricSecurityKey GetSymmetricSecurityKey()
        //{
        //    return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        //}
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 120; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

