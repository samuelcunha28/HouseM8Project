using System;

namespace HouseM8API.Interfaces
{
    public interface IDAO<T> : IDisposable where T : class, new()
    {
        /// <summary>
        /// Método para criar um utilizador
        /// </summary>
        /// <param name="model">Utilizador a registar</param>
        /// <returns>O utilizadro criado</returns>
        T Create(T model);

        /// <summary>
        /// Método para atualizar dados pessoais de um utilizador
        /// </summary>
        /// <param name="model">Modelo com os parametros do utilizador</param>
        /// <param name="id">Id do utilizador a ser atualizado</param>
        /// <returns>Utilizador com os dados atualizados</returns>
        T Update(T model, int id);
    }
}
