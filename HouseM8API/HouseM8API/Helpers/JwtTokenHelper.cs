using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Microsoft.IdentityModel.Tokens;

namespace HouseM8API.Helpers
{
    public class JwtTokenHelper
    {
        private readonly IConnection _connection;

        /// <summary>
        /// Constructor da classe JwtTokenHelper
        /// </summary>
        /// <param name="connection">Objeto com a conexão à BD</param>
        public JwtTokenHelper(IConnection connection){
            _connection = connection;
        }

        /// <summary>
        /// Método para fazer a autenticação de um utilizador com
        /// JWT e Refresh Token
        /// </summary>
        /// <param name="secret">Secret para criação do token de acesso</param>
        /// <param name="user">Utilizador que faz a autenticação</param>
        /// <returns>Retorna o token de acesso e de refresh</returns>
        public ResponseTokens Authenticate(string secret, User user){

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString())}),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            string tokenRefresh = RefreshTokenHelper.generateRefreshToken();

            RefreshTokenDAO refreshTokenDAO = new RefreshTokenDAO(_connection);
            try {
                if (refreshTokenDAO.GetEncryptedRefreshTokenModel(user.Email) == null){
                    refreshTokenDAO.saveEncryptedRefreshToken(tokenRefresh, user.Email);
                } else {
                    refreshTokenDAO.updateEncryptedRefreshToken(tokenRefresh, user.Email);
                }
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
            
            return (new ResponseTokens {Token = tokenString, RefreshToken = tokenRefresh } );
        }

        /// <summary>
        /// Overload do método Autenticate , que cria um novo token
        /// com as claims do token antigo e cria um novo refresh token
        /// </summary>
        /// <param name="email">Email do utilizador</param>
        /// <param name="secret">Secret para criar o token de acesso</param>
        /// <param name="claims">Claims do token de acesso antigo</param>
        /// <returns>Retorna um objeto ResponseTokens, com o token de acesso novo
        /// e o refresh token novo </returns>
        public ResponseTokens Authenticate(string email, string secret, Claim[] claims){

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.SerialNumber, claims[0].Value),
                    new Claim(ClaimTypes.Role, claims[1].Value),
                    new Claim(ClaimTypes.Email, claims[2].Value)}),
                    
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            
            string tokenRefresh = RefreshTokenHelper.generateRefreshToken();

            RefreshTokenDAO refreshTokenDAO = new RefreshTokenDAO(_connection);

            try{
                if (refreshTokenDAO.GetEncryptedRefreshTokenModel(email) == null){
                    refreshTokenDAO.saveEncryptedRefreshToken(tokenRefresh, email);
                } else {
                    refreshTokenDAO.updateEncryptedRefreshToken(tokenRefresh, email);
                }
            } catch (Exception e) {
                throw new Exception(e.Message);
            }

            return new ResponseTokens
            {
                Token = tokenString,
                RefreshToken = tokenRefresh
            };
        }

        /// <summary>
        /// Método que faz o refresh de um token.
        /// É utilizado o token antigo para validação e para ir buscar as claims
        /// </summary>
        /// <param name="tokens">Objeto ResponseTokens com o token antigo 
        /// e token de resfresh</param>
        /// <param name="secret">Secret para criar um token de acesso novo</param>
        /// <returns>Retorna um objeto ResponseTokens com o token novo 
        /// e token de resfresh novo</returns>
        public ResponseTokens Refresh(ResponseTokens tokens, string secret){

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            SecurityToken validatedToken;

            var principal = tokenHandler.ValidateToken(tokens.Token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false
                }, out validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;

            if(jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token Inválido!");
            }

            string email = ClaimHelper.GetEmailFromClaimIdentity((ClaimsIdentity)principal.Identity);

            RefreshTokenDAO refreshTokenDAO = new RefreshTokenDAO(_connection);
            EncryptedRefreshTokenModel encToken = refreshTokenDAO.GetEncryptedRefreshTokenModel(email);

            if(encToken == null){
                throw new Exception("O token nao existe para o email pretendido!");
            }

            if(PasswordOperations.VerifyHash(tokens.RefreshToken, encToken.Hash, encToken.Salt) == false)
            {
                throw new SecurityTokenException("Token Inválido!");
            }

            return Authenticate(email, secret, principal.Claims.ToArray());
        }
    }
}
