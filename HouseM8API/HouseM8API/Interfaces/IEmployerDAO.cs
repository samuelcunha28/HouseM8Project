using HouseM8API.Models;
using Models;
using System.Collections.ObjectModel;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas ao Employer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEmployerDAO<T> : IDAO<T> where T : class, new()
    {
        /// <summary>
        /// Método que retorna uma coleção de mates favoritos
        /// do employer correspondente ao ID de parametro do método
        /// </summary>
        /// <param name="id">Id do employer que possui os mates favoritos</param>
        /// <returns>Coleção de Mates favoritos</returns>
        Collection<FavoriteModel> FavoritesList(int id);

        /// <summary>
        /// Método para adicionar um mate aos favoritos de um Employer
        /// </summary>
        /// <param name="emID">Id do Employer</param>
        /// <param name="mateID">Id do Mate a ser adicionado</param>
        /// <returns>Retorna o Id do Mate adicionado</returns>
        int? AddFavorite(int emID, int mateID);

        /// <summary>
        /// Método para remover um mate dos favoritos
        /// </summary>
        /// <param name="employerId">Id do Employer com favoritos</param>
        /// <param name="mateId">Id do Mate a ser Removido</param>
        /// <returns>Retorna o Id do mate removido</returns>
        int? RemoveFavorite(int employerId, int mateId);

        /// <summary>
        /// Método para encontrar um Employer por Id
        /// </summary>
        /// <param name="id">Id do Employer a procurar</param>
        /// <returns>Retorna um objeto Employer com 
        /// os dados do Employer pretendido</returns>
        Employer FindEmployerById(int id);
    }
}
