using HouseM8API.Entities;
using HouseM8API.Models;
using System;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface para efetuar operações na base de dados
    /// relativas ao Utilizador
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUserDAO<T> : IDisposable where T : class, new()
    {
        /// <summary>
        ///  Método que procura um utilizador
        /// através do seu email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>O utilizador encontrado</returns>
        User FindUserByEmail(string email);

        /// <summary>
        /// Método que procura um utilizador
        /// através do seu id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        User FindById(int userId);

        /// <summary>
        /// Método que atualiza a password
        /// de um determindado utilizador
        /// </summary>
        /// <param name="newPass"></param>
        /// <param name="id"></param>
        /// <returns>
        /// True caso a password seja atualizada com sucesso
        /// False caso contrário
        /// </returns>
        bool UpdatePassword(PasswordUpdateModel newPass, int? id);

        /// <summary>
        /// Método que retorna o nome da imagem de perfil
        /// do user
        /// </summary>
        /// <param name="id">Id do user</param>
        /// <returns>Retorna o nome da Imagem de perfil</returns>
        ImageName getProfileImage(int id);

        /// <summary>
        /// Método que apaga a imagem de perfil do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador</param>
        /// <returns>Retorna True se a imagem é apagada, False caso contrário</returns>
        bool deleteImage(int id);
    }
}
