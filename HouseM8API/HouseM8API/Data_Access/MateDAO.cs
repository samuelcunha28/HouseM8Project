using Abp.Extensions;
using Enums;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas às diversas funções do Mate
    /// </summary>
    public class MateDAO : IMateDAO<Mate>, IDisposable
    {
        private IConnection _connection;

        /// <summary>
        /// Método construtor do MateDAO
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        public MateDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método para criar/registar um Mate na base de dados
        /// </summary>
        /// <param name="model">Modelo do mate com os dados</param>
        /// <returns>Mate caso seja adicionado com sucesso,
        /// senão retorna NULL</returns>
        public Mate Create(Mate model)
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
                    cmd.Parameters.Add("@Rid", SqlDbType.Int).Value = Roles.M8;
                    cmd.Parameters.Add("@Addr", SqlDbType.NVarChar).Value = model.Address;


                    model.Id = int.Parse(cmd.ExecuteScalar().ToString());


                    model.Role = Roles.M8;
                    model.Password = password.Item2;
                    model.PasswordSalt = password.Item1;
                    model.Rank = Ranks.MATE;
                }

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.[Mate] (Id, Range, RankId)" +
                       "VALUES (@Id, @Rng, @Rkid); SELECT @@Identity";

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                    cmd.Parameters.Add("@Rng", SqlDbType.Int).Value = model.Range;
                    cmd.Parameters.Add("Rkid", SqlDbType.Int).Value = model.Rank;

                    cmd.ExecuteScalar();
                }

                foreach (Categories category in model.Categories)
                {
                    using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO dbo.[CategoriesFromM8] (MateId, CategoryId)" +
                           "VALUES (@MtId, @CtgId); SELECT @@Identity";

                        cmd.Parameters.Add("@MtId", SqlDbType.Int).Value = model.Id;
                        cmd.Parameters.Add("@CtgId", SqlDbType.Int).Value = category;

                        cmd.ExecuteScalar();
                    }
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Método para listar as categorias do
        /// mate, através do seu ID
        /// </summary>
        /// <param name="id">Id do Mate a pesquisar as categorias</param>
        /// <returns>Todas as categorias correspondentes ao Mate</returns>
        public Collection<CategoryModel> CategoriesList(int id)
        {
            Collection<CategoryModel> category = new Collection<CategoryModel>();

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT c.[Name] " +
                    "FROM dbo.[CategoriesFromM8] d, dbo.[Categories] c " +
                    "WHERE d.[MateId] = @id AND c.Id = d.CategoryId";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        CategoryModel cModel = new CategoryModel
                        {
                            categories = (Categories)Enum.Parse(typeof(Categories), row["Name"].ToString())
                        };

                        category.Add(cModel);
                    }
                }

                return category;
            }
        }

        /// <summary>
        /// Método para adicionar categorias de trabalho
        /// ao Mate
        /// </summary>
        /// <param name="id">Id do Mate</param>
        /// <param name="category">Categoria(s) a adicionar</param>
        public void AddCategory(int id, CategoryModel[] category)
        {
            foreach (CategoryModel cat in category)
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.[CategoriesFromM8] (MateId, CategoryId) " +
                    "VALUES (@MtId, @CtgId); SELECT @@Identity";

                    cmd.Parameters.Add("@MtId", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@CtgId", SqlDbType.Int).Value = cat.categories;

                    cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Método para remover uma categoria de trabalho
        /// do Mate
        /// </summary>
        /// <param name="id">Id do Mate</param>
        /// <param name="category">Categoria a remover</param>
        public void RemoveCategory(int id, CategoryModel category)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM dbo.[CategoriesFromM8] " +
                "WHERE MateId = @MtId AND CategoryId = @CtgId";

                cmd.Parameters.Add("@MtId", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@CtgId", SqlDbType.Int).Value = category.categories;

                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Método para atualizar os dados pessoais de um Mate
        /// </summary>
        /// <param name="model">Modelo com os parâmetros do Mate</param>
        /// <param name="id">Id do Mate a ser atualizado</param>
        /// <returns>Mate com as informações atualizadas</returns>
        public Mate Update(Mate model, int id)
        {
            Mate existent = FindMateById((int)id);

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE dbo.[User] SET FirstName = @fn, LastName = @ln, UserName = @un, Description = @des, " +
                    "Address = @add WHERE Id = @id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

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

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE dbo.[Mate] SET Range = @rng WHERE Id = @id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                if(model.Range == 0){
                    cmd.Parameters.Add("@rng", SqlDbType.Int).Value = existent.Range;
                } else {
                    cmd.Parameters.Add("@rng", SqlDbType.Int).Value = model.Range;
                }

                cmd.ExecuteNonQuery();
            }

            return model;
        }

        /// <summary>
        /// Método para obter um Mate através do seu ID
        /// </summary>
        /// <param name="id">Id do Mate a pesquisar</param>
        /// <returns>Objeto Mate com os dados do Mate que
        /// foi pesquisado</returns>
        public Mate FindMateById(int id)
        {
            Mate mate = null;
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandText = "SELECT Id, UserName, FirstName, LastName, Email, Address, Description, AverageRating " +
                    "FROM dbo.[User] " +
                    "WHERE Id = @id AND RoleId = 1";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        mate = new Mate();
                        reader.Read();
                        mate.Id = reader.GetInt32(0);
                        mate.UserName = reader.GetString(1);
                        mate.FirstName = reader.GetString(2);
                        mate.LastName = reader.GetString(3);
                        mate.Email = reader.GetString(4);
                        mate.Address = reader.GetString(5);

                        if (!reader.IsDBNull(6))
                        {
                            mate.Description = reader.GetString(6);
                        }

                        if (!reader.IsDBNull(7))
                        {
                            mate.AverageRating = reader.GetDouble(7);
                        }
                    }
                }
            }

            return mate;
        }

        /// <summary>
        /// Metodo que retorna mates baseado 
        /// nos filtros que entrem como paramentros
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="address"></param>
        /// <param name="rank"></param>
        /// <param name="distance"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public List<Mate> GetMates(Categories[] categories, string address, Ranks? rank, int? distance, int? rating)
        {
            List<Mate> mates = new List<Mate>();

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                bool hasQuery = false;
                string query = "WHERE ";

                if (rating != null)
                {
                    hasQuery = true;
                    query += " (AverageRating >= " + rating + ")";
                }

                if (categories.Length != 0)
                {
                    string innerSelect = "dbo.[User].id IN (Select MateId FROM dbo.[CategoriesFromM8] WHERE ";
                    if (hasQuery)
                    {
                        query += "and (" + innerSelect;
                        int lastOne = categories.Length - 1;

                        for (int i = 0; i < lastOne; i++)
                        {
                            query += "CategoryId=" + (int)categories[i] + " or ";
                        }
                        query += "CategoryId=" + (int)categories[lastOne] + " )";
                    }
                    else
                    {
                        hasQuery = true;
                        int lastOne = categories.Length - 1;
                        query += innerSelect;
                        for (int i = 0; i < lastOne; i++)
                        {
                            query += "CategoryId=" + (int)categories[i] + " or ";
                        }
                        query += "CategoryId=" + (int)categories[lastOne] + " ";

                    }
                    query += ")";
                }

                if (rank != null)
                {
                    if ((int)rank > 0 && (int)rank < 5)
                    {
                        hasQuery = true;
                        query += " and (RankId >= " + (int)rank + ")";
                    }
                }

                if (hasQuery)
                {
                    cmd.CommandText = "SELECT dbo.[User].id, dbo.[User].UserName, dbo.[User].Email, dbo.[User].Description, " +
                                      "dbo.[User].FirstName, dbo.[User].LastName, dbo.[User].Address, " +
                                      "dbo.[User].AverageRating, dbo.[Mate].Range, dbo.[Mate].RankId " +
                                      "FROM dbo.[User] " +
                                      "INNER JOIN dbo.[Mate] ON dbo.[User].Id = dbo.[Mate].Id " +
                                      query +
                                      "ORDER BY Id;";
                }
                else
                {
                    cmd.CommandText = "SELECT dbo.[User].id, dbo.[User].UserName, dbo.[User].Email, dbo.[User].Description, " +
                                      "dbo.[User].FirstName, dbo.[User].LastName, dbo.[User].Address, " +
                                      "dbo.[User].AverageRating, dbo.[Mate].Range, dbo.[Mate].RankId " +
                                      "FROM dbo.[User] " +
                                      "INNER JOIN dbo.[Mate] ON dbo.[User].Id = dbo.[Mate].Id " +
                                      "ORDER BY Id;";
                }

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adpt.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        Mate mate = new Mate
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            UserName = row["UserName"].ToString(),
                            Email = row["Email"].ToString(),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Address = row["Address"].ToString(),
                            Range = int.Parse(row["Range"].ToString()),
                            Rank = (Ranks)int.Parse(row["RankId"].ToString()),
                        };

                        if (row["Description"] == DBNull.Value)
                        {
                            mate.Description = null;
                        }
                        else
                        {
                            mate.Description = row["Description"].ToString();
                        }

                        if (row["AverageRating"] == DBNull.Value)
                        {
                            mate.AverageRating = 0;
                        }
                        else
                        {
                            mate.AverageRating = Convert.ToDouble(row["AverageRating"]);
                        }

                        using (SqlCommand categoryCommand = _connection.Fetch().CreateCommand())
                        {
                            categoryCommand.CommandType = CommandType.Text;
                            categoryCommand.CommandText = "Select dbo.[CategoriesFromM8].CategoryId FROM dbo.[CategoriesFromM8] WHERE MateId = " + mate.Id;

                            using (SqlDataAdapter adptCategory = new SqlDataAdapter(categoryCommand))
                            {
                                DataTable tableCategory = new DataTable();
                                adptCategory.Fill(tableCategory);

                                List<Categories> categoriesList = new List<Categories>();
                                foreach (DataRow rowCategory in tableCategory.Rows)
                                {
                                    categoriesList.Add((Categories)int.Parse(rowCategory["CategoryId"].ToString()));
                                }
                                mate.Categories = categoriesList.ToArray();
                            }
                        }

                        mates.Add(mate);
                    }
                }

            }

            if (address != null)
            {
                if (distance != null)
                {
                    Mate[] matesArray = mates.ToArray();
                    mates = new List<Mate>();
                    for (int i = 0; i < matesArray.Length; i++)
                    {
                        if (DistancesHelper.calculateDistanceBetweenAddresses(address, matesArray[i].Address) <= distance)
                        {
                            mates.Add(matesArray[i]);
                        }
                    }
                }
            }
            return mates;
        }

        /// <summary>
        /// Metodo para ignorar um jobPost
        /// </summary>
        /// <param name="mateId"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        public bool IgnoreJobPost(int mateId, IgnoredJobModel job)
        {

            JobDAO jobDAO = new JobDAO(_connection);
            JobPost jobPost = jobDAO.FindById(job.Id);

            if (jobPost == null)
            {
                return false;
            }

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.[IgnoredJobs] (MateId, JobPostId) " +
                "VALUES (@MtId, @JbpstId); SELECT @@Identity";

                cmd.Parameters.Add("@MtId", SqlDbType.Int).Value = mateId;
                cmd.Parameters.Add("@JbpstId", SqlDbType.Int).Value = job.Id;

                cmd.ExecuteScalar();
            }

            return true;
        }

        /// <summary>
        /// Método para listar todos os trabalhos pendentes
        /// </summary>
        /// <param name="id">Id do Mate que pretende aceder à sua
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
                    "WHERE pj.MateId = @id AND pj.MateId = j.MateId AND j.JobPostId = jp.Id AND pj.JobId = j.Id AND j.FinishedConfirmedByMate = 0";

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
        /// Método para atualizar o rank de um Mate
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="MateId"></param>
        /// <returns>True ou Exception</returns>
        public bool UpdateRank(Ranks rank, int MateId)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE dbo.[Mate] SET RankId = @rkId " +
                    "WHERE Id = @id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = MateId;
                cmd.Parameters.Add("@rkId", SqlDbType.Int).Value = rank;

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
