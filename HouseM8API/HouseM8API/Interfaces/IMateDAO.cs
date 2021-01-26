using Enums;
using HouseM8API.Models;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas ao Mate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMateDAO<T> : IDAO<T> where T : class, new()
    {
        /// <summary>
        /// Método para listar as categorias do
        /// mate, através do seu ID
        /// </summary>
        /// <param name="id">Id do Mate a pesquisar as categorias</param>
        /// <returns>Todas as categorias correspondentes ao Mate</returns>
        Collection<CategoryModel> CategoriesList(int id);

        /// <summary>
        /// Método para adicionar categorias de trabalho
        /// ao Mate
        /// </summary>
        /// <param name="id">Id do Mate</param>
        /// <param name="category">Categoria(s) a adicionar</param>
        void AddCategory(int id, CategoryModel[] category);

        /// <summary>
        /// Método para remover uma categoria de trabalho
        /// do Mate
        /// </summary>
        /// <param name="id">Id do Mate</param>
        /// <param name="category">Categoria a remover</param>
        void RemoveCategory(int id, CategoryModel category);

        /// <summary>
        /// Metodo que retorna mates baseado 
        /// nos filtros que entrem como paramentros
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="rank"></param>
        /// <param name="distance"></param>
        /// <param name="address"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        List<Mate> GetMates(Categories[] categories, string address, Ranks? rank, int? distance, int? rating);

        /// <summary>
        /// Método para obter um Mate através do seu ID
        /// </summary>
        /// <param name="id">Id do Mate a pesquisar</param>
        /// <returns>Objeto Mate com os dados do Mate que
        /// foi pesquisado</returns>
        Mate FindMateById(int id);

        /// <summary>
        /// Metodo para ignorar um jobPost
        /// </summary>
        /// <param name="mateId"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        bool IgnoreJobPost(int mateId, IgnoredJobModel job);

        /// <summary>
        /// Método para listar todos os trabalhos pendentes
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Coleção de trabalhos pendentes</returns>
        Collection<PendingJobModel> PendingJobsList(int id);

        /// <summary>
        /// Método para atualizar o rank de um Mate
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="MateId"></param>
        /// <returns>True ou Exception</returns>
        bool UpdateRank(Ranks rank, int MateId);
    }
}
