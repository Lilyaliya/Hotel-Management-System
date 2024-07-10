using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace Hotel.Functions
{
    public class Hash
    {
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hasBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hasBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
    
}