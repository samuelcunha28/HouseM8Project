using AutoMapper;
using Enums;
using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using HouseM8API.Models.ReturnedMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controller para a tarefas relacionadas com o modulo de procura de trabalho
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WorkSearchController : Controller
    {

        private readonly IConnection _connection;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do Controller
        /// </summary>
        /// <param name="config"></param>
        /// <param name="mapper"></param>
        public WorkSearchController(IOptions<AppSettings> config, IMapper mapper)
        {
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Rota de pesquisa de trabalho disponível para o mate com vários filtros disponíveis
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="address"></param>
        /// <param name="distance"></param>
        /// <param name="rating"></param>
        /// <remarks>
        /// 
        ///     GET /worksearch?categories=PLUMBING&amp;categories=GARDENING&amp;address=asddfdsdfsd&amp;distance=400
        /// 
        /// </remarks>
        /// <returns>Endereço ou Exception</returns>
        /// <response code="200">Retorna uma Lista de JobPost disponíveis</response>
        /// <response code="404">Not Found</response>
        /// <response code="422">Unprocessable Entity</response>
        [HttpGet]
        [Authorize(Roles = "M8")]
        public ActionResult<List<JobPostReturnedModel>> GetAvailableWorks([FromQuery(Name = "categories")] Categories[] categories, [FromQuery(Name = "address")] string address, [FromQuery(Name = "distance")] int? distance, [FromQuery(Name = "rating")] int? rating)
        {
            try
            {
                int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (mateID == null)
                {
                    return NotFound(new ErrorMessageModel("Utilizador não existe!"));
                }

                MateDAO mateDao = new MateDAO(_connection);
                Mate mate = mateDao.FindMateById((int)mateID);

                if (address == null)
                {
                    address = mate.Address;
                }

                IJobDAO jobDAO = new JobDAO(_connection);
                List<JobPostReturnedModel> postsList = _mapper.Map<List<JobPostReturnedModel>>(jobDAO.GetJobs(categories, address, distance, rating, (int)mateID));

                return Ok(postsList);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new ErrorMessageModel(ex.Message));
            }

        }

        /// <summary>
        /// Rota para realizar uma oferta de preço a um trabalho selecionado
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="offer"></param>
        /// <remarks>
        ///     
        ///     {
        ///         "price": 20
        ///     }
        /// 
        /// </remarks>
        /// <returns>Oferta realizada ou Exception</returns>
        /// <response code="200">Retorna a oferta realizada</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        [HttpPost("makeOffer/{id}")]
        [Authorize(Roles = "M8")]
        public ActionResult<OfferModel> MakeOfferOnJobPost(int Id, [FromBody] OfferModel offer)
        {
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            IJobDAO jobDAO = new JobDAO(_connection);
            JobPost job = jobDAO.FindById(Id);

            if (job == null)
            {
                return NotFound(new ErrorMessageModel("Trabalho não encontrado!"));
            }

            Offer newOffer = _mapper.Map<Offer>(offer);
            newOffer.JobPost = job;
            OfferModel resultOffer = _mapper.Map<OfferModel>(jobDAO.makeOfferOnJob(newOffer, mateID));

            return Ok(resultOffer);
        }

        /// <summary>
        /// Rota para ignorar um jobPost
        /// </summary>
        /// <param name="job"></param>
        /// <remarks>
        ///     
        ///     {
        ///         "id": 10
        ///     }
        /// 
        /// </remarks>
        /// <returns>Mensagem de Sucesso ou Exception</returns>
        /// <response code="200">Retorna a Mensagem de Sucesso</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unprocessable Entity</response>

        [HttpPost("ignoreJobPost")]
        [Authorize(Roles = "M8")]
        public ActionResult IgnoreJobPost([FromBody] IgnoredJobModel job)
        {
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            MateDAO mateDAO = new MateDAO(_connection);
            bool result = mateDAO.IgnoreJobPost((int)mateID, job);

            if (!result)
            {
                return UnprocessableEntity(new ErrorMessageModel("Publicação Inválida!"));
            }

            return Ok(new SuccessMessageModel("Result : " + result));
        }
    }
}
