using Enums;
using HouseM8API;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HouseM8APITests.PasswordTests
{
    [Collection("Sequential")]
    public class CanUserRecoverPassword
    {
        private Connection _connection;
        private PasswordFixture _fixture;

        public CanUserRecoverPassword()
        {
            _fixture = new PasswordFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanUserRecoverPasswordTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Samuel";
            testMate.LastName = "Cunha";
            testMate.UserName = "samuelcunha28";
            testMate.Password = "samuelcunha";
            testMate.Email = "samuelcunha1998@gmail.com";
            testMate.Description = "Quero recuperar a pass";
            testMate.Address = "Aparecida";
            testMate.Categories = new[] { Categories.GARDENING };
            testMate.Rank = Ranks.MATE;
            testMate.Range = 10;

            Mate returned = MateDAO.Create(testMate);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("RQj!O9+Sq|D8XjYa|}kgnk|}ZaQUso)EMF48Fx1~0n~^~%]n|O{NqH(&5RqXbx7");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, testMate.Email.ToString())}),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            String auxResetToken = tokenString;

            PasswordOperations.NewPasswordRequest("samuelcunha1998@gmail.com", auxResetToken);

            LoginDAO loginDAO = new LoginDAO(_connection);
            RecoverPasswordModel recoverPassword = new RecoverPasswordModel();
            recoverPassword.Email = "samuelcunha1998@gmail.com";
            recoverPassword.Password = "samuelcunha123";
            recoverPassword.ConfirmPassword = "samuelcunha123";
            recoverPassword.Token = tokenString;

            Assert.True(loginDAO.RecoverPassword(recoverPassword, returned.Email));

            _fixture.Dispose();
        }
    }
}
