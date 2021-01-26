using AutoMapper;
using Enums;
using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using HouseM8API.Models.ReturnedMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Classe controlador de JobPosts
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class JobPostsController : ControllerBase
    {
        private readonly IConnection _connection;
        private readonly IMapper _mapper;

        private IWebHostEnvironment _environment;

        /// <summary>
        /// Construtor do controller de JobPosts
        /// </summary>
        /// <param name="config">Objeto config com a conexão à BD</param>
        /// <param name="mapper">Objeto mapper para mapear entidades</param>
        /// <param name="environment">Fornece informações sobre o ambiente de 
        /// hosting na web em que o aplicativo está a ser executado</param>
        public JobPostsController(IOptions<AppSettings> config, IMapper mapper, IWebHostEnvironment environment)
        {
            _environment = environment;
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Rota para criar publicação
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///     "title": "Pintura de quarto",
        ///     "Category": "INTERIOR_DESIGN",
        ///     "ImagePath":"",
        ///     "Description": "Pintar paredes de verde às pintas amarelas",
        ///     "Tradable": false,
        ///     "InitialPrice": 150,
        ///     "Address":  {
        ///         "street": "Rua das Veigas",
        ///         "streetNumber": 377,
        ///         "postalCode": "4620-471",
        ///         "district": "Porto",
        ///         "country": "Portugal"
        ///      },
        ///     "PaymentMethod": ["MONEY", "MBWAY"]
        ///     }
        /// 
        /// </remarks>
        /// <param name="model">Modelo com a informação da publicação</param>
        /// <returns>Retorna a publicação criada</returns>
        /// <response code="200">Retorna a publicação adicionada</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        [HttpPost]
        [Route("CreatePost")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<JobPost> Create(JobPostModel model)
        {
            try
            {
                int? employerID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (employerID == null)
                {
                    return Unauthorized();
                }

                JobPost post = _mapper.Map<JobPost>(model);
                IJobDAO JobPostDAO = new JobDAO(_connection);
                JobPost JobPosts = JobPostDAO.Create((int)employerID, post);
                return Ok(JobPosts);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
        }

        /// <summary>
        /// Rota para apagar publicação
        /// </summary>
        /// <param name="id">Id da publicação a ser apagada</param>
        /// <returns>Retorna uma mensagem de sucesso ou de erro</returns>
        /// <response code="200">Retorna uma mensagem de sucesso</response>
        /// <response code="404">Not Found</response>
        /// <response code="403">Forbidden</response>
        [HttpDelete]
        [Route("DeletePost/{id}")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<SuccessMessageModel> Delete(int id)
        {
            int? employerID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (employerID == null)
            {
                return Unauthorized();
            }

            string rootPath = this._environment.ContentRootPath;
            IJobDAO JobDAO = new JobDAO(_connection, rootPath);
            JobPost toDel = JobDAO.FindById(id);

            if (toDel == null)
            {
                return NotFound(new ErrorMessageModel("Post não encontrado!"));
            }

            JobDAO.Delete(toDel);
            return Ok(new SuccessMessageModel("Apagado!"));
        }

        /// <summary>
        /// Rota para obter todos as publicações de um employer
        /// </summary>
        /// <returns>Lista de publicações</returns>
        /// <response code="200">Retorna lista de publicações do employer</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("EmployerPosts")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<List<JobPostReturnedModel>> GetAllEmployerPosts()
        {
            int? employerID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (employerID == null)
            {
                return Unauthorized();
            }

            IJobDAO jobDAO = new JobDAO(_connection);
            List<JobPostReturnedModel> listPosts = _mapper.Map<List<JobPostReturnedModel>>(jobDAO.GetEmployerPosts((int)employerID));
            return Ok(listPosts);
        }

        /// <summary>
        /// Rota para atualizar dados de uma publicação
        /// </summary>
        /// <remarks>
        ///     
        ///     {
        ///     "title": "Pintura de quarto",
        ///     "Category": "INTERIOR_DESIGN",
        ///     "ImagePath":"",
        ///     "Description": "Pintar paredes de verde às pintas amarelas",
        ///     "Tradable": false,
        ///     "InitialPrice": 150,
        ///     "Address":  {
        ///         "street": "Rua das Veigas",
        ///         "streetNumber": 377,
        ///         "postalCode": "4620-471",
        ///         "district": "Porto",
        ///         "country": "Portugal"
        ///      },
        ///     "PaymentMethod": ["MONEY", "MBWAY"]
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">Id do jobpost a ser atualizado</param>
        /// <param name="model">Modelo com dados da publicação</param>
        /// <returns>Retorna publicação atualizada</returns>
        /// <response code="200">Retorna publicação atualizada</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        [HttpPut]
        [Route("updatePost/{id}")]
        [Authorize(Roles = "EMPLOYER")]
        public IActionResult Update(int id, JobPostModel model)
        {
            try
            {
                int? employerID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (employerID == null)
                {
                    return Unauthorized();
                }

                IJobDAO JobPostDAO = new JobDAO(_connection);
                JobPost oldPost = JobPostDAO.FindById(id, (int)employerID);

                if (oldPost == null)
                {
                    return NotFound(new ErrorMessageModel("Post não encontrado!"));
                }

                JobPost newPost = _mapper.Map<JobPost>(model);

                oldPost.Title = newPost.Title;
                oldPost.Category = newPost.Category;
                oldPost.ImagePath = newPost.ImagePath;
                oldPost.Description = newPost.Description;
                oldPost.Tradable = newPost.Tradable;
                oldPost.InitialPrice = newPost.InitialPrice;
                oldPost.Address = newPost.Address;
                oldPost.PaymentMethod = newPost.PaymentMethod;

                JobPost updatedPost = JobPostDAO.UpdatePostDetails(oldPost);

                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }

        }

        /// <summary>
        /// Adiciona um ou mais tipos de pagamento
        /// a uma publicação de trabalho
        /// </summary>
        /// <remarks>
        ///     
        ///     [
        ///      { 
        ///       "payments": "CRYPTO" 
        ///      }
        ///     ]
        /// 
        /// </remarks>
        /// <param name="jobId">Id do Job onde se pretende adicionar
        /// um método de pagamento</param>
        /// <param name="payment">Método de pagamento a adicionar</param>
        /// <returns>Os tipos de pagamento adicionados</returns>
        /// <response code="200">Retorna os tipos de pagamento adicionados</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        [HttpPost("addpayments/{jobId}")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<Payment> AddPayment(int jobId, PaymentModel[] payment)
        {
            IJobDAO jobDAO = new JobDAO(_connection);
            JobPost post = jobDAO.FindById(jobId);

            if (post == null)
            {
                return NotFound(new ErrorMessageModel("Post não encontrado!"));
            }

            jobDAO.AddPayment(jobId, payment);

            return Ok(payment);
        }

        /// <summary>
        /// Retorna uma publicação de trabalho através do ID
        /// </summary>
        /// <param name="id">Id do JobPost a pesquisar</param>
        /// <returns>A publicação de trabalho correspondente ao ID procurado</returns>
        /// <response code="200">Retorna a publicação de trabalho correspondente</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<JobPost> GetById(int id)
        {
            IJobDAO jobDAO = new JobDAO(_connection);
            JobPost Post = jobDAO.FindById(id);

            if (Post == null)
            {
                return NotFound(new ErrorMessageModel("Post não encontrado!"));
            }

            return Ok(Post);
        }

        /// <summary>
        /// Retorna "Deleted" quando uma tipo de pagamento
        /// de uma publicação de trabalho é removido com
        /// sucesso
        /// </summary>
        /// <remarks>
        ///     
        ///     {
        ///      "payments": "CRYPTO"
        ///     }
        /// 
        /// </remarks>
        /// <param name="jobId">Id do JobPost onde se pretende remover
        /// um método de pagamento</param>
        /// <param name="payment">Método de pagamento a remover</param>
        /// <returns>
        /// "Deleted" caso o tipo de pagamento
        /// seja removido com sucesso
        /// </returns>
        /// <response code="200">Retorna mensagem de sucesso</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        [HttpDelete("removepayment/{jobId}")]
        [Authorize(Roles = "EMPLOYER")]
        public IActionResult RemovePayment(int jobId, PaymentModel payment)
        {
            IJobDAO jobDAO = new JobDAO(_connection);
            JobPost post = jobDAO.FindById(jobId);

            if (post == null)
            {
                return NotFound(new ErrorMessageModel("Post não encontrado!"));
            }

            jobDAO.RemovePayment(jobId, payment);

            return Ok(new SuccessMessageModel("Apagado!"));
        }

        /// <summary>
        /// Rota para fazer upload de imagens para um post
        /// de um employer
        /// </summary>
        /// <param name="id">id do post</param>
        /// <param name="images">coleção de imagens</param>
        /// <param name="mainImage">Imagem de destaque</param>
        /// <response code="200">Caminhos das imagens</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("uploadImages/{id}")]
        [Authorize(Roles = "EMPLOYER")]
        public IActionResult UploadImagesToPost(int id,
        [FromForm] IFormFileCollection images,
        [FromForm] IFormFile mainImage)
        {
            try
            {
                int? employerID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (employerID == null)
                {
                    return Unauthorized(new ErrorMessageModel("Não Autorizado!"));
                }

                IJobDAO jobDAO = new JobDAO(_connection, this._environment.ContentRootPath);

                if (jobDAO.FindById(id, (int)employerID) == null)
                {
                    return NotFound(new ErrorMessageModel("Post não encontrado!"));
                }

                SuccessMessageModel message = jobDAO.UploadImagesToPost(id, images, mainImage);

                return Ok(message);

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Rota para obter uma lista com os 
        /// nomes das imagens de um jobPost
        /// </summary>
        /// <param name="post">Id do jobPost</param>
        /// <returns>Lista com nomes de imagens</returns>
        /// <response code="200">Lista com nomes de imagens</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("postImages/{post}")]
        [AllowAnonymous]
        public IActionResult getPicturesNames(int post)
        {
            try
            {
                JobDAO jobDAO = new JobDAO(_connection, this._environment.ContentRootPath);
                List<ImageName> images = jobDAO.getImages(post);
                return Ok(images);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }

        }

        /// <summary>
        /// Rota para obter o nome da imagem de
        /// destaque de um jobPost
        /// </summary>
        /// <param name="post">Id do jobPost</param>
        /// <returns>Nome da imagem de destaque</returns>
        /// <response code="200">Nome da imagem de destaque</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("postMainImage/{post}")]
        [AllowAnonymous]
        public IActionResult getMainPicture(int post)
        {
            try
            {
                JobDAO jobDAO = new JobDAO(_connection, this._environment.ContentRootPath);
                ImageName image = jobDAO.getMainImage(post);
                return Ok(image);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Rota para apagar a imagem de destaque de um Post
        /// </summary>
        /// <param name="post">Id do Jobpost</param>
        /// <param name="image">Objeto imageName com o nome da imagem</param>
        /// <returns>Imagem apagada!</returns>
        /// <response code="200">Imagem apagada!</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        [HttpDelete("deleteMainImage/{post}")]
        [Authorize(Roles = "EMPLOYER")]
        public IActionResult deleteMainPicture(int post, ImageName image)
        {

            int? id = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (id == null)
            {
                return Unauthorized(new ErrorMessageModel("Sem Autorização ou sem sessão inciada"));
            }

            if (image == null)
            {
                return BadRequest(new ErrorMessageModel("Nome de imagem não enviado!"));
            }

            try
            {
                JobDAO jobDAO = new JobDAO(_connection, this._environment.ContentRootPath);
                bool deleted = jobDAO.deleteMainImage((int)id, post, image);

                if (deleted == true)
                {
                    return Ok(new SuccessMessageModel("Imagem apagada!"));
                }
                else
                {
                    return BadRequest(new ErrorMessageModel("Imagem não apagada ou inexistente!"));
                }

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Rota para apagar a imagens de um Post
        /// </summary>
        /// <param name="post">Id do Jobpost</param>
        /// <param name="image">Objeto imageName com o nome da imagem</param>
        /// <returns>Imagem apagada!</returns>
        /// <response code="200">Imagem apagada!</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        [HttpDelete("deleteImage/{post}")]
        [Authorize(Roles = "EMPLOYER")]
        public IActionResult deleteImage(int post, ImageName image)
        {

            int? id = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (id == null)
            {
                return Unauthorized(new ErrorMessageModel("Sem Autorização ou sem sessão inciada"));
            }

            if (image == null)
            {
                return BadRequest(new ErrorMessageModel("Nome de imagem não enviado!"));
            }

            try
            {
                JobDAO jobDAO = new JobDAO(_connection, this._environment.ContentRootPath);
                bool deleted = jobDAO.deleteImage((int)id, post, image);

                if (deleted == true)
                {
                    return Ok(new SuccessMessageModel("Imagem apagada!"));
                }
                else
                {
                    return BadRequest(new ErrorMessageModel("Imagem não apagada ou inexistente!"));
                }

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

    }
}
