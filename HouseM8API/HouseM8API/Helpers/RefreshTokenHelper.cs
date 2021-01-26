using System;
using System.Security.Cryptography;

namespace HouseM8API.Helpers
{
    public class RefreshTokenHelper
    {
        /// <summary>
        /// Método para gerar um novo refresh token
        /// </summary>
        /// <returns>Retorna o refresh token</returns>
        public static string generateRefreshToken(){
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        
    }
}