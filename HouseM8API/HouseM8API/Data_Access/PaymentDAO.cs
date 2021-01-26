using Enums;
using HouseM8API.Entities;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// Classe para realizar os diferentes tipos de pagamento
    /// </summary>
    public class PaymentDAO : IPaymentDAO
    {

        private readonly IConnection _connection;

        /// <summary>
        /// Constructor da Classe
        /// </summary>
        /// <param name="connection"></param>
        public PaymentDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método para obter um Invoice por Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jobId"></param>
        /// <returns>Invoice ou Exception</returns>
        public Invoice GetInvoiceByID(int userId, int jobId)
        {
            WorkDAO workDAO = new WorkDAO(_connection);
            WorkDetailsModel job = workDAO.FindById(jobId);

            if (job == null)
            {
                throw new Exception("Trabalho não existe!");
            }

            if (job.Employer.Id != userId && job.Mate.Id != userId)
            {
                throw new Exception("Não tem autorização para aceder a este pagamento!");
            }

            Invoice invoice = null;
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select Id, Value, PaymentTypeId, Date, ConfirmedPayment FROM dbo.[Invoice] where Id=@id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = job.InvoiceId;

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    invoice = new Invoice();
                    reader.Read();

                    invoice.Id = reader.GetInt32(0);
                    invoice.Value = reader.GetDouble(1);
                    invoice.PaymentType = (Payment)reader.GetInt32(2);
                    invoice.Date = reader.GetDateTime(3);
                    invoice.ConfirmedPayment = reader.GetBoolean(4);
                }
            }

            if (invoice == null)
            {
                throw new Exception("Pagamento ainda não foi realizado!");
            }

            return invoice;
        }

        /// <summary>
        /// Método de verificação dos dados do pagamento
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="jobId"></param>
        /// <param name="employerId"></param>
        /// <returns>Invoice ou Exception</returns>
        public Invoice makePayment(Invoice payment, int jobId, int employerId)
        {
            WorkDAO workDAO = new WorkDAO(_connection);
            WorkDetailsModel job = workDAO.FindById(jobId);

            payment.ConfirmedPayment = false;

            if (job == null)
            {
                throw new Exception("Trabalho não existe!");
            }

            if (job.Employer.Id != employerId)
            {
                throw new Exception("Não tem acesso a este trabalho!");
            }

            if (!job.FinishedConfirmedByEmployer || !job.FinishedConfirmedByMate)
            {
                throw new Exception("Trabalho não foi confirmado como terminado!");
            }

            bool paymentTypeAllowed = false;

            for (int i = 0; i < job.JobPost.PaymentMethod.Length; i++)
            {
                if (job.JobPost.PaymentMethod[i].Equals(payment.PaymentType))
                {
                    paymentTypeAllowed = true;
                    break;
                }
            }

            if (!paymentTypeAllowed)
            {
                throw new Exception("Método de pagamento inválido para este trabalho!");
            }

            if (payment.PaymentType.Equals(Payment.PAYPAL))
            {
                payment = MakePaymentWithPayPal(payment, job, jobId);
            }

            if (payment.PaymentType.Equals(Payment.CRYPTO))
            {
                Console.WriteLine("Pagamento feito com Crypto!");
            }

            if (payment.PaymentType.Equals(Payment.MBWAY))
            {
                Console.WriteLine("Pagamento feito com MBWay!");
            }

            if (payment.PaymentType.Equals(Payment.MONEY))
            {
                payment = MakePaymentWithMoney(payment, job, jobId);
            }

            return payment;
        }

        /// <summary>
        /// Método para confirmar o pagamento
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="mateId"></param>
        /// <returns>bool ou Exception</returns>
        public bool confirmPayment(int jobId, int mateId)
        {
            WorkDAO workDAO = new WorkDAO(_connection);
            WorkDetailsModel job = workDAO.FindById(jobId);

            if (job == null)
            {
                throw new Exception("Trabalho não existe!");
            }

            if (job.Mate.Id != mateId)
            {
                throw new Exception("Não tem acesso a este trabalho!");
            }

            Invoice invoice = this.GetInvoiceByID(mateId, jobId);

            if (invoice == null)
            {
                throw new Exception("Pagamento ainda não foi realizado!");
            }

            using (SqlCommand cmdSetInvoice = _connection.Fetch().CreateCommand())
            {
                cmdSetInvoice.CommandText = "UPDATE dbo.[Invoice]" +
                    "SET ConfirmedPayment = @cfrmdPmnt " +
                    "WHERE dbo.[Invoice].Id = @Id";

                cmdSetInvoice.Parameters.Add("@Id", SqlDbType.Int).Value = invoice.Id;
                cmdSetInvoice.Parameters.Add("@cfrmdPmnt", SqlDbType.Bit).Value = true;

                cmdSetInvoice.ExecuteScalar();
            }

            return true;
        }

        /// <summary>
        /// Método para realizar o pagamento com Dinheiro vivo
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="job"></param>
        /// <param name="jobId"></param>
        /// <returns>Invoice ou Exception</returns>
        private Invoice MakePaymentWithMoney(Invoice payment, WorkDetailsModel job, int jobId)
        {

            if (!job.JobPost.Tradable)
            {
                if (job.JobPost.InitialPrice.Equals(payment.Value))
                {

                    using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO dbo.[Invoice] (Value, PaymentTypeId, Date, ConfirmedPayment)" +
                            "VALUES (@vl, @ptId, @dt, @cnfrdPmnt); SELECT @@Identity";

                        cmd.Parameters.Add("@vl", SqlDbType.Float).Value = payment.Value;
                        cmd.Parameters.Add("@ptId", SqlDbType.Int).Value = payment.PaymentType;
                        cmd.Parameters.Add("@dt", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@cnfrdPmnt", SqlDbType.Bit).Value = payment.ConfirmedPayment;

                        payment.Id = int.Parse(cmd.ExecuteScalar().ToString());
                    }

                    using (SqlCommand cmdSetInvoice = _connection.Fetch().CreateCommand())
                    {
                        cmdSetInvoice.CommandText = "UPDATE dbo.[Job]" +
                            "SET InvoiceId = @pId " +
                            "WHERE dbo.[Job].Id = @jid";

                        cmdSetInvoice.Parameters.Add("@pId", SqlDbType.Int).Value = payment.Id;
                        cmdSetInvoice.Parameters.Add("@jid", SqlDbType.Int).Value = jobId;

                        cmdSetInvoice.ExecuteScalar();
                    }

                    return payment;
                }

                throw new Exception("Preço diferente do negociado!");
            }
            else
            {
                throw new Exception("Preço Negociavel ainda não foi implementado!");
            }

        }

        /// <summary>
        /// Método para realizar o pagamento com Paypal
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="job"></param>
        /// <param name="jobId"></param>
        /// <returns>Invoice ou Exception</returns>
        private Invoice MakePaymentWithPayPal(Invoice payment, WorkDetailsModel job, int jobId)
        {

            if (!job.JobPost.Tradable)
            {
                if (job.JobPost.InitialPrice.Equals(payment.Value))
                {

                    using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO dbo.[Invoice] (Value, PaymentTypeId, Date, ConfirmedPayment)" +
                            "VALUES (@vl, @ptId, @dt, @cnfrdPmnt); SELECT @@Identity";

                        cmd.Parameters.Add("@vl", SqlDbType.Float).Value = payment.Value;
                        cmd.Parameters.Add("@ptId", SqlDbType.Int).Value = payment.PaymentType;
                        cmd.Parameters.Add("@dt", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@cnfrdPmnt", SqlDbType.Bit).Value = payment.ConfirmedPayment;

                        payment.Id = int.Parse(cmd.ExecuteScalar().ToString());
                    }

                    using (SqlCommand cmdSetInvoice = _connection.Fetch().CreateCommand())
                    {
                        cmdSetInvoice.CommandText = "UPDATE dbo.[Job]" +
                            "SET InvoiceId = @pId " +
                            "WHERE dbo.[Job].Id = @jid";

                        cmdSetInvoice.Parameters.Add("@pId", SqlDbType.Int).Value = payment.Id;
                        cmdSetInvoice.Parameters.Add("@jid", SqlDbType.Int).Value = jobId;

                        cmdSetInvoice.ExecuteScalar();
                    }

                    //Change in production
                    string email = "sb-w7dsp4008171@business.example.com";
                    payment.Link = String.Format("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&amount={0}&currency_code=EUR&business={1}&item_name={2}&return=Page", job.JobPost.InitialPrice, email, job.JobPost.Title);

                    return payment;
                }

                throw new Exception("Preço diferente do negociado!");
            }
            else
            {
                throw new Exception("Preço Negociavel ainda não foi implementado!");
            }

        }

    }
}
