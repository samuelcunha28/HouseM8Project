using Enums;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using HouseM8API.Models.ReturnedMessages;
using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas ao JobPost
    /// </summary>
    public class JobDAO : IJobDAO
    {
        private readonly IConnection _connection;
        private string _root;
        private char separator = Path.DirectorySeparatorChar;

        /// <summary>
        /// Método construtor do JobDAO
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        public JobDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método construtor do JobDAO que recebe o root do projeto
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        /// <param name="projectRoot">string root do projeto</param>
        public JobDAO(IConnection connection, string projectRoot)
        {
            _connection = connection;
            _root = projectRoot;
        }

        /// <summary>
        /// Método para criar um Post 
        /// </summary>
        /// <param name="employer">Id do Employer que vai criar o Post</param>
        /// <param name="model">Objeto do Post com a informação a ser publicada</param>
        /// <returns>Retorna o JobPost criado</returns>
        public JobPost Create(int employer, JobPost model)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                if (model.ImagePath != null)
                {
                    cmd.CommandText = "INSERT INTO dbo.[JobPosts] (Title, Category, ImagePath, Description, Tradable, InitialPrice, Address, EmployerId, IsDone)" +
                    "VALUES (@Titl, @Cat, @Ipath, @Desc, @Trad, @IPrc, @Addr, @EmpId, @Dn); SELECT @@Identity";

                    cmd.Parameters.Add("@Ipath", SqlDbType.NVarChar).Value = model.ImagePath;
                }
                else
                {
                    cmd.CommandText = "INSERT INTO dbo.[JobPosts] (Title, Category, Description, Tradable, InitialPrice, Address, EmployerId, IsDone)" +
                    "VALUES (@Titl, @Cat, @Desc, @Trad, @IPrc, @Addr, @EmpId, @Dn); SELECT @@Identity";
                }

                cmd.Parameters.Add("@Titl", SqlDbType.NVarChar).Value = model.Title;
                cmd.Parameters.Add("@Cat", SqlDbType.Int).Value = model.Category;
                cmd.Parameters.Add("@Desc", SqlDbType.NVarChar).Value = model.Description;
                cmd.Parameters.Add("@Trad", SqlDbType.Bit).Value = model.Tradable;
                cmd.Parameters.Add("@IPrc", SqlDbType.Float).Value = model.InitialPrice;
                cmd.Parameters.Add("@Addr", SqlDbType.NVarChar).Value = model.Address;
                cmd.Parameters.Add("@EmpId", SqlDbType.Int).Value = employer;
                cmd.Parameters.Add("@Dn", SqlDbType.Bit).Value = false;

                model.Id = int.Parse(cmd.ExecuteScalar().ToString());

            }

            foreach (Payment payment in model.PaymentMethod)
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.[PaymentJob] (JobPostId, PaymentTypeId)" +
                       "VALUES (@PostId, @PayId); SELECT @@Identity";

                    cmd.Parameters.Add("@PostId", SqlDbType.Int).Value = model.Id;
                    cmd.Parameters.Add("@PayId", SqlDbType.Int).Value = payment;

                    cmd.ExecuteScalar();
                }
            }

            return model;
        }

        /// <summary>
        /// Método de pesquisa de trabalho disponível para o mate com vários filtros disponíveis
        /// </summary>
        /// <param name="categories">Filtro de Categorias</param>
        /// <param name="address">Filtro da Morada</param>
        /// <param name="distance">Filtro de distância</param>
        /// <param name="rating">Filtro de Rating</param>
        /// <param name="mateId"></param>
        /// <returns>Retorna a listagem de posts</returns>
        public List<JobPostReturnedModel> GetJobs(Categories[] categories, string address, int? distance, int? rating, int mateId)
        {
            List<JobPostReturnedModel> posts = new List<JobPostReturnedModel>();

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                bool hasQuery = false;
                string query = " where ";

                if (rating != null)
                {
                    hasQuery = true;
                    query += " (AverageRating >= " + rating + ")";
                }

                if (categories.Length != 0)
                {
                    if (hasQuery)
                    {
                        query += "and (";
                        int lastOne = categories.Length - 1;

                        for (int i = 0; i < lastOne; i++)
                        {
                            query += "Category=" + (int)categories[i] + " or ";
                        }
                        query += "Category=" + (int)categories[lastOne] + " )";
                    }
                    else
                    {
                        hasQuery = true;
                        int lastOne = categories.Length - 1;
                        query += "(";
                        for (int i = 0; i < lastOne; i++)
                        {
                            query += "Category=" + (int)categories[i] + " or ";
                        }
                        query += "Category=" + (int)categories[lastOne] + ") ";

                    }
                }

                if (hasQuery)
                {
                    cmd.CommandText = "Select dbo.[JobPosts].Id, dbo.[JobPosts].Title, dbo.[JobPosts].Category, dbo.[JobPosts].Description, dbo.[JobPosts].Tradable, dbo.[JobPosts].InitialPrice, dbo.[JobPosts].Address, dbo.[JobPosts].EmployerId, dbo.[User].AverageRating FROM dbo.[JobPosts] INNER JOIN dbo.[User] ON dbo.[JobPosts].EmployerId = dbo.[User].Id" + query + "AND NOT EXISTS (SELECT * FROM dbo.[IgnoredJobs] WHERE dbo.[IgnoredJobs].MateId = " + mateId + " AND dbo.[IgnoredJobs].JobPostId = dbo.[JobPosts].Id ) AND dbo.[JobPosts].IsDone = 0 ORDER BY dbo.[JobPosts].Id ASC;";
                }
                else
                {
                    cmd.CommandText = "Select dbo.[JobPosts].Id, dbo.[JobPosts].Title, dbo.[JobPosts].Category, dbo.[JobPosts].Description, dbo.[JobPosts].Tradable, dbo.[JobPosts].InitialPrice, dbo.[JobPosts].Address, dbo.[JobPosts].EmployerId, dbo.[User].AverageRating FROM dbo.[JobPosts] INNER JOIN dbo.[User] ON dbo.[JobPosts].EmployerId = dbo.[User].Id WHERE NOT EXISTS (SELECT * FROM dbo.[IgnoredJobs] WHERE dbo.[IgnoredJobs].MateId = " + mateId + " AND dbo.[IgnoredJobs].JobPostId = dbo.[JobPosts].Id ) AND dbo.[JobPosts].IsDone = 0 ORDER BY dbo.[JobPosts].Id ASC;";
                }


                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adpt.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        JobPostReturnedModel post = new JobPostReturnedModel
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            Title = row["Title"].ToString(),
                            Description = row["Description"].ToString(),
                            Tradable = (bool)row["Tradable"],
                            Category = (Categories)row["Category"],
                            InitialPrice = (double)row["InitialPrice"],
                            Address = row["Address"].ToString(),
                            EmployerId = int.Parse(row["EmployerId"].ToString()),
                        };

                        using (SqlCommand paymentCommand = _connection.Fetch().CreateCommand())
                        {
                            paymentCommand.CommandType = CommandType.Text;
                            paymentCommand.CommandText = "Select dbo.[PaymentJob].PaymentTypeId FROM dbo.[PaymentJob] WHERE dbo.[PaymentJob].JobPostId = " + post.Id;

                            using (SqlDataAdapter adptPayment = new SqlDataAdapter(paymentCommand))
                            {
                                DataTable tablePayment = new DataTable();
                                adptPayment.Fill(tablePayment);

                                List<Payment> payments = new List<Payment>();
                                foreach (DataRow rowPayment in tablePayment.Rows)
                                {
                                    payments.Add((Payment)rowPayment["PaymentTypeId"]);
                                }
                                post.PaymentMethod = payments.ToArray();
                            }
                        }

                        posts.Add(post);
                    }
                }
            }

            if (address != null)
            {
                if (distance != null)
                {
                    JobPostReturnedModel[] jobPosts = posts.ToArray();
                    posts = new List<JobPostReturnedModel>();
                    for (int i = 0; i < jobPosts.Length; i++)
                    {
                        int? distanceToWork = DistancesHelper.calculateDistanceBetweenAddresses(address, jobPosts[i].Address);

                        if (distanceToWork != null)
                        {
                            if (distanceToWork <= distance)
                            {
                                jobPosts[i].Range = (int)distanceToWork;
                                posts.Add(jobPosts[i]);
                            }
                        }
                    }
                }
            }

            return posts;
        }

        /// <summary>
        /// Método para apagar um jobPost
        /// </summary>
        /// <param name="toDelete">Objeto Jobpost que vai ser apagado</param>
        /// <returns>True se for apagado, false caso contrário</returns>
        public bool Delete(JobPost toDelete)
        {
            bool deleted = false;
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM dbo.[Job] WHERE JobPostId=@id;" +
                                  "DELETE FROM dbo.[Offer] WHERE JobPostId=@id;" +
                                  "DELETE FROM dbo.[PaymentJob] WHERE JobPostId=@id; " +
                                  "DELETE FROM dbo.[JobPosts] WHERE Id=@id;";

                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = toDelete.Id;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    deleted = true;
                }
            }

            if (deleted == true)
            {
                string path = _root + $"{separator}" + "Uploads"
                + $"{separator}" + "Posts"
                + $"{separator}" + toDelete.Id;

                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }

            return deleted;
        }

        /// <summary>
        /// Método para Encontrar um Post por Id
        /// </summary>
        /// <param name="id">Id do Post a ser encontrado</param>
        /// <returns>Retorna o JobPost procurado</returns>
        public JobPost FindById(int id)
        {
            JobPost p = null;
            bool contains = false;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Id, Title, Category, ImagePath, Description, Tradable, InitialPrice, Address, IsDone FROM dbo.[JobPosts] " +
                    "WHERE Id=@id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (contains = reader.HasRows)
                    {
                        p = new JobPost();
                        reader.Read();
                        p.Id = reader.GetInt32(0);
                        p.Title = reader.GetString(1);
                        p.Category = (Categories)reader.GetInt32(2);

                        if (reader.IsDBNull(3))
                        {
                            p.ImagePath = "";
                        }
                        else
                        {
                            p.ImagePath = reader.GetString(3);
                        }

                        p.Description = reader.GetString(4);
                        p.Tradable = reader.GetBoolean(5);
                        p.InitialPrice = reader.GetDouble(6);
                        p.Address = reader.GetString(7);
                        p.IsDone = reader.GetBoolean(8);
                    }
                }
            }

            if (contains)
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT JobPostId, PaymentTypeId  FROM dbo.[PaymentJob] " +
                        "WHERE JobPostId=@id";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        List<Payment> temp = new List<Payment>();

                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        foreach (DataRow row in table.Rows)
                        {
                            temp.Add((Payment)row["PaymentTypeId"]);
                        }

                        p.PaymentMethod = temp.ToArray();
                    }
                }
            }
            return p;
        }

        /// <summary>
        /// Método para encontrar um JobPost por Id com um employer associado
        /// </summary>
        /// <param name="id">Id do Jobpost</param>
        /// <param name="employerId">Id do Employer associado</param>
        /// <returns>Retorna o Jobpost</returns>
        public JobPost FindById(int id, int employerId)
        {
            JobPost p = null;
            bool contains = false;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Id, Title, Category, ImagePath, Description, Tradable, InitialPrice, Address FROM dbo.[JobPosts] " +
                    "WHERE Id=@id AND EmployerId = @employerId";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@employerId", SqlDbType.Int).Value = employerId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (contains = reader.HasRows)
                    {
                        p = new JobPost();
                        reader.Read();
                        p.Id = reader.GetInt32(0);
                        p.Title = reader.GetString(1);
                        p.Category = (Categories)reader.GetInt32(2);

                        if (reader.IsDBNull(3))
                        {
                            p.ImagePath = "";
                        }
                        else
                        {
                            p.ImagePath = reader.GetString(3);
                        }

                        p.Description = reader.GetString(4);
                        p.Tradable = reader.GetBoolean(5);
                        p.InitialPrice = reader.GetDouble(6);
                        p.Address = reader.GetString(7);
                    }
                }
            }

            if (contains)
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT JobPostId, PaymentTypeId  FROM dbo.[PaymentJob] " +
                        "WHERE JobPostId=@id";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        List<Payment> temp = new List<Payment>();

                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        foreach (DataRow row in table.Rows)
                        {
                            temp.Add((Payment)row["PaymentTypeId"]);
                        }

                        p.PaymentMethod = temp.ToArray();
                    }
                }
            }
            return p;
        }

        /// <summary>
        /// Método que retorna a listagem 
        /// de todos os JobPosts de um Employer
        /// </summary>
        /// <param name="employer">Id do Employer dos JobPosts</param>
        /// <returns>Retorna uma lista de Jobposts associada ao Employer</returns>
        public List<JobPost> GetEmployerPosts(int employer)
        {
            List<JobPost> posts = new List<JobPost>();

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM dbo.[JobPosts] " +
                    "WHERE EmployerId=@id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = employer;

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adpt.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        JobPost jp = new JobPost
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            Title = row["Title"].ToString(),
                            Category = (Categories)row["Category"],
                            Description = row["Description"].ToString(),
                            Tradable = Convert.ToBoolean(row["Tradable"]),
                            InitialPrice = (double)row["InitialPrice"],
                            Address = row["Address"].ToString(),
                        };

                        if (row["ImagePath"].ToString() == "" ||
                            row["ImagePath"].ToString() == null)
                        {
                            jp.ImagePath = null;
                        }
                        else
                        {
                            jp.ImagePath = row["ImagePath"].ToString();
                        }

                        using (SqlCommand paymentCommand = _connection.Fetch().CreateCommand())
                        {
                            paymentCommand.CommandType = CommandType.Text;
                            paymentCommand.CommandText = "SELECT PaymentTypeId FROM dbo.[PaymentJob] " +
                                                         "WHERE JobPostId=@JobId";
                            paymentCommand.Parameters.Add("@JobId", SqlDbType.Int).Value = jp.Id;

                            using (SqlDataAdapter paymentAdapter = new SqlDataAdapter(paymentCommand))
                            {

                                List<Payment> temp = new List<Payment>();
                                DataTable paymentTable = new DataTable();
                                paymentAdapter.Fill(paymentTable);

                                foreach (DataRow payment in paymentTable.Rows)
                                {
                                    temp.Add((Payment)payment["PaymentTypeId"]);
                                }

                                jp.PaymentMethod = temp.ToArray();
                            }
                        }
                        posts.Add(jp);
                    }
                }
            }
            return posts;
        }

        /// <summary>
        /// Método para realizar uma oferta de preço a um trabalho selecionado
        /// </summary>
        /// <param name="offer">Objeto Offer com informação da oferta</param>
        /// <param name="mateId">Id do Mate que realiza a oferta</param>
        /// <returns>Retorna a oferta feita pelo Mate</returns>
        public Offer makeOfferOnJob(Offer offer, int? mateId)
        {
            if (offer.Price == 0)
            {
                offer.Price = offer.JobPost.InitialPrice;
            }

            offer.Approved = false;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.[Offer] (Approved, Price, MateId, JobPostId)" +
                "VALUES (@Apprvd, @Prc, @MtId, @JbPstId); SELECT @@Identity";


                cmd.Parameters.Add("@Apprvd", SqlDbType.Bit).Value = offer.Approved;
                cmd.Parameters.Add("@Prc", SqlDbType.Float).Value = offer.Price;
                cmd.Parameters.Add("@MtId", SqlDbType.Int).Value = mateId;
                cmd.Parameters.Add("@JbPstId", SqlDbType.Int).Value = offer.JobPost.Id;

                offer.Id = int.Parse(cmd.ExecuteScalar().ToString());

            }

            return offer;
        }

        /// <summary>
        /// Método para atualizar os detalhes de um post
        /// </summary>
        /// <param name="model">Modelo do post com dados novos para atualizar</param>
        /// <returns>retorna o JobPost atualizado</returns>
        public JobPost UpdatePostDetails(JobPost model)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                if (model.ImagePath != null)
                {
                    cmd.CommandText = "UPDATE dbo.[JobPosts] " +
                     "SET Title=@ttl, Category=@cat, ImagePath=@Ipath, Description=@desc, Tradable=@trad, InitialPrice=@Iprice, Address=@Add " +
                     "WHERE Id=@id";
                    cmd.Parameters.Add("@Ipath", SqlDbType.NVarChar).Value = model.ImagePath;
                }
                else
                {
                    cmd.CommandText = "UPDATE dbo.[JobPosts] " +
                     "SET Title=@ttl, Category=@cat, Description=@desc, Tradable=@trad, InitialPrice=@Iprice, Address=@Add " +
                     "WHERE Id=@id";
                }

                cmd.Parameters.Add("@ttl", SqlDbType.NVarChar).Value = model.Title;
                cmd.Parameters.Add("@cat", SqlDbType.Int).Value = model.Category;
                cmd.Parameters.Add("@desc", SqlDbType.NVarChar).Value = model.Description;
                cmd.Parameters.Add("@trad", SqlDbType.Bit).Value = model.Tradable;
                cmd.Parameters.Add("@Iprice", SqlDbType.Float).Value = model.InitialPrice;
                cmd.Parameters.Add("@Add", SqlDbType.NVarChar).Value = model.Address;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = model.Id;

                cmd.ExecuteNonQuery();
            }

            return model;
        }

        /// <summary>
        /// Método que adiciona vários
        /// tipos de pagamento a uma 
        /// publicação de trabalho
        /// </summary>
        /// <param name="jobId">Id do Job onde se pretende adicionar
        /// métodos de pagamento</param>
        /// <param name="payments">Método(s) de pagamento a adicionar</param>
        public void AddPayment(int jobId, PaymentModel[] payments)
        {
            foreach (PaymentModel pay in payments)
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.[PaymentJob] (JobPostId, PaymentTypeId) " +
                        "VALUES (@JpId, @PtId); SELECT @@Identity";

                    cmd.Parameters.Add("@JpId", SqlDbType.Int).Value = jobId;
                    cmd.Parameters.Add("@PtId", SqlDbType.Int).Value = pay.payments;

                    cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Método para remover um tipo de pagamento
        /// de uma publicação de trabalho
        /// </summary>
        /// <param name="jobId">Id do Job onde se pretende remover
        /// um método de pagamento</param>
        /// <param name="payment">Método de pagamento a remover</param>
        public void RemovePayment(int jobId, PaymentModel payment)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM dbo.[PaymentJob] " +
                    "WHERE JobPostId = @JpId AND PaymentTypeId = @PtId";

                cmd.Parameters.Add("@JpId", SqlDbType.Int).Value = jobId;
                cmd.Parameters.Add("@PtId", SqlDbType.Int).Value = payment.payments;

                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Método que permite fazer upload de imagens para um post
        /// </summary>
        /// <param name="id">id do post</param>
        /// <param name="images">coleção de imagens</param>
        /// <param name="mainImage">Imagem de destaque</param>
        /// <returns>Retorna os caminhos das imagens adicionadas</returns>
        public SuccessMessageModel UploadImagesToPost(int id, IFormFileCollection images,
        IFormFile mainImage)
        {
            try
            {

                ImageUploadHelper.UploadImage(mainImage, _root, id, "postMain");
                string imagesPath = ImageUploadHelper.UploadPostImages(images, _root, id, "post");

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "UPDATE dbo.[JobPosts] " +
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
        /// Método que retorna uma lista com os nomes das imagens
        /// do jobPost
        /// </summary>
        /// <param name="id">Id do post</param>
        /// <returns>Retorna lista com nomes de imagens</returns>
        public List<ImageName> getImages(int id)
        {

            if (FindById(id) == null)
            {
                throw new Exception("O Post não existe!");
            }

            try
            {

                return ImageUploadHelper.getImages(id, _root, "post");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método que retorna o nome da imagem de destaque
        /// </summary>
        /// <param name="id">Id do post</param>
        /// <returns>Retorna o nome da Imagem de destaque</returns>
        public ImageName getMainImage(int id)
        {

            if (FindById(id) == null)
            {
                throw new Exception("O Post/user não existe!");
            }
            try
            {
                return ImageUploadHelper.getMainImage(id, _root, "postMain");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para apagar a imagem de destaque de um JobPost
        /// </summary>
        /// <param name="id">Id do employer</param>
        /// <param name="post">Id do JobPost</param>
        /// <param name="image">Objeto ImageName com o nome da imagem</param>
        /// <returns>Retorna True se a imagem for apagada, 
        /// False caso contrário</returns>
        public bool deleteMainImage(int id, int post, ImageName image)
        {

            if (FindById(post, id) == null)
            {
                throw new Exception("Não existe post associado a este employer!");
            }

            if (image.Name == null || image.Name.Length == 0)
            {
                throw new Exception("Nome de imagem não enviado!");
            }

            try
            {
                return ImageUploadHelper.deletePostImage(post, image.Name, _root, "postMain");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para apagar uma imagem de um JobPost
        /// </summary>
        /// <param name="id">Id do employer</param>
        /// <param name="post">Id do JobPost</param>
        /// <param name="image">Objeto ImageName com o nome da imagem</param>
        /// <returns>Retorna True se a imagem for apagada, 
        /// False caso contrário</returns>

        public bool deleteImage(int id, int post, ImageName image)
        {

            if (FindById(post, id) == null)
            {
                throw new Exception("Não existe post associado a este employer!");
            }

            if (image.Name == null || image.Name.Length == 0)
            {
                throw new Exception("Nome de imagem não enviado!");
            }

            try
            {
                return ImageUploadHelper.deletePostImage(post, image.Name, _root, "post");
            }
            catch (Exception e)
            {
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
