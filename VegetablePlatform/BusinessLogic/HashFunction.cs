using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography; //必要
using System.Text; //必要

namespace VegetablePlatform.BusinessLogic
{
    public class HashFunction
    {
        public static string Hashfun(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] password_byte = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_byte);
            return Convert.ToBase64String(encrypted_bytes);
        }
    }
}