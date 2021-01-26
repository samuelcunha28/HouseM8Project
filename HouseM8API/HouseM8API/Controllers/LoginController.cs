using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using HouseM8API.Entities;
using HouseM8API.Models.ReturnedMessages;
using HouseM8API.Helpers;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Controller para a tarefa relacionada com o modulo de login
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly string _secret;
        private readonly IConnection _connection;
        public static string auxResetToken { get; set; }
        public static string auxEmail { get; set; }

        /// <summary>
        /// Construtor do Controller
        /// </summary>
        /// <param name="config"></param>
        public LoginController(IOptions<AppSettings> config)
        {
            _connection = new Connection(config);
            _secret = config.Value.Secret;
            _connection.Fetch();
        }

        /// <summary>
        /// Rota para o utilizador realizar o login com o uso de Json Web Tokens
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>O token string</returns>
        /// <response code="200">Retorna mensagem de Sucesso com o Token</response>
        /// <response code="400">Bad Request</response>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            try {
                LoginDAO loginDAO = new LoginDAO(_connection);
                var user = loginDAO.Authenticate(loginModel.Email, loginModel.Password);

                if (user == null)
                {
                    return BadRequest(new ErrorMessageModel("Username ou password incorreto(s)"));
                }

                JwtTokenHelper tokenHelper = new JwtTokenHelper(_connection);
                ResponseTokens response = tokenHelper.Authenticate(_secret, user);

                return Ok(response);

            } catch (Exception e){
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Rota para realizar o pedido de uma nova password
        /// </summary>
        /// <param name="forgotPasswordModel"></param>
        /// <returns>Mensagem de Sucesso ou de Erro</returns>
        /// <response code="200">Retorna mensagem de Sucesso</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            UserDAO userDAO = new UserDAO(_connection);
            User user = userDAO.FindUserByEmail(forgotPasswordModel.Email);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Email, user.Email.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(10),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    auxResetToken = tokenString;

                    PasswordOperations.NewPasswordRequest(forgotPasswordModel.Email, tokenString);
                    auxEmail = forgotPasswordModel.Email;

                    return Ok(new SuccessMessageModel("Email enviado com sucesso!"));
                }
            }
            return BadRequest(new ErrorMessageModel("Email não encontrado!"));
        }

        /// <summary>
        /// Rota para mudar a password depois de efetuar o pedido de esquecimento
        /// </summary>
        /// <param name="recoverPasswordModel"></param>
        /// <returns>Mensagem de Sucesso ou de Erro</returns>
        /// <response code="200">Retorna mensagem de Sucesso</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(RecoverPasswordModel recoverPasswordModel)
        {

            if (ModelState.IsValid)
            {
                UserDAO userDAO = new UserDAO(_connection);
                User user = userDAO.FindUserByEmail(recoverPasswordModel.Email);

                if (user != null && recoverPasswordModel.Email == auxEmail && recoverPasswordModel.Token == auxResetToken)
                {
                    LoginDAO loginDAO = new LoginDAO(_connection);
                    loginDAO.RecoverPassword(recoverPasswordModel, recoverPasswordModel.Email);

                    return Ok(new SuccessMessageModel("Password alterada com sucesso! Pode fazer login com a password nova"));

                }
                else
                {
                    return BadRequest(new ErrorMessageModel("O email que introduziu não é o seu ou o token é inválido! Erro!"));
                }
            }

            return BadRequest(new ErrorMessageModel("Dados não correspondem ao formulário!"));
        }

        /// <summary>
        /// Rota para dar refresh ao token
        /// </summary>
        /// <param name="refreshCred">Objeto com o token antigo 
        /// e com o token de refresh</param>
        /// <returns>Retorna o token novo e o token de refresh</returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh(ResponseTokens refreshCred)
        {
            JwtTokenHelper tokenHelper = new JwtTokenHelper(_connection);
            ResponseTokens token = tokenHelper.Refresh(refreshCred, _secret);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}