using HouseM8API.Entities;
using System;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas ao Report de utilizadores
    /// </summary>
    public interface IReportDAO : IDisposable
    {
        /// <summary>
        /// Método para criar um Report a um User
        /// </summary>
        /// <param name="id">Id do User irá reportar</param>
        /// <param name="user">Utilizador a ser reportado</param>
        /// <param name="model">Modelo com as informações do report a ser publicado</param>
        /// <returns>Retorna o Report criado</returns>
        Report ReportUser(int id, int user, Report model);
    }
}
