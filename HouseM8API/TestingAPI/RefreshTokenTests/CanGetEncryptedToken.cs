using Enums;
using HouseM8API;
using HouseM8API.Data_Access;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using Xunit;

namespace HouseM8APITests.WorkTests
{
    [Collection("Sequential")]
    public class CanGetEncryptedToken
    {
        private Connection _connection;
        private RefreshTokenTestsFixture _fixture;

        public CanGetEncryptedToken()
        {
            _fixture = new RefreshTokenTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanGetEncryptedTokenTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmlgas23@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueiró";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returned = MateDAO.Create(testMate);

            string refreshToken = RefreshTokenHelper.generateRefreshToken();
            RefreshTokenDAO refreshTokenDAO = new RefreshTokenDAO(_connection);
            refreshTokenDAO.saveEncryptedRefreshToken(refreshToken, returned.Email);

            EncryptedRefreshTokenModel returnedToken = refreshTokenDAO.GetEncryptedRefreshTokenModel(returned.Email);

            Assert.Equal(returned.Email, returnedToken.Email);
            Assert.True(PasswordOperations.VerifyHash(refreshToken ,returnedToken.Hash, returnedToken.Salt));

            _fixture.Dispose();
        }
    }
}