using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using Xunit;

namespace HouseM8APITests.PasswordTests
{
    [Collection("Sequential")]
    public class CanUserUpdatePassword
    {
        private Connection _connection;
        private PasswordFixture _fixture;

        public CanUserUpdatePassword()
        {
            _fixture = new PasswordFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanUserUpdatePasswordTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Jessica";
            testMate.LastName = "Coelho";
            testMate.UserName = "Jessijow";
            testMate.Password = "123";
            testMate.Email = "j@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Ordem";
            testMate.Categories = new[] { Categories.FURNITURE_ASSEMBLE, Categories.TRANSPORTATION };
            testMate.Rank = Ranks.MATE;
            testMate.Range = 20;

            Mate returned = MateDAO.Create(testMate);

            UserDAO userDAO = new UserDAO(_connection);
            PasswordUpdateModel newPass = new PasswordUpdateModel();
            newPass.Password = "abc";
            newPass.OldPassword = "123";

            Assert.True(userDAO.UpdatePassword(newPass, returned.Id));

            _fixture.Dispose();
        }
    }
}
