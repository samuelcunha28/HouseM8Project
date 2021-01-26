using System;
using System.Data;
using System.Data.SqlClient;
using HouseM8API.Entities;
using HouseM8API.Interfaces;
using HouseM8API.Models;

namespace HouseM8API.Data_Access
{
    public class RefreshTokenDAO : IRefreshTokenDAO
    {
        private readonly IConnection _connection;

        /// <summary>
        /// Método construtor do RefreshTokenDAO
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        public RefreshTokenDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método para guardar o token refresh encriptado na BD
        /// </summary>
        /// <param name="refreshToken">Refesh token</param>
        /// <param name="email">Email do utilizador</param>
        /// <returns>Retorna True se for guardado, 
        /// False caso contrário</returns>
        public bool saveEncryptedRefreshToken(string refreshToken, string email){

            UserDAO userDAO = new UserDAO(_connection);
            User user  =  userDAO.FindUserByEmail(email);

            if(user == null){
                throw new Exception("O utilizador não existe");
            }
            
            try{

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "INSERT INTO dbo.[RefreshTokens] (Email, TokenHash, TokenSalt) " +
                    "VALUES (@Em , @Th, @Ts)";

                    cmd.Parameters.Add("@Em", SqlDbType.NVarChar).Value = email;
                    var token = PasswordOperations.Encrypt(refreshToken);
                    cmd.Parameters.Add("@Th", SqlDbType.NVarChar).Value = token.Item2;
                    cmd.Parameters.Add("@Ts", SqlDbType.NVarChar).Value = token.Item1;
                    
                    if(cmd.ExecuteNonQuery() == 0){
                        return false;
                    }

                    return true;
                }

            } catch(Exception e) {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para atualizar o token refresh guardado na BD
        /// </summary>
        /// <param name="refreshToken">Refesh token</param>
        /// <param name="email">Email do utilizador</param>
        /// <returns>Retorna True se for atualizado,
        /// False caso contrário</returns>
        public bool updateEncryptedRefreshToken(string refreshToken, string email){
            
             try{

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "UPDATE dbo.[RefreshTokens] SET TokenHash = @Th, TokenSalt = @Ts " +
                    "WHERE Email = @Em";

                    cmd.Parameters.Add("@Em", SqlDbType.NVarChar).Value = email;
                    var token = PasswordOperations.Encrypt(refreshToken);
                    cmd.Parameters.Add("@Th", SqlDbType.NVarChar).Value = token.Item2;
                    cmd.Parameters.Add("@Ts", SqlDbType.NVarChar).Value = token.Item1;

                    if(cmd.ExecuteNonQuery() == 0){
                        return false;
                    }

                    return true;
                }
            } catch(Exception e) {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para retornar o token refresh encriptado de 
        /// um utilizador com o email de argumento
        /// </summary>
        /// <param name="email">Email do utilizador</param>
        /// <returns>Retorna um objeto EncryptedRefreshTokenModel,
        /// caom a Hash, salt do token e email do user </returns>
        public EncryptedRefreshTokenModel GetEncryptedRefreshTokenModel(string email){

            try{

                EncryptedRefreshTokenModel encryptedToken = null;

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandText = "SELECT Email, TokenHash, TokenSalt " +
                        "FROM dbo.[RefreshTokens] " +
                        "WHERE Email = @Em";

                    cmd.Parameters.Add("@Em", SqlDbType.NVarChar).Value = email;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            encryptedToken = new EncryptedRefreshTokenModel();
                            reader.Read();
                            encryptedToken.Email = reader.GetString(0);
                            encryptedToken.Hash = reader.GetString(1);
                            encryptedToken.Salt = reader.GetString(2);
                        }
                    }
                }

                return encryptedToken;

            } catch (Exception e){
                throw new Exception(e.Message);
            }
        }
    }
}