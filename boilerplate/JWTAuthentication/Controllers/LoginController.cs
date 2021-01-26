using Microsoft.AspNetCore.Mvc;
using JWTAuthentication.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using JWTAuthentication.Services;
using JWTAuthentication.Helpers;

namespace JWTAuthentication.Controllers
{
    [Route("v1/account")]
    public class LoginController : Controller
    {

        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Path to user login.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
	    ///         "UserName" : "DevMig",
	    ///         "Password" : "1234567890",
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] User model)
        {

            var user = _userService.GetByUserName(model.UserName);

            if (user == null)
            {
                return Unauthorized(new { message = "Nome de Utilizador Inválido!" });
            }

            var submitedPassword = Helper.ConvertPasswordToHash(model.Password);

            if (!(user.Password == submitedPassword))
            {
                return Unauthorized(new { message = "Palavra Passe Incorreta!" });
            }

            var token = TokenService.GenerateToken(user);
            UserDTO dto = UserToDTO(user);

            return new { user = dto, token = token };
        }

        private static UserDTO UserToDTO(User user) =>
        new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Role = user.Role
        };


        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        public string Admin() => "Administrador";

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "Employer")]
        public string Employer() => "Cliente";

        [HttpGet]
        [Route("m8")]
        [Authorize(Roles = "M8")]
        public string M8() => "M8";
    }

}