using AutoMapper;
using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System.Security.Claims;
using System;
using HouseM8API.Models.ReturnedMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using HouseM8API.Entities;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controlador com as rotas relativas
    /// ao perfil do User
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConnection _connection;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _environment;

        /// <summary>
        /// Método construtor da classe UsersController
        /// </summary>
        /// <param name="config">Config com a conexão à BD</param>
        /// <param name="mapper">Mapper para mapear entidades</param>
        /// <param name="environment">Fornece informações sobre o ambiente de 
        /// hosting na web em que o aplicativo está a ser executado
        /// </param>
        public UsersController(IOptions<AppSettings> config, IMapper mapper, IWebHostEnvironment environment)
        {
            _environment = environment;
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Registar um Mate na plataforma
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///      "FirstName":"Jorge",
        ///      "LastName":"Moreira",
        ///      "UserName": "Jorge_moreira",
        ///      "Password": "123",
        ///      "Email":"jorge@gmail.com",
        ///      "Description":"Faço trabalho de qualidade",
        ///      "Address": {
        ///          "street": "Rua das Veigas",
        ///          "streetNumber": 377,
        ///          "postalCode": "4620-471",
        ///          "district": "Porto",
        ///          "country": "Portugal"
        ///      },
        ///      "Categories":[
        ///          "PLUMBING", 
        ///          "GARDENING"
        ///      ],
        ///      "Range": 20
        ///     }
        ///     
        /// </remarks>
        /// <param name="regMate">Mate a ser registado na plataforma</param>
        /// <returns>Mate registado</returns>
        /// <response code="200">Mate registado</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("mate")]
        public ActionResult<MateModel> Create(MateRegister regMate)
        {
            try
            {
                Mate mate = _mapper.Map<Mate>(regMate);
                IMateDAO<Mate> MateDAO = new MateDAO(_connection);
                MateModel mateModel = _mapper.Map<MateModel>(MateDAO.Create(mate));
                return Ok(mateModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
        }

        /// <summary>
        /// Regista um Employer na plataforma
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     { 
        ///      "UserName": "TheBloodyMonday",
        ///      "Password": "abc",
        ///      "Email": "jessica@gmail.com",
        ///      "FirstName": "Jessica",
        ///      "LastName": "Coelho",
        ///      "RoleId": 2,
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
        /// <param name="regEmployer">Dados do Employer</param>
        /// <returns>Employer</returns>
        /// <response code="200">Employer registado</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("employer")]
        public ActionResult<EmployerModel> Create(EmployerRegister regEmployer)
        {
            try
            {
                Employer employer = _mapper.Map<Employer>(regEmployer);
                IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
                EmployerModel employerModel = _mapper.Map<EmployerModel>(EmployerDAO.Create(employer));
                return Ok(employerModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
        }

        /// <summary>
        /// Atualiza a password de um utilizador
        /// </summary>
        /// <remarks>
        ///     
        ///     { 
        ///      "Password": "Nova_Password" 
        ///      "OldPassword": "Password_Antiga"
        ///     }
        ///     
        /// </remarks>
        /// <param name="passwordUpdateModel">Nova password</param>
        /// <returns>
        /// True caso a password seja atualizada com sucesso
        /// False caso contrário
        /// </returns>
        /// <response code="200">True (Password alterada)</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut("password")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public IActionResult UpdatePassword(PasswordUpdateModel passwordUpdateModel)
        {
            try
            {
                int? id = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (id == null)
                {
                    return Unauthorized(new ErrorMessageModel("Sem Autorização ou sem sessão inciada"));
                }

                UserDAO userDAO = new UserDAO(_connection);
                bool newPass = userDAO.UpdatePassword(passwordUpdateModel, id);
                return Ok(new SuccessMessageModel("Password alterada com sucesso!"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
        }

        /// <summary>
        /// Rota para fazer Upload de uma imagem 
        /// de perfil de user
        /// </summary>
        /// <param name="profilePic">Imagem</param>
        /// <returns>
        /// Imagem de perfil definida!
        /// </returns>
        /// <response code="200">Imagem de perfil definida!</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("profilePic")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public IActionResult UploadProfilePic([FromForm] IFormFile profilePic)
        {

            try
            {
                int? id = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

                if (id == null)
                {
                    return Unauthorized(new ErrorMessageModel("Sem Autorização ou sem sessão inciada"));
                }

                UserDAO userDAO = new UserDAO(_connection, this._environment.ContentRootPath);
                SuccessMessageModel message = userDAO.UploadImagesToUser((int)id, profilePic);

                return Ok(message);

            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }

        }

        /// <summary>
        /// Rota para obter a imagem de perfil de um user
        /// </summary>
        /// <param name="user">Id do user</param>
        /// <returns>Nome da imagem de destaque</returns>
        /// <response code="200">Nome da imagem de destaque</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("profilePic/{user}")]
        [AllowAnonymous]
        public IActionResult getProfilePicture(int user)
        {
            try
            {
                UserDAO userDAO = new UserDAO(_connection, this._environment.ContentRootPath);
                ImageName image = userDAO.getProfileImage(user);
                return Ok(image);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Rota para apagar a imagem de perfil do utilizador
        /// </summary>
        /// <returns>Imagem Apagada!</returns>
        /// <response code="200">Imagem Apagada!</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        [HttpDelete("DeleteProfilePic")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public IActionResult deleteProfilePicture()
        {

            int? id = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (id == null)
            {
                return Unauthorized(new ErrorMessageModel("Sem Autorização ou sem sessão inciada"));
            }

            try
            {

                UserDAO userDAO = new UserDAO(_connection, this._environment.ContentRootPath);
                bool deleted = userDAO.deleteImage((int)id);

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
        /// Rota para obter um utilizador por Id
        /// </summary>
        /// <param name="userId">Id do user</param>
        /// <returns>Retorna o User</returns>
        /// <response code="200">User</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad Request</response>
        [HttpGet("{userId}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public IActionResult GetUserById(int userId)
        {

            int? id = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (id == null)
            {
                return Unauthorized(new ErrorMessageModel("Sem Autorização ou sem sessão inciada"));
            }

            try
            {

                UserDAO userDAO = new UserDAO(_connection, this._environment.ContentRootPath);
                User user = userDAO.FindById(userId);

                if (user == null)
                {
                    return BadRequest(new ErrorMessageModel("Utilizador não encontrado!"));
                }
                
                return Ok(user);

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }
    }
}
