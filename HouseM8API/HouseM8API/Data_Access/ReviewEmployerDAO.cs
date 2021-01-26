using HouseM8API.Interfaces;
using Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas ao Review do Employer
    /// </summary>
    public class ReviewEmployerDAO : IReviewEmployerDAO, IDisposable
    {
        private IConnection _connection;

        /// <summary>
        /// Método construtor do ReviewEmployerDAO
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        public ReviewEmployerDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método para criar uma Review ao Employer
        /// Neste método também é possível obter a media das ratings que um employer tenha através da tabela EmployerReview
        /// E de seguida é crucial colocar essa média na tabela de Users associado ao ID do employer respetivo à review
        /// </summary>
        /// <param name="employer">Id do Employer que vai ser feita a review</param>
        /// <param name="model">Modelo com as informações do review a ser publicada</param>
        /// <returns>Retorna a Review criada</returns>
        public Review ReviewEmployer(int employer, Review model)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandText = "INSERT INTO dbo.[EmployerReview] (Rating, EmployerId)" +
                    "VALUES (@rat, @empId); SELECT @@Identity";

                cmd.Parameters.Add("@rat", SqlDbType.Float).Value = model.Rating;
                cmd.Parameters.Add("@empId", SqlDbType.Int).Value = employer;

                model.Id = int.Parse(cmd.ExecuteScalar().ToString());
            }

            // Calculo da media das ratings do employer que está na tabela do EmployerReview
            using (SqlCommand cmdAvg = _connection.Fetch().CreateCommand())
            {
                cmdAvg.CommandText = "SELECT AVG([dbo].[EmployerReview].Rating)" +
                    "FROM dbo.[EmployerReview]" +
                    "WHERE dbo.[EmployerReview].EmployerId = @id";

                cmdAvg.Parameters.Add("@id", SqlDbType.Int).Value = employer;

                double Avg = Double.Parse(cmdAvg.ExecuteScalar().ToString());

                // Update da média na tabela de Users, onde se encontra o ID do employer
                using (SqlCommand cmdSetAvg = _connection.Fetch().CreateCommand())
                {
                    cmdSetAvg.CommandText = "UPDATE dbo.[User]" +
                        "SET AverageRating = @avg " +
                        "WHERE dbo.[User].Id = @id";

                    cmdSetAvg.Parameters.Add("@avg", SqlDbType.Float).Value = Avg;
                    cmdSetAvg.Parameters.Add("@id", SqlDbType.Int).Value = employer;

                    cmdSetAvg.ExecuteScalar();
                }
            }
            return model;
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
