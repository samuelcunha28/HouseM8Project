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
using System.Collections.Generic;
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controller para as tarefas relacionadas com o modulo de review a um employer e a um mate
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IConnection _connection;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do Controller
        /// </summary>
        /// <param name="config"></param>
        /// <param name="mapper"></param>
        public ReviewsController(IOptions<AppSettings> config, IMapper mapper)
        {
            _connection = new Connection(config);
            _mapper = mapper;
            _connection.Fetch();
        }

        /// <summary>
        /// Rota para a criação de uma review a um employer.
        /// Quem pode fazer esta operação será somente um user designado como Mate.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         "rating" : 5.00
        ///     }
        /// </remarks>
        /// <param name="employer"></param>
        /// <param name="revModel"></param>
        /// <returns>O review model</returns>
        /// <response code="200">Retorna ReviewsModel</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("employer/{employer}")]
        [Authorize(Roles = "M8")]
        public ActionResult<ReviewsModel> ReviewEmployer(int employer, ReviewsModel revModel)
        {
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            // caso o mate id seja nulo
            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            Review review = _mapper.Map<Review>(revModel);
            IReviewEmployerDAO ReviewEmployerDAO = new ReviewEmployerDAO(_connection);
            ReviewsModel reviewModel = _mapper.Map<ReviewsModel>(ReviewEmployerDAO.ReviewEmployer(employer, review));
            return Ok(reviewModel);
        }

        /// <summary>
        /// Rota para a criação de uma review a um mate.
        /// Quem pode fazer esta operação será somente um user designado como Employer.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     
        ///     {
        ///         "rating" : 5.00,
        ///         "comment" : "Very Good Job!"
        ///     }
        /// </remarks>
        /// <param name="mate"></param>
        /// <param name="revModel"></param>
        /// <returns>O MateReviewsModel</returns>
        /// <response code="200">Retorna MateReviewsModel</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("mate/{mate}")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<MateReviewsModel> ReviewMate(int mate, MateReviewsModel revModel)
        {
            int? employerID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            // caso o mate id seja nulo
            if (employerID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            MateReview review = _mapper.Map<MateReview>(revModel);
            IReviewMateDAO ReviewMateDAO = new ReviewMateDAO(_connection);
            MateReviewsModel reviewModel = _mapper.Map<MateReviewsModel>(ReviewMateDAO.ReviewMate((int)employerID, mate, review));
            return Ok(reviewModel);
        }

        /// <summary>
        /// Rota para obter as reviews Feitas a um mate
        /// </summary>
        /// <param name="id">Id do mate</param>
        /// <returns>Lista com as reviews do mate</returns>
        /// <response code="200">Lista com as reviews do mate</response>
        /// <response code="404">NotFound</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("mateReviews/{id}")]
        [Authorize(Roles = "EMPLOYER")]
        public ActionResult<List<MateReviewsModel>> MateReviewsList(int id){
            
            int? EmployerID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            // caso o employerId seja nulo
            if (EmployerID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            MateDAO mateDao = new MateDAO(_connection);

            if(mateDao.FindMateById(id)==null) {
                return NotFound(new ErrorMessageModel("Mate não Encontrado"));
            }

            ReviewMateDAO reviewDao = new ReviewMateDAO(_connection);
            List<MateReviewsModel> reviewsList = reviewDao.MateReviewsList(id);
            
            return Ok(reviewsList);
        }

        /// <summary>
        /// Rota para um mate obter as próprias reviews
        /// </summary>
        /// <response code="200">Lista com as reviews do mate</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("mateReviews")]
        [Authorize(Roles = "M8")]
        public ActionResult<List<MateReviewsModel>> MateReviewsList(){
            
            int? mateID = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            // caso o mate id seja nulo
            if (mateID == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não existe!"));
            }

            ReviewMateDAO reviewDao = new ReviewMateDAO(_connection);
            List<MateReviewsModel> reviewsList = reviewDao.MateReviewsList((int)mateID);

            return Ok(reviewsList);
        }

    }
}
