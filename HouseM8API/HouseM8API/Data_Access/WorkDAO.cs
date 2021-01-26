using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// DAO para efetuar operações na base de dados
    /// relativas a um trabalho (Job)
    /// </summary>
    public class WorkDAO : IWorkDAO
    {

        private IConnection _connection;

        /// <summary>
        /// Método construtor da classe WorkDAO
        /// </summary>
        /// <param name="connection">Objeto Connection </param>
        public WorkDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método que cria um trabalho com data, jobpost, status 
        /// e os seus participantes
        /// </summary>
        /// <param name="id">Id do Employer que pretende criar o Job</param>
        /// <param name="model">Modelo do Trabalho com a informação pretendida</param>
        /// <returns>O modelo de trabalho com a informação pretendida</returns>
        public Job Create(int id, Job model)
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            if (MateDAO.FindMateById(model.Mate) == null)
            {
                return null;
            }

            IJobDAO jobDao = new JobDAO(_connection);
            if (jobDao.FindById(model.JobPost) == null)
            {
                return null;
            }

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.[Job] (Date, MateId, JobPostId, FinishedConfirmedByEmployer, FinishedConfirmedByMate, EmployerId)" +
                    "VALUES (@Date, @MId, @postId, @fshEmpl, @fshMt, @EId); SELECT @@Identity";

                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = model.Date;
                cmd.Parameters.Add("@MId", SqlDbType.Int).Value = model.Mate;
                cmd.Parameters.Add("@postId", SqlDbType.Int).Value = model.JobPost;
                cmd.Parameters.Add("@fshEmpl", SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@fshMt", SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@EId", SqlDbType.Int).Value = id;

                model.Id = int.Parse(cmd.ExecuteScalar().ToString());
                model.Employer = (int)id;
            }

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.[PendingJobs] (JobId, MateId, EmployerId)" +
                    "VALUES (@jId, @mId, @eId); SELECT @@Identity";

                cmd.Parameters.Add("@jId", SqlDbType.Int).Value = model.Id;
                cmd.Parameters.Add("@mId", SqlDbType.Int).Value = model.Mate;
                cmd.Parameters.Add("@eId", SqlDbType.Int).Value = id;

                cmd.ExecuteScalar();
            }

            using (SqlCommand cmdSetJobPostAsDone = _connection.Fetch().CreateCommand())
            {
                cmdSetJobPostAsDone.CommandText = "UPDATE dbo.[JobPosts]" +
                    "SET IsDone = 1 " +
                    "WHERE dbo.[JobPosts].Id = @Id";

                cmdSetJobPostAsDone.Parameters.Add("@Id", SqlDbType.Int).Value = model.JobPost;

                cmdSetJobPostAsDone.ExecuteScalar();
            }

            return model;
        }

        /// <summary>
        /// Método para encontrar um trabalho por Id
        /// </summary>
        /// <param name="id">Id do trabalho pretendido</param>
        /// <returns>Retorna o trabalho pretendido, se nao existir
        /// retorna null </returns>
        public WorkDetailsModel FindById(int id)
        {
            WorkDetailsModel wd = null;
            bool contains = false;
            int _mateId = 0;
            int _employerId = 0;
            int _postId = 0;
            int? _invoiceId = 0;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Date, MateId, JobPostId, InvoiceId, FinishedConfirmedByEmployer, FinishedConfirmedByMate, EmployerId " +
                    "FROM dbo.[Job] " +
                    "WHERE Id=@id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (contains = reader.HasRows)
                    {
                        wd = new WorkDetailsModel();
                        reader.Read();
                        wd.Date = reader.GetDateTime(0);
                        _mateId = reader.GetInt32(1);
                        _postId = reader.GetInt32(2);

                        //guardar o id do invoice para mais tarde fazer o find
                        if (reader.IsDBNull(3))
                        {
                            _invoiceId = null;
                        }
                        else
                        {
                            _invoiceId = reader.GetInt32(3);
                        }

                        wd.FinishedConfirmedByEmployer = reader.GetBoolean(4);
                        wd.FinishedConfirmedByMate = reader.GetBoolean(5);
                        _employerId = reader.GetInt32(6);
                    }
                }
            }

            if (contains)
            {
                IMateDAO<Mate> mateDAO = new MateDAO(_connection);
                Mate mate = mateDAO.FindMateById(_mateId);

                wd.Mate = mate;

                IJobDAO postDao = new JobDAO(_connection);
                JobPost post = postDao.FindById(_postId);

                wd.JobPost = post;


                if (_invoiceId != null)
                {
                    wd.InvoiceId = (int)_invoiceId;
                }

                IEmployerDAO<Employer> employerDAO = new EmployerDAO(_connection);
                Employer employer = employerDAO.FindEmployerById(_employerId);

                wd.Employer = employer;
            }
            else
            {
                return null;
            }

            return wd;
        }

        /// <summary>
        /// Overload do FindById para procurar um work com o employer
        /// na sessão
        /// </summary>
        /// <param name="workId">Id do work</param>
        /// <param name="employerId">Id do employer</param>
        /// <returns>Retorna o Work</returns>
        public WorkDetailsModel FindById(int workId, int employerId)
        {
            WorkDetailsModel wd = null;
            bool contains = false;
            int _mateId = 0;
            int _employerId = 0;
            int _postId = 0;
            int? _invoiceId = 0;

            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Date, MateId, JobPostId, InvoiceId, FinishedConfirmedByEmployer, FinishedConfirmedByMate, EmployerId " +
                    "FROM dbo.[Job] " +
                    "WHERE Id=@id AND EmployerId=@Emp";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = workId;
                cmd.Parameters.Add("@Emp", SqlDbType.Int).Value = employerId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (contains = reader.HasRows)
                    {
                        wd = new WorkDetailsModel();
                        reader.Read();
                        wd.Date = reader.GetDateTime(0);
                        _mateId = reader.GetInt32(1);
                        _postId = reader.GetInt32(2);

                        if (reader.IsDBNull(3))
                        {
                            _invoiceId = null;
                        }
                        else
                        {
                            _invoiceId = reader.GetInt32(3);
                        }

                        wd.FinishedConfirmedByEmployer = reader.GetBoolean(4);
                        wd.FinishedConfirmedByMate = reader.GetBoolean(5);
                        _employerId = reader.GetInt32(6);
                    }
                }
            }

            if (contains)
            {
                IMateDAO<Mate> mateDAO = new MateDAO(_connection);
                Mate mate = mateDAO.FindMateById(_mateId);

                wd.Mate = mate;

                IJobDAO postDao = new JobDAO(_connection);
                JobPost post = postDao.FindById(_postId);

                wd.JobPost = post;


                if (_invoiceId != null)
                {
                    wd.InvoiceId = (int)_invoiceId;
                }

                IEmployerDAO<Employer> employerDAO = new EmployerDAO(_connection);
                Employer employer = employerDAO.FindEmployerById(_employerId);

                wd.Employer = employer;
            }
            else
            {
                return null;
            }

            return wd;
        }

        /// <summary>
        /// Método para confirmar o trabalho como realizado,
        /// removendo-o posteriormente da lista de trabalhos
        /// pendentes do Mate
        /// </summary>
        /// <param name="jobId">Id do trabalho a marcar como concluído</param>
        /// <param name="userId">Id do utilizador que marca o trabalho como 
        /// concluído</param>
        /// <returns>Verdadeiro em caso de sucesso, falso caso contrário</returns>
        public bool MarkJobAsDone(int jobId, int userId)
        {
            WorkDAO workDAO = new WorkDAO(_connection);
            WorkDetailsModel job = workDAO.FindById(jobId);

            if (job == null)
            {
                throw new Exception("ID de trabalho Inválido!");
            }

            EmployerDAO employerDAO = new EmployerDAO(_connection);
            Employer employer = employerDAO.FindEmployerById(userId);

            bool foundEmployer = (employer != null);

            if (foundEmployer)
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE dbo.[Job] SET FinishedConfirmedByEmployer = @fnshdEmployer " +
                        "WHERE Id = @id";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = jobId;
                    cmd.Parameters.Add("@fnshdEmployer", SqlDbType.Bit).Value = true;

                    cmd.ExecuteNonQuery();
                }

                    return true;
            }

            MateDAO mateDAO = new MateDAO(_connection);
            Mate mate = mateDAO.FindMateById(userId);

            bool foundMate = (mate != null);

            if (foundMate)
            {
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE dbo.[Job] SET FinishedConfirmedByMate = @fnshdMate " +
                        "WHERE Id = @id";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = jobId;
                    cmd.Parameters.Add("@fnshdMate", SqlDbType.Bit).Value = true;

                    cmd.ExecuteNonQuery();
                }

                return true;
            }

            throw new Exception("ID do utilizador Inválido!");
        }

        /// <summary>
        /// Método para apagar um Work
        /// </summary>
        /// <param name="toDelete">Id do work a apagar</param>
        /// <returns>Retorna True caso o work seja apagado,
        /// False caso contrário</returns>
        public bool Delete(int toDelete)
        {   
            try{

                bool deleted = false;
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM dbo.[PendingJobs] WHERE JobId=@Id;" +
                    "DELETE FROM dbo.[Job] WHERE Id=@Id;";

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = toDelete;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        deleted = true;
                    }
                }

                return deleted;
                
            } catch (Exception e) {

                throw new Exception(e.Message);

            }
        }

        /// <summary>
        /// Método para atualizar a data de um work
        /// </summary>
        /// <param name="workId">Id do work</param>
        /// <param name="date">Objeto DateModel com a data</param>
        /// <returns>Retorna true se a data for atualizada,
        /// false caso contrário </returns>
        public bool updateDate(int workId, DateModel date){

            try{

                bool updated = false;

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "UPDATE dbo.[Job] " +
                        "SET Date=@date " +
                        "WHERE Id=@id";
                    
                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = date.Date;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = workId;
                    
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        updated = true;
                    }
                }

                return updated;
                
            }catch(Exception e){
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
