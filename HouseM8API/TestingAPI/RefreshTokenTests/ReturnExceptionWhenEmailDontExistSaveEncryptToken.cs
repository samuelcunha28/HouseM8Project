using System;
using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.WorkTests
{
    [Collection("Sequential")]
    public class ReturnExceptionWhenEmailDontExistSaveEncryptToken
    {
        private Connection _connection;
        private RefreshTokenTestsFixture _fixture;

        public ReturnExceptionWhenEmailDontExistSaveEncryptToken()
        {
            _fixture = new RefreshTokenTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void ReturnExceptionWhenEmailDontExistSaveEncryptTokenTest()
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

            Assert.Throws<Exception>(() => refreshTokenDAO.saveEncryptedRefreshToken(refreshToken, "1@gmail.com"));

            _fixture.Dispose();
        }
    }
}