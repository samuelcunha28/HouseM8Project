using AutoMapper;
using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using HouseM8API.Models.ReturnedMessages;
using HouseM8API.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controlador com as rotas relativas
    /// ao perfil do Employer
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]

    public class EmployerProfileController : ControllerBase
    {
        private readonly IConnection _connection;
        private readonly IMapper _mapper;

        /// <summary>
        /// Método construtor da classe EmployerProfileController
        /// </summary>
        /// <param name="config">Config com a conexão à BD</param>
        /// <param name="mapper">Mapper para mapear entidades</param>
        public EmployerProfileController(IOptions<AppSettings> config, IMapper mapper)
        {
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Atualiza os dados pessoais do Employer
        /// </summary>
        /// <remarks>
        /// 
        ///     {
        ///      "FirstName": "Jessica", 
        ///      "LastName": "Silva",
        ///      "Description": "Sou um employer!",
        ///      "UserName": "JessicaaSilvaa",
        ///      "Address": {
        ///          "street": "Rua das Veigas",
        ///          "streetNumber": 377,
        ///          "postalCode": "4620-471",
        ///          "district": "Porto",
        ///          "country": "Portugal"
        ///      }
        ///     }
        ///     
        /// </remarks>
        /// <returns>Mensagem de sucesso ou Mensagem de erro</returns>
        /// <response code="200">Update successful</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="400">Bad Request</response>
        [HttpPut("update")]
        [Authorize(Roles = "EMPLOYER")]
        public IActionResult Update(EmployerUpdate employerUpdate)
        {
            try
            {
                int? id = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
                if (id == null)
                {
                    return Unauthorized("Sem Autorização ou sem sessão inciada");
                }

                Employer employer = _mapper.Map<Employer>(employerUpdate);
                EmployerDAO employerDAO = new EmployerDAO(_connection);
                employerDAO.Update(employer, (int) id);

                return Ok(new SuccessMessageModel("Campos Atualizados!"));

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
        }

        /// <summary>
        /// Obtém a lista de mates favoritos de um employer
        /// </summary>
        /// <returns>Lista de Mates favoritos do Employer na sessão</returns>
        /// <response code="200">Lista de Mates favoritos</response>
        /// <response code="404">Not Found</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response> 
        [HttpGet("favlist")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<List<FavoriteModel>> FavoritesList()
        {
            try
            {
                int? id = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (id == null)
                {
                    return Unauthorized(new ErrorMessageModel("Sem Autorização ou sem sessão inciada"));
                }

                EmployerDAO employerDAO = new EmployerDAO(_connection);

                List<FavoriteModel> favorites = employerDAO.FavoritesList((int)id).ToList();

                if (favorites == null)
                {
                    return NotFound(new ErrorMessageModel("Lista de favoritos Inexistente!"));
                }

                return favorites;

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
        }

        /// <summary>
        /// Adiciona um Mate à lista de Mates favoritos do Employer
        /// </summary>
        /// <remarks>
        ///     
        ///     { 
        ///      "email": "je@gmail.com",
        ///      "UserName" : Jessica
        ///     }
        /// 
        /// </remarks>
        /// <param name="mate">Modelo do Mate que se quer adicionar à lista</param>
        /// <returns>Retorna o Mate adicionado</returns>
        /// <response code="200">Mate adicionado como favorito</response>
        /// <response code="404">NotFound</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("addfav")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<SuccessMessageModel> AddFavorite(FavoriteModel mate)
        {

            int? EmployerId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
            if (EmployerId == null)
            {
                return Unauthorized("Sem Autorização ou sem sessão inciada");
            }

            EmployerDAO employerDAO = new EmployerDAO(_connection);
            int? mateID = employerDAO.FindMateByEmail(mate.Email);

            if (mateID == null)
            {
                return NotFound(new ErrorMessageModel("Utilizador não existe!"));
            }

            int? returnedId = employerDAO.AddFavorite((int)EmployerId, (int)mateID);

            return Ok(new SuccessMessageModel("ID do Mate adicionado: " + returnedId));
        }

        /// <summary>
        /// Remove o Mate da lista de favoritos de um Employer
        /// </summary>
        /// <remarks>
        ///     
        ///     { 
        ///      "UserName": "dev",
        ///      "Email": "dev@gmail.com" 
        ///     }
        /// 
        /// </remarks>
        /// <param name="favoriteModel">Modelo do Mate que pretende remover</param>
        /// <returns>O id do Mate removido</returns>
        /// <response code="200">Retorna o Id do Mate removidp</response>
        /// <response code="404">Not Found</response>
        /// <response code="403">Forbidden</response> 
        [HttpDelete("delfav")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<SuccessMessageModel> RemoveFavorite(FavoriteModel favoriteModel)
        {
            int? EmployerId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
            if (EmployerId == null)
            {
                return Unauthorized("Sem Autorização ou sem sessão inciada");
            }

            EmployerDAO employerDAO = new EmployerDAO(_connection);
            int? mateID = employerDAO.FindMateByEmail(favoriteModel.Email);

            if (mateID == null)
            {
                return NotFound(new ErrorMessageModel("Utilizador não existe!"));
            }

            int? returnedId = employerDAO.RemoveFavorite((int)EmployerId, (int)mateID);

            return Ok(new SuccessMessageModel("ID do Mate removido: " + returnedId));
        }

        /// <summary>
        /// Obtém o Employer através do ID
        /// </summary>
        /// <remarks>
        /// 
        ///     GET /api/EmployerProfile/11
        /// 
        /// </remarks>
        /// <param name="id">Id do Employer a pesquisar</param>
        /// <returns>O Employer pesquisado</returns>
        /// <response code="200">Retorna o Employer pesquisado</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult<EmployerProfileModel> GetEmployerById(int id)
        {
            try
            {
                EmployerDAO employerDAO = new EmployerDAO(_connection);
                EmployerProfileModel returned = _mapper.Map<EmployerProfileModel>(employerDAO.FindEmployerById(id));

                return Ok(returned);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }

        }

        /// <summary>
        /// Obtém a lista de trabalhos pendentes de um Employer
        /// </summary>
        /// <remarks>
        ///     GET /pending
        /// </remarks>
        /// <returns>A lista de trabalhos pendentes</returns>
        /// <response code="200">Retorna a lista de trabalhos pendentes</response>
        /// <response code="404">Not Found</response>
        [Authorize(Roles = "EMPLOYER")]
        [HttpGet("pending")]
        public ActionResult<List<PendingJobModel>> PendingJobsList()
        {
            int? employerID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (employerID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            EmployerDAO employerDAO = new EmployerDAO(_connection);
            List<PendingJobModel> pendingJobs = employerDAO.PendingJobsList((int)employerID).ToList();

            return Ok(pendingJobs);
        }
    }
}
