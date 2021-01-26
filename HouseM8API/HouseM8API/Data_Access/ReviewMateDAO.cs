using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas ao Review do Mate
    /// </summary>
    public class ReviewMateDAO : IReviewMateDAO, IDisposable
    {
        private IConnection _connection;

        /// <summary>
        /// Método construtor do ReviewMateDAO
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        public ReviewMateDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método para criar uma Review ao Mate
        /// Neste método também é possível obter a media das ratings que um mate tenha através da tabela MateReview
        /// E de seguida é crucial colocar essa média na tabela de Users associado ao ID do mate respetivo à review
        /// </summary>
        /// <param name="employer">Id do Employer autor da review</param>
        /// <param name="mate">Id do Mate que vai ser feita a review</param>
        /// <param name="model">Modelo com as informações do review a ser publicada</param>
        /// <returns>Retorna o MateReview criado</returns>
        public MateReview ReviewMate(int employer, int mate, MateReview model)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandText = "INSERT INTO dbo.[MateReview] (Comment, MateId, EmployerId, Rating)" +
                    "VALUES (@com, @mateId, @empId, @rat); SELECT @@Identity";

                cmd.Parameters.Add("@com", SqlDbType.NVarChar).Value = model.Comment;
                cmd.Parameters.Add("@mateId", SqlDbType.Int).Value = mate;
                cmd.Parameters.Add("@empId", SqlDbType.Int).Value = employer;
                cmd.Parameters.Add("@rat", SqlDbType.Float).Value = model.Rating;

                model.Id = int.Parse(cmd.ExecuteScalar().ToString());
            }

            // Calculo da media das ratings do mate que está na tabela do MateReview
            using (SqlCommand cmdAvg = _connection.Fetch().CreateCommand())
            {
                cmdAvg.CommandText = "SELECT AVG([dbo].[MateReview].Rating)" +
                    "FROM dbo.[MateReview]" +
                    "WHERE dbo.[MateReview].MateId = @id";

                cmdAvg.Parameters.Add("@id", SqlDbType.Int).Value = mate;

                double Avg = Double.Parse(cmdAvg.ExecuteScalar().ToString());

                // Update da média na tabela de Users, onde se encontra o ID do mate
                using (SqlCommand cmdSetAvg = _connection.Fetch().CreateCommand())
                {
                    cmdSetAvg.CommandText = "UPDATE dbo.[User]" +
                        "SET AverageRating = @avg " +
                        "WHERE dbo.[User].Id = @id";

                    cmdSetAvg.Parameters.Add("@avg", SqlDbType.Float).Value = Avg;
                    cmdSetAvg.Parameters.Add("@id", SqlDbType.Int).Value = mate;

                    cmdSetAvg.ExecuteScalar();
                }
            }
            return model;
        }

        /// <summary>
        /// Método que retorna a lista de reviews feitas ao mate
        /// </summary>
        /// <param name="mate">Id do mate</param>
        /// <returns>Lista de reviews</returns>
        public List<MateReviewsModel> MateReviewsList(int mate){
            List<MateReviewsModel> reviews = new List<MateReviewsModel>();

             using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT MateReview.Rating, MateReview.Comment, dbo.[User].UserName as Author FROM MateReview " +
                "inner join dbo.[User] ON dbo.[User].Id = MateReview.EmployerId " + 
                "Where MateId=@mate";

                cmd.Parameters.Add("@mate", SqlDbType.Int).Value = mate;

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    adpt.Fill(table);

                    foreach (DataRow row in table.Rows)
                    {
                        MateReviewsModel mr = new MateReviewsModel
                        {
                            Rating = Convert.ToSingle(row["Rating"]),
                            Comment = row["Comment"].ToString(),
                            Author = row["Author"].ToString()
                        };

                        reviews.Add(mr);
                    }
                }
            }

            return reviews;
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
