using Models;
using System;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas às reviews feitas a um Employer
    /// </summary>
    public interface IReviewEmployerDAO : IDisposable
    {
        /// <summary>
        /// Método para criar uma Review ao Employer
        /// Neste método também é possível obter a media das ratings que um employer tenha através da tabela EmployerReview
        /// E de seguida é crucial colocar essa média na tabela de Users associado ao ID do employer respetivo à review
        /// </summary>
        /// <param name="employer">Id do Employer que vai ser feita a review</param>
        /// <param name="model">Modelo com as informações do review a ser publicada</param>
        /// <returns>Retorna a Review criada</returns>
        Review ReviewEmployer(int employer, Review model);
    }
}
