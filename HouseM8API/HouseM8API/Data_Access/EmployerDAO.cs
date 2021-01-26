using Abp.Extensions;
using Enums;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas ao Employer
    /// </summary>
    public class EmployerDAO : IEmployerDAO<Employer>
    {

        private readonly IConnection _connection;

        /// <summary>
        /// Método construtor da classe EmployerDAO
        /// </summary>
        /// <param name="connection">Objeto Connection </param>
        public EmployerDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método para criar um Employer 
        /// na Base de dados
        /// </summary>
        /// <param name="model">Modelo do employer com os dados</param>
        /// <returns>Employer caso seja adicionado com sucesso,
        /// senão retorna NULL</returns>
        public Employer Create(Employer model)
        {
            try
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    if (model.Description != null)
                    {
                        cmd.CommandText = "INSERT INTO dbo.[User] (UserName, Password, PasswordSalt, Email, Description, FirstName, LastName, RoleId, Address)" +
                        "VALUES (@Un, @Pass, @Slt, @mail, @Desc, @Fn, @Ln, @Rid, @Addr); SELECT @@Identity";

                        cmd.Parameters.Add("@Desc", SqlDbType.NVarChar).Value = model.Description;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO dbo.[User] (UserName, Password, PasswordSalt, Email, FirstName, LastName, RoleId, Address)" +
                        "VALUES (@Un, @Pass, @Slt, @mail, @Fn, @Ln, @Rid, @Addr); SELECT @@Identity";
                    }

                    cmd.Parameters.Add("@Un", SqlDbType.NVarChar).Value = model.UserName;
                    var password = PasswordOperations.Encrypt(model.Password);
                    cmd.Parameters.Add("@Pass", SqlDbType.NVarChar).Value = password.Item2;
                    cmd.Parameters.Add("@Slt", SqlDbType.NVarChar).Value = password.Item1;
                    cmd.Parameters.Add("@mail", SqlDbType.NVarChar).Value = model.Email;
                    cmd.Parameters.Add("@Fn", SqlDbType.NVarChar).Value = model.FirstName;
                    cmd.Parameters.Add("@Ln", SqlDbType.NVarChar).Value = model.LastName;
                    cmd.Parameters.Add("@Rid", SqlDbType.Int).Value = Roles.EMPLOYER;
                    cmd.Parameters.Add("@Addr", SqlDbType.NVarChar).Value = model.Address;

                    model.Id = int.Parse(cmd.ExecuteScalar().ToString());
                    model.Role = Roles.EMPLOYER;
                    model.Password = password.Item2;
                    model.PasswordSalt = password.Item1;
                }

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.[Employer] (Id)" +
                       "VALUES (@Id); SELECT @@Identity";

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                    cmd.ExecuteScalar();
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Método para atualizar dados pessoais de um employer
        /// </summary>
        /// <param name="model">Modelo com os parametros do employer</param>
        /// <param name="id">Id do employer a ser atualizado</param>
        /// <returns>Employer atualizado</returns>
        public Employer Update(Employer model, int id)
        {

            Employer existent = FindEmployerById((int)id);

            try
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE dbo.[User] SET FirstName = @fn, LastName = @ln, UserName = @un, Description = @des, Address = @add " +
                        "WHERE Id = @id AND RoleId = @rId";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@rId", SqlDbType.Int).Value = 2;

                    if(model.FirstName.IsNullOrEmpty()){
                        cmd.Parameters.Add("@fn", SqlDbType.NVarChar).Value = existent.FirstName;
                    } else {
                        cmd.Parameters.Add("@fn", SqlDbType.NVarChar).Value = model.FirstName;
                    }

                    if(model.LastName.IsNullOrEmpty()){
                        cmd.Parameters.Add("@ln", SqlDbType.NVarChar).Value = existent.LastName;
                    } else {
                        cmd.Parameters.Add("@ln", SqlDbType.NVarChar).Value = model.LastName;
                    }

                    if(model.UserName.IsNullOrEmpty()){
                        cmd.Parameters.Add("@un", SqlDbType.NVarChar).Value = existent.UserName;
                    } else {
                        cmd.Parameters.Add("@un", SqlDbType.NVarChar).Value = model.UserName;
                    }

                    if(model.Description.IsNullOrEmpty()){
                        cmd.Parameters.Add("@des", SqlDbType.NVarChar).Value = existent.Description;
                    } else {
                        cmd.Parameters.Add("@des", SqlDbType.NVarChar).Value = model.Description;
                    }
                    
                    if(model.Address.IsNullOrEmpty()){
                        cmd.Parameters.Add("@add", SqlDbType.NVarChar).Value = existent.Address;
                    } else {
                        cmd.Parameters.Add("@add", SqlDbType.NVarChar).Value = model.Address;
                    }
                    
                    cmd.ExecuteNonQuery();
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Método que retorna uma coleção de mates favoritos
        /// do employer correspondente ao ID de parametro do método
        /// </summary>
        /// <param name="id">Id do employer que possui os mates favoritos</param>
        /// <returns>Coleção de Mates favoritos</returns>
        public Collection<FavoriteModel> FavoritesList(int id)
        {
            Collection<FavoriteModel> favorite = new Collection<FavoriteModel>();

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT dbo.[User].UserName, dbo.[User].Email " +
                    "FROM dbo.[Favourites] INNER JOIN dbo.[User] ON Favourites.MateId = dbo.[User].Id " +
                    "WHERE EmployerId = @id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        FavoriteModel favoritesModel = new FavoriteModel
                        {
                            UserName = row["UserName"].ToString(),
                            Email = row["Email"].ToString(),
                        };

                        favorite.Add(favoritesModel);
                    }
                }
            }

            return favorite;
        }

        /// <summary>
        /// Método para adicionar um mate aos favoritos de um Employer
        /// </summary>
        /// <param name="emID">Id do Employer</param>
        /// <param name="mateID">Id do Mate a ser adicionado</param>
        /// <returns>Retorna o Id do Mate adicionado</returns>
        public int? AddFavorite(int emID, int mateID)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.[Favourites] (EmployerId, MateId) " +
                    "VALUES (@EmId, @MtID); SELECT @@Identity";

                cmd.Parameters.Add("@EmId", SqlDbType.Int).Value = emID;
                cmd.Parameters.Add("@MtID", SqlDbType.Int).Value = mateID;

                cmd.ExecuteScalar();
            }

            return mateID;
        }

        /// <summary>
        /// Método para encontrar um Mate por Email
        /// </summary>
        /// <param name="email">Email do Mate a procurar</param>
        /// <returns>Retorna o Id do mate encontrado</returns>
        public int FindMateByEmail(string email)
        {
            Mate m = null;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Id " +
                    "FROM dbo.[User] " +
                    "WHERE Email = @email AND RoleId = 1";

                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        m = new Mate();
                        reader.Read();
                        m.Id = reader.GetInt32(0);
                    }
                }
            }

            return m.Id;
        }

        /// <summary>
        /// Método para remover um mate dos favoritos
        /// </summary>
        /// <param name="employerId">Id do Employer com favoritos</param>
        /// <param name="mateId">Id do Mate a ser Removido</param>
        /// <returns>Retorna o Id do mate removido</returns>
        public int? RemoveFavorite(int employerId, int mateId)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM dbo.[Favourites] " +
                    "WHERE EmployerId = @emId AND MateId = @mtId";

                cmd.Parameters.Add("@emId", SqlDbType.Int).Value = employerId;
                cmd.Parameters.Add("@mtId", SqlDbType.Int).Value = mateId;

                cmd.ExecuteScalar();
            }

            return mateId;
        }

        /// <summary>
        /// Método para encontrar um Employer por Id
        /// </summary>
        /// <param name="id">Id do Employer a procurar</param>
        /// <returns>Retorna um objeto Employer com 
        /// os dados do Employer pretendido</returns>
        public Employer FindEmployerById(int id)
        {
            Employer employer = null;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT Id, UserName, FirstName, LastName, Email, Address, Description, AverageRating " +
                    "FROM dbo.[User] " +
                    "WHERE Id = @id AND RoleId = 2";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        employer = new Employer();
                        reader.Read();
                        employer.Id = reader.GetInt32(0);
                        employer.UserName = reader.GetString(1);
                        employer.FirstName = reader.GetString(2);
                        employer.LastName = reader.GetString(3);
                        employer.Email = reader.GetString(4);



                        employer.Address = reader.GetString(5);

                        if (!reader.IsDBNull(6))
                        {
                            employer.Description = reader.GetString(6);
                        }

                        if (!reader.IsDBNull(7))
                        {
                            employer.AverageRating = reader.GetDouble(7);
                        }
                    }
                }
            }

            return employer;
        }

        /// <summary>
        /// Método para listar todos os trabalhos pendentes
        /// </summary>
        /// <param name="id">Id do Employer que pretende aceder à sua
        /// lista de trabalhos pendentes</param>
        /// <returns>Coleção de trabalhos pendentes</returns>
        public Collection<PendingJobModel> PendingJobsList(int id)
        {
            Collection<PendingJobModel> pendingJobs = new Collection<PendingJobModel>();

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT j.Id, jp.Title, jp.Category, jp.Description " +
                    "FROM dbo.[Job] j, dbo.[JobPosts] jp, dbo.[PendingJobs] pj " +
                    "WHERE pj.EmployerId = @id AND pj.EmployerId = j.EmployerId AND j.JobPostId = jp.Id AND pj.JobId = j.Id AND j.FinishedConfirmedByEmployer = 0";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        PendingJobModel pending = new PendingJobModel
                        {
                            JobId = int.Parse(row["Id"].ToString()),
                            Title = row["Title"].ToString(),
                            Category = row["Category"].ToString(),
                            Descritpion = row["Description"].ToString(),
                        };

                        pendingJobs.Add(pending);
                    }
                }
            }

            return pendingJobs;
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
