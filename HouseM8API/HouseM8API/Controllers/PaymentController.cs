using AutoMapper;
using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using HouseM8API.Models.ReturnedMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controller para a realização de Payments
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {

        private readonly IConnection _connection;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do Controller
        /// </summary>
        /// <param name="config"></param>
        /// <param name="mapper"></param>
        public PaymentController(IOptions<AppSettings> config, IMapper mapper)
        {
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Rota para obter um Invoice
        /// <param name="jobId"></param>
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /payment/getInvoice/10
        /// </remarks>
        /// <returns>Invoice ou Exception</returns>
        /// <response code="200">Retorna Invoice</response>
        /// <response code="422">Unprocessable Entity</response>
        [HttpGet]
        [Route("getInvoice/{jobId}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult<InvoiceModel> GetInvoice(int jobId)
        {
            try
            {
                int? userId = Helpers.ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
                PaymentDAO paymentDAO = new PaymentDAO(_connection);
                InvoiceModel invoice = _mapper.Map<InvoiceModel>(paymentDAO.GetInvoiceByID((int)userId, jobId));
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new ErrorMessageModel(ex.Message));
            }

        }

        /// <summary>
        /// Rota para realizar o pagamento
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="payment"></param>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         "value" : 15.00,
        ///         "paymentType" : "PAYPAL" 
        ///     }
        /// </remarks>
        /// <returns>Invoice ou Exception</returns>
        /// <response code="200">Retorna Invoice</response>
        /// <response code="422">Unprocessable Entity</response>
        [HttpPost]
        [Route("makePayment/{jobId}")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<InvoiceModel> MakePayment(int jobId, [FromBody] InvoiceModel payment)
        {
            try
            {
                int? employerId = Helpers.ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
                PaymentDAO paymentDAO = new PaymentDAO(_connection);
                Invoice invoice = _mapper.Map<Invoice>(payment);
                InvoiceModel invoiceModel = _mapper.Map<InvoiceModel>(paymentDAO.makePayment(invoice, jobId, (int)employerId));
                return Ok(invoiceModel);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new ErrorMessageModel(ex.Message));
            }
        }

        /// <summary>
        /// Rota para confirmar o pagamento
        /// </summary>
        /// <param name="jobId"></param>
        /// <remarks>
        /// Sample request:
        ///     POST /payment/confirmPayment/10
        /// </remarks>
        /// <returns>Mensagem ou Exception</returns>
        /// <response code="200">Retorna Mensagem de Sucesso</response>
        /// <response code="422">Unprocessable Entity</response>
        [HttpPost]
        [Route("confirmPayment/{jobId}")]
        [Authorize(Roles = "M8")]
        public ActionResult ConfirmPayment(int jobId)
        {
            try
            {
                int? mateId = Helpers.ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
                PaymentDAO paymentDAO = new PaymentDAO(_connection);
                bool result = paymentDAO.confirmPayment(jobId, (int)mateId);
                return Ok(new SuccessMessageModel("Resultado: " + result));
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new ErrorMessageModel(ex.Message));
            }
        }
    }
}
