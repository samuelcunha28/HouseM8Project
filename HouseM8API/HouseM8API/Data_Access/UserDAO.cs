using HouseM8API.Entities;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using HouseM8API.Models.ReturnedMessages;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas ao User
    /// </summary>
    public class UserDAO : IUserDAO<User>
    {
        private IConnection _connection;
        private string _root;

        /// <summary>
        /// Método construtor da classe UserDAO
        /// </summary>
        /// <param name="connection">Objeto Connection</param>
        public UserDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método construtor da classe UserDAO
        /// </summary>
        /// <param name="connection">Objeto Connection</param>
        /// <param name="projectRoot">Diretório do projeto</param>
        public UserDAO(IConnection connection, string projectRoot)
        {
            _connection = connection;
            _root = projectRoot;
        }

        /// <summary>
        /// Método que procura um utilizador
        /// através do seu email
        /// </summary>
        /// <param name="email">Email do utilizador a pesquisar</param>
        /// <returns>O utilizador encontrado</returns>
        public User FindUserByEmail(string email)
        {
            User user = null;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Id, UserName, Password, PasswordSalt, Email, Description, FirstName, LastName, RoleId, Address " +
                    "FROM dbo.[User] " +
                    "WHERE Email = @email";

                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        user = new User();
                        reader.Read();
                        user.Id = reader.GetInt32(0);
                        user.UserName = reader.GetString(1);
                        user.Password = reader.GetString(2);
                        user.PasswordSalt = reader.GetString(3);
                        user.Email = reader.GetString(4);

                        if (!reader.IsDBNull(5))
                        {
                            user.Description = reader.GetString(5);
                        }

                        user.FirstName = reader.GetString(6);
                        user.LastName = reader.GetString(7);
                        user.Role = (Enums.Roles)reader.GetInt32(8);
                        user.Address = reader.GetString(9);
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// Método para encontrar o utilizador por ID
        /// </summary>
        /// <param name="userId">Id do utilizador</param>
        /// <returns>Retorna o utilizador pretendido, senao retorna null</returns>
        public User FindById(int userId)
        {

            User user = null;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Id, UserName, Password, PasswordSalt, Email, Description, FirstName, LastName, RoleId, Address " +
                    "FROM dbo.[User] " +
                    "WHERE Id = @Id";

                cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = userId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        user = new User();
                        reader.Read();
                        user.Id = reader.GetInt32(0);
                        user.UserName = reader.GetString(1);
                        user.Password = reader.GetString(2);
                        user.PasswordSalt = reader.GetString(3);
                        user.Email = reader.GetString(4);

                        if (!reader.IsDBNull(5))
                        {
                            user.Description = reader.GetString(5);
                        }

                        user.FirstName = reader.GetString(6);
                        user.LastName = reader.GetString(7);
                        user.Role = (Enums.Roles)reader.GetInt32(8);
                        user.Address = reader.GetString(9);
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// Método que atualiza a password
        /// de um determindado utilizador
        /// </summary>
        /// <param name="newPass">Nova palavra-passe</param>
        /// <param name="id">Id do utilizador que pretende alterar
        /// a sua palavra-passe</param>
        /// <returns>
        /// True caso a password seja atualizada com sucesso
        /// False caso contrário
        /// </returns>
        public bool UpdatePassword(PasswordUpdateModel newPass, int? id)
        {
            User user = FindById((int)id);

            if(user==null){
                throw new Exception("O utilizador não existe!");
            }

            if(PasswordOperations.VerifyHash(newPass.OldPassword,user.Password,user.PasswordSalt)){
                
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE dbo.[User] " +
                        "SET Password = @pass, PasswordSalt = @salt " +
                        "WHERE Id = @id";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var password = PasswordOperations.Encrypt(newPass.Password);
                    cmd.Parameters.Add("@pass", SqlDbType.NVarChar).Value = password.Item2;
                    cmd.Parameters.Add("@salt", SqlDbType.NVarChar).Value = password.Item1;

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        return false;
                    }
                }

                return true;

            } else {
                throw new Exception("A password antiga é inválida!");
            }
        }

        /// <summary>
        /// Método para fazer Upload de imagens para o perfil de
        /// utilizador
        /// </summary>
        /// <param name="id">id do user</param>
        /// <param name="mainImage">Imagem</param>
        /// <returns>Caminho da imagem</returns>
        public SuccessMessageModel UploadImagesToUser(int id, IFormFile mainImage)
        {
            try
            {
                string imagesPath = ImageUploadHelper.UploadImage(mainImage, _root, id, "user");

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "UPDATE dbo.[User] " +
                        "SET ImagePath=@Ipath " +
                        "WHERE Id=@id";
                    cmd.Parameters.Add("@Ipath", SqlDbType.NVarChar).Value = imagesPath;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }

                return new SuccessMessageModel("Upload sucedido!");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método que retorna o nome da imagem de perfil
        /// do user
        /// </summary>
        /// <param name="id">Id do user</param>
        /// <returns>Nome da Imagem de perfil</returns>
        public ImageName getProfileImage(int id)
        {

            if (FindById(id) == null)
            {
                throw new Exception("O Post/user não existe!");
            }

            try
            {
                return ImageUploadHelper.getMainImage(id, _root, "user");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método que apaga a imagem de perfil do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador</param>
        /// <returns>Retorna True se a imagem é apagada, False caso contrário</returns>
        public bool deleteImage(int id){
            try{
                return ImageUploadHelper.deleteUserImage(id,_root,"user");
            } catch (Exception e){
                throw new Exception(e.Message);
            }
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
