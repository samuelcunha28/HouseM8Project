using HouseM8API.Models;
using Models;
using System;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas ao Trabalho
    /// </summary>
    public interface IWorkDAO : IDisposable
    {
        /// <summary>
        /// Método que cria um trabalho com data, jobpost, status 
        /// e os seus participantes
        /// </summary>
        /// <param name="id">Id do Employer que pretende criar o Job</param>
        /// <param name="model">Modelo do Trabalho com a informação pretendida</param>
        /// <returns>O modelo de trabalho com a informação pretendida</returns>
        Job Create(int id, Job model);

        /// <summary>
        /// Método para encontrar um trabalho por Id
        /// </summary>
        /// <param name="id">Id do trabalho pretendido</param>
        /// <returns>Retorna o trabalho pretendido, se nao existir
        /// retorna null </returns>
        WorkDetailsModel FindById(int id);

        /// <summary>
        /// Método para confirmar o trabalho como realizado,
        /// removendo-o posteriormente da lista de trabalhos
        /// pendentes do Mate
        /// </summary>
        /// <param name="jobId">Id do trabalho a marcar como concluído</param>
        /// <param name="userId">Id do utilizador que marca o trabalho como 
        /// concluído</param>
        /// <returns>Verdadeiro em caso de sucesso, falso caso contrário</returns>
        bool MarkJobAsDone(int jobId, int userId);

        /// <summary>
        /// Overload do FindById para procurar um work com o employer
        /// na sessão
        /// </summary>
        /// <param name="workId">Id do work</param>
        /// <param name="employerId">Id do employer</param>
        /// <returns>Retorna o Work</returns>
        WorkDetailsModel FindById(int workId, int employerId);
        
        /// <summary>
        /// Método para apagar um Work
        /// </summary>
        /// <param name="toDelete">Id do work a apagar</param>
        /// <returns>Retorna True caso o work seja apagado,
        /// False caso contrário</returns>
        bool Delete(int toDelete);

        /// <summary>
        /// Método para atualizar a data de um work
        /// </summary>
        /// <param name="workId">Id do work</param>
        /// <param name="date">Objeto DateModel com a data</param>
        /// <returns>Retorna true se a data for atualizada,
        /// false caso contrário </returns>
        bool updateDate(int workId, DateModel date);
    }
}
