using System.Text;
using System.Dynamic;
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace JWTAuthentication.Helpers
{
    public static class Helper {

        private static string saltString = "UF8Ku+ta28jdocWI7RVoRg==";

        public static string ConvertPasswordToHash(string password)
        {

            /* // generate a 128-bit salt using a secure PRNG
            var salt = Encoding.UTF8.GetBytes(saltString);
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
 
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}"); */
        
            var passwordSalt =  saltString + password;
            var passwordBytes = Encoding.UTF8.GetBytes(passwordSalt);
            string hashed = Convert.ToBase64String(passwordBytes);
            Console.WriteLine($"Hashed: {hashed}");

            return hashed;
        }

        public static string ConvertHashToPassword(string hash)
        {
            var base64EncodeBytes = Convert.FromBase64String(hash);
            var password = Encoding.UTF8.GetString(base64EncodeBytes);
            var result = password.Substring(saltString.Length);
            Console.WriteLine($"Password: {result}");

            return password;
        }

    }
}