using AutoMapper;
using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controller para a tarefa relacionada com o modulo de report de um utilizador
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IConnection _connection;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do Controller
        /// </summary>
        /// <param name="config"></param>
        /// <param name="mapper"></param>
        public ReportController(IOptions<AppSettings> config, IMapper mapper)
        {
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Rota para a criação de um report de um utilizador
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="repModel"></param>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         "comment" : "Estragou me o Jardim!",
        ///         "reason" : "INADEQUATE_BEHAVIOUR" 
        ///     }
        /// </remarks> 
        /// <returns>O report model</returns>
        /// <response code="200">Retorna ReportModel</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("user/{userId}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult<ReportModel> ReportUser(int userId, ReportModel repModel)
        {
            int? userID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            // Caso o user seja nulo
            if (userID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            // Caso o user dê report a si mesmo
            if (userID == userId)
            {
                return BadRequest(new ErrorMessageModel("Nao se pode reportar a si proprio"));
            }

            Report report = _mapper.Map<Report>(repModel);
            IReportDAO ReportDAO = new ReportDAO(_connection);
            ReportModel reportModel = _mapper.Map<ReportModel>(ReportDAO.ReportUser((int)userID, userId, report));
            return Ok(reportModel);
        }
    }
}
