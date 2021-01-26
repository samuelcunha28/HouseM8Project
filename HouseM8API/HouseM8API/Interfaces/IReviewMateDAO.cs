using HouseM8API.Models;
using Models;
using System;
using System.Collections.Generic;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas às reviews feitas a um Mate
    /// </summary>
    public interface IReviewMateDAO : IDisposable
    {
        /// <summary>
        /// Método para criar uma Review ao Mate
        /// Neste método também é possível obter a media das ratings que um mate tenha através da tabela MateReview
        /// E de seguida é crucial colocar essa média na tabela de Users associado ao ID do mate respetivo à review
        /// </summary>
        /// <param name="employer">ID do employer autor da review</param>
        /// <param name="mate">Id do Mate que vai ser feita a review</param>
        /// <param name="model">Modelo com as informações do review a ser publicada</param>
        /// <returns>Retorna o MateReview criado</returns>
        MateReview ReviewMate(int employer, int mate, MateReview model);

        /// <summary>
        /// Método que retorna a lista de reviews feitas ao mate
        /// </summary>
        /// <param name="mate">Id do mate</param>
        /// <returns>Lista de reviews</returns>
        List<MateReviewsModel> MateReviewsList(int mate);
    }
}
