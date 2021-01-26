using HouseM8API.Entities;
using HouseM8API.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas ao Report de um utilizador
    /// </summary>
    public class ReportDAO : IReportDAO, IDisposable
    {
        private IConnection _connection;

        /// <summary>
        /// Método construtor do ReportDAO
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        public ReportDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método para criar um Report a um User
        /// </summary>
        /// <param name="id">Id do User irá reportar</param>
        /// <param name="user">Utilizador a ser reportado</param>
        /// <param name="model">Modelo com as informações do report a ser publicado</param>
        /// <returns>Retorna o Report criado</returns>
        public Report ReportUser(int id, int user, Report model)
        {
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandText = "INSERT INTO dbo.[Reports] (Comment, ReporterId, ReportedId, ReasonId)" +
                    "VALUES (@com, @rptId, @rptedId, @reasId); SELECT @@Identity";

                cmd.Parameters.Add("@com", SqlDbType.NVarChar).Value = model.Comment;
                cmd.Parameters.Add("@rptId", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@rptedId", SqlDbType.Int).Value = user;
                cmd.Parameters.Add("@reasId", SqlDbType.Int).Value = model.Reason;

                model.Id = int.Parse(cmd.ExecuteScalar().ToString());
                model.ReportedId = (int)id;
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
