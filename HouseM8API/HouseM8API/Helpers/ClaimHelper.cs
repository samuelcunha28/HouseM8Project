using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HouseM8API.Helpers
{
    public class ClaimHelper
    {
        /// <summary>
        /// Método para obter o id do utilizador através do token
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns>Id do User</returns>
        public static int? GetIdFromClaimIdentity(ClaimsIdentity claimsIdentity)
        {
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type.Equals(ClaimTypes.SerialNumber))
                {
                    return int.Parse(claim.Value);
                }
            }

            return null;
        }

        /// <summary>
        /// Método para obter a role do utilizador através do token
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns>Role do User</returns>
        public static Roles? GetRoleFromClaimIdentity(ClaimsIdentity claimsIdentity)
        {
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type.Equals(ClaimTypes.Role))
                {
                    return (Roles)Enum.Parse(typeof(Roles), claim.Value);
                }
            }

            return null;
        }

        /// <summary>
        /// Método para obter o email do utilizador através do token
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns>Email do User</returns>
        public static string GetEmailFromClaimIdentity(ClaimsIdentity claimsIdentity)
        {
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type.Equals(ClaimTypes.Email))
                {
                    return claim.Value;
                }
            }

            return null;
        }
    }
}
