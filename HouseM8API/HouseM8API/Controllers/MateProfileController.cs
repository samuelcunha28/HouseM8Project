using AutoMapper;
using Enums;
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
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controlador com as rotas relativas
    /// ao perfil do Mate
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MateProfileController : ControllerBase
    {
        private readonly IConnection _connection;
        private readonly IMapper _mapper;

        /// <summary>
        /// Método construtor da classe MateProfileController
        /// </summary>
        /// <param name="config">Objeto config com a conexão à BD</param>
        /// <param name="mapper">Objeto mapper para mapear entidades</param>
        public MateProfileController(IOptions<AppSettings> config, IMapper mapper)
        {
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Rota que obtém a lista de categorias de um Mate
        /// </summary>
        /// <returns>Lista de categorias do Mate</returns>
        /// <response code="200">Lista de categorias do Mate</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        [Authorize(Roles = "M8")]
        [HttpGet("listcat")]
        public ActionResult<List<CategoryModel>> CategoriesList()
        {
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            MateDAO mateDAO = new MateDAO(_connection);
            List<CategoryModel> categories = mateDAO.CategoriesList((int)mateID).ToList();

            return categories;
        }

        /// <summary>
        /// Adiciona categorias de trabalho ao Mate
        /// </summary>
        /// <remarks>
        ///     
        ///     [
        ///       { 
        ///         "categories": "FURNITURE_ASSEMBLE" 
        ///       }
        ///     ]
        ///      
        /// </remarks>
        /// <param name="category">Categoria(s) a adicionar</param>
        /// <returns>Categoria(s) adicionada(s)</returns>
        /// <response code="200">Categoria(s) adicionada(s)</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "M8")]
        [HttpPost("addcat")]
        public ActionResult<Categories> AddCategory(CategoryModel[] category)
        {
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            MateDAO mateDAO = new MateDAO(_connection);
            mateDAO.AddCategory((int)mateID, category);

            return Ok(category);
        }

        /// <summary>
        /// Remove uma categoria de trabalho do Mate
        /// </summary>
        /// <remarks>
        ///     
        ///     { 
        ///       "categories": "FURNITURE_ASSEMBLE" 
        ///     }
        ///     
        /// </remarks>
        /// <param name="category">Categoria</param>
        /// <returns>"Deleted" em caso de sucesso</returns>
        /// <response code="200">"Deleted" em caso de sucesso</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "M8")]
        [HttpDelete("deletecat")]
        public IActionResult RemoveCategory(CategoryModel category)
        {
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            MateDAO mateDAO = new MateDAO(_connection);
            mateDAO.RemoveCategory((int)mateID, category);

            return Ok(new SuccessMessageModel("Apagado!"));
        }

        /// <summary>
        /// Atualiza os dados pessoais do Mate
        /// </summary>
        /// <remarks>
        ///     
        ///     { 
        ///         "FirstName": "Jessica", 
        ///         "LastName": "Silva",
        ///         "Description": "Sou um Mate!",
        ///         "UserName": "TheBloodyMonday",
        ///         "Address": {
        ///             "street": "Rua das Veigas",
        ///             "streetNumber": 377,
        ///             "postalCode": "4620-471",
        ///             "district": "Porto",
        ///             "country": "Portugal"
        ///             },
        ///         "Range": 20 
        ///     }
        ///     
        /// </remarks>
        /// <param name="mateUpdate">Informações do Mate atualizadas</param>
        /// <returns>"Update Successful" em caso de sucesso</returns>
        /// <response code="200">Update successful</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "M8")]
        [HttpPut("update")]
        public IActionResult Update(MateUpdate mateUpdate)
        {
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            Mate mate = _mapper.Map<Mate>(mateUpdate);
            MateDAO mateDAO = new MateDAO(_connection);
            mateDAO.Update(mate, (int)mateID);

            return Ok(new SuccessMessageModel("Atualizado com sucesso!"));
        }

        /// <summary>
        /// Rota para procurar mates com filtros
        /// </summary>
        /// <param name="categories">Categorias</param>
        /// <param name="address">Morada</param>
        /// <param name="rank">Rank</param>
        /// <param name="distance">Distancia</param>
        /// <param name="rating">Classificação</param>
        /// <returns>Lista de Mates</returns>
        /// <response code="200">Lista de Mates</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [AllowAnonymous]
        [HttpGet("filterMates")]
        public ActionResult<List<MateModelExtended>> GetMates(
            [FromQuery(Name = "categories")] Categories[] categories,
            [FromQuery(Name = "address")] string address,
            [FromQuery(Name = "rank")] Ranks? rank,
            [FromQuery(Name = "distance")] int? distance,
            [FromQuery(Name = "rating")] int? rating)
        {

            IMateDAO<Mate> mateDAO = new MateDAO(_connection);
            List<MateModelExtended> matesList = _mapper.Map<List<MateModelExtended>>(mateDAO.GetMates(categories, address, rank, distance, rating));
            return Ok(matesList);
        }

        /// <summary>
        /// Rota para obter um Mate através do seu ID
        /// </summary>
        /// <param name="id">Id do mate a ser procurado</param>
        /// <returns>Retorna um Mate</returns>
        /// <response code="200">Retorna um Mate</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<MateProfileModel> GetMateById(int id)
        {
            MateDAO mateDAO = new MateDAO(_connection);
            MateProfileModel returned = _mapper.Map<MateProfileModel>(mateDAO.FindMateById(id));

            return Ok(returned);
        }

        /// <summary>
        /// Obtém a lista de trabalhos pendentes de um Mate
        /// </summary>
        /// <remarks>
        /// 
        ///     GET /pending
        /// 
        /// </remarks>
        /// <returns>A lista de trabalhos pendentes</returns>
        /// <response code="200">Retorna a lista de trabalhos pendentes</response>
        /// <response code="404">Not Found</response>
        [Authorize(Roles = "M8")]
        [HttpGet("pending")]
        public ActionResult<List<PendingJobModel>> PendingJobsList()
        {
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            MateDAO mateDAO = new MateDAO(_connection);
            List<PendingJobModel> pendingJobs = mateDAO.PendingJobsList((int)mateID).ToList();

            return Ok(pendingJobs);
        }
    }
}
