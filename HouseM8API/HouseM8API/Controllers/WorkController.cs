using AutoMapper;
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
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controlador com as rotas relativas ao Work
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WorkController : Controller
    {
        private readonly IConnection _connection;
        private readonly IMapper _mapper;

        /// <summary>
        /// Método construtor da classe WorkController
        /// </summary>
        /// <param name="config">Objeto config com a conexão à BD</param>
        /// <param name="mapper">Objeto mapper para mapear entidades</param>
        public WorkController(IOptions<AppSettings> config, IMapper mapper)
        {
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Adiciona um trabalho à plataforma
        /// </summary>
        /// <remarks>
        /// 
        ///     { 
        ///         "Date":"2021-02-19",
        ///         "Mate":37,
        ///         "JobPost":21
        ///     }
        /// 
        /// </remarks>
        /// <param name="work">Trabalho a adicionar</param>
        /// <returns>O trabalho criado</returns>
        /// <response code="200">Retorna o trabalho criado</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<WorkModel> Create(WorkModel work)
        {

            int? employerId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (employerId == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            IWorkDAO WorkDao = new WorkDAO(_connection);
            Job JobModel = _mapper.Map<Job>(work);
            WorkModel result = _mapper.Map<WorkModel>(WorkDao.Create((int)employerId, JobModel));

            if (result == null)
            {
                return BadRequest(new ErrorMessageModel("Mate ou Post não encontrados!"));
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtém um trabalho através do sou ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /find/{id}
        /// </remarks>
        /// <param name="id">Id do trabalho a pesquisar</param>
        /// <returns>O trabalho encontrado</returns>
        /// <response code="200">Retorna o trabalho encontrado</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("find/{id}")]
        [AllowAnonymous]
        public ActionResult<WorkDetailsModel> FindById(int id)
        {
            WorkDAO workDao = new WorkDAO(_connection);
            WorkDetailsModel work = workDao.FindById(id);

            if (work == null)
            {
                return NotFound(new ErrorMessageModel("Trabalho não encontrado"));
            }

            return Ok(work);
        }

        /// <summary>
        /// Rota para marcar um trabalho como concluído
        /// </summary>
        /// <remarks>
        /// 
        ///     POST /markJobAsCompleted/{id}
        ///
        /// </remarks>
        /// <param name="Id">Id do Job a marcar como concluído</param>
        /// <returns>Verdadeiro em caso de sucesso</returns>
        /// <response code="200">Retorna verdadeiro em caso de sucesso</response>
        /// <response code="422">Unprocessable Entity</response>
        [HttpPost]
        [Route("markJobAsCompleted/{id}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult MarkJobAsDone(int Id)
        {
            try
            {
                int userId = (int)Helpers.ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
                WorkDAO workDAO = new WorkDAO(_connection);
                bool result = workDAO.MarkJobAsDone(Id, userId);
                return Ok(new SuccessMessageModel("Result: " + result));
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new ErrorMessageModel(ex.Message));
            }
        }

        

        /// <summary>
        /// Rota para apagar um work criado por um employer
        /// </summary>
        /// <remarks>
        /// 
        ///     Delete /DeleteWork/{id}
        ///
        /// </remarks> 
        /// <param name="id">Id do work</param>
        /// <returns>Retorna mensagem de sucesso</returns>
        /// <response code="200">Retorna o trabalho criado</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        [HttpDelete]
        [Route("DeleteWork/{id}")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<SuccessMessageModel> Delete(int id){

            try{

                int? employerId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (employerId == null)
                {
                    return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
                }

                WorkDAO workDao = new WorkDAO(_connection);
                WorkDetailsModel work = workDao.FindById(id, (int)employerId);

                if(work == null){
                    return BadRequest(new ErrorMessageModel("O trabalho não existe ou não está associado ao Employer!"));
                }

                bool deleted = workDao.Delete(id);

                if(deleted){
                    return Ok(new SuccessMessageModel("Trabalho apagado com sucesso!"));
                } else {
                    return BadRequest(new ErrorMessageModel("Erro! O trabalho não foi apagado!"));
                }

            } catch(Exception e) {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
            
        }

        /// <summary>
        /// Rota para atualizar/reagendar a data do work
        /// </summary>
        /// <remarks>
        /// 
        ///     {
        ///      "Date": "2021-02-19"
        ///     }
        ///
        /// </remarks>  
        /// <param name="id">Id do Work</param>
        /// <param name="date">Data nova</param>
        /// <returns>Retorna mensagem de sucesso</returns>
        /// <response code="200">Retorna o trabalho criado</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        [HttpPut]
        [Route("RescheduleWork/{id}")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<SuccessMessageModel> Reschedule(int id, DateModel date){

            try{
                int? employerId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (employerId == null)
                {
                    return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
                }

                WorkDAO workDao = new WorkDAO(_connection);
                WorkDetailsModel work = workDao.FindById(id, (int)employerId);

                if(work == null){
                    return BadRequest(new ErrorMessageModel("O trabalho não existe ou não está associado ao Employer!"));
                }

                bool updated = workDao.updateDate(id, date);
            
                if(updated){
                    return Ok(new SuccessMessageModel("Data atualizada com sucesso!"));
                } else {
                    return BadRequest(new ErrorMessageModel("Erro! A data não foi atualizada!"));
                }

            } catch(Exception e){
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

    }
}
