using HouseM8API.Entities;

namespace HouseM8API.Interfaces
{
    /// <summary>
    /// Interface de Pagamentos
    /// </summary>
    interface IPaymentDAO
    {
        /// <summary>
        /// Método para obter um invoice através do JobID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Invoice GetInvoiceByID(int userId, int jobId);
        /// <summary>
        /// Método para realizar o pagamento de um trabalho
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="jobId"></param>
        /// <param name="employerId"></param>
        /// <returns></returns>
        Invoice makePayment(Invoice payment, int jobId, int employerId);
        /// <summary>
        /// Método para confirmar um pagamento
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="mateId"></param>
        /// <returns></returns>
        bool confirmPayment(int jobId, int mateId);
    }
}
