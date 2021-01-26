using Enums;
using HouseM8API;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.LoginTests
{
    [Collection("Sequential")]
    public class CanMateLoginWithWrongPassword
    {
        private Connection _connection;
        private LoginFixture _fixture;

        public CanMateLoginWithWrongPassword()
        {
            _fixture = new LoginFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanRegisterMateWrongPasswordTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();
            string password = testMate.Password;

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "Devlffg@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueir";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returned = MateDAO.Create(testMate);

            Assert.False(PasswordOperations.VerifyHash("12345", returned.Password, returned.PasswordSalt));

            _fixture.Dispose();
        }
    }
}
