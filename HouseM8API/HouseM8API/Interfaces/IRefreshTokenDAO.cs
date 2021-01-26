using HouseM8API.Models;

namespace HouseM8API.Interfaces
{
    public interface IRefreshTokenDAO
    {   
        /// <summary>
        /// Método para guardar o token refresh encriptado na BD
        /// </summary>
        /// <param name="refreshToken">Refesh token</param>
        /// <param name="email">Email do utilizador</param>
        /// <returns>Retorna True se for guardado, 
        /// False caso contrário</returns>
        bool saveEncryptedRefreshToken(string refreshToken, string email);

        /// <summary>
        /// Método para atualizar o token refresh guardado na BD
        /// </summary>
        /// <param name="refreshToken">Refesh token</param>
        /// <param name="email">Email do utilizador</param>
        /// <returns>Retorna True se for atualizado,
        /// False caso contrário</returns>
        bool updateEncryptedRefreshToken(string refreshToken, string email);

        /// <summary>
        /// Método para retornar o token refresh encriptado de 
        /// um utilizador com o email de argumento
        /// </summary>
        /// <param name="email">Email do utilizador</param>
        /// <returns>Retorna um objeto EncryptedRefreshTokenModel,
        /// caom a Hash, salt do token e email do user </returns>
        EncryptedRefreshTokenModel GetEncryptedRefreshTokenModel(string email);
    }
}