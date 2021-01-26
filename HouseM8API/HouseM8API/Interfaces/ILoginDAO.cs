using HouseM8API.Entities;
using System;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas ao Login
    /// </summary>
    public interface ILoginDAO : IDisposable
    {
        /// <summary>
        /// Método para autenticar um User
        /// </summary>
        /// <param name="email">O email do utilizador a ser autenticado</param>
        /// <param name="password">A password do utilizador a ser autenticado</param>
        /// <returns>Retorna o User autenticado</returns>
        User Authenticate(string email, string password);
    }
}
