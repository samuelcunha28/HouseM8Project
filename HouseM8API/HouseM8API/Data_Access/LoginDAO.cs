using HouseM8API.Entities;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas ao Login de um utilizador
    /// </summary>
    public class LoginDAO : ILoginDAO
    {
        private readonly IConnection _connection;

        /// <summary>
        /// Método construtor do LoginDAO
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        public LoginDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método para autenticar um User
        /// </summary>
        /// <param name="email">O email do utilizador a ser autenticado</param>
        /// <param name="password">A password do utilizador a ser autenticado</param>
        /// <returns>Retorna o User autenticado ou null</returns>
        public User Authenticate(string email, string password)
        {
            // Caso nao o email seja nulo ou vazio ou a password nula ou vazia
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            UserDAO userDAO = new UserDAO(_connection);
            User user = userDAO.FindUserByEmail(email);

            // Caso o user seja nulo
            if (user == null)
            {
                return null;
            }

            // Verificar a hash da password para que o user faça o login
            if (!PasswordOperations.VerifyHash(password, user.Password, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Método para recuperar a password
        /// </summary>
        /// <param name="newPass">Nova Password</param>
        /// <param name="email">Email do user que vai alterar a password</param>
        /// <returns>Retorna bool</returns>
        public bool RecoverPassword(RecoverPasswordModel newPass, string email)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE dbo.[User] SET Password = @pass, PasswordSalt = @salt " +
                    "WHERE Email = @email";

                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
                var password = PasswordOperations.Encrypt(newPass.Password);
                cmd.Parameters.Add("@pass", SqlDbType.NVarChar).Value = password.Item2;
                cmd.Parameters.Add("@salt", SqlDbType.NVarChar).Value = password.Item1;

                newPass.Password = password.Item2;


                cmd.ExecuteNonQuery();
            }

            return true;
        }

        /// <summary>
        /// Gestão de recursos não gerenciados.
        /// Método que controla o garbage collector
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
