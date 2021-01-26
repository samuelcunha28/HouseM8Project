using HouseM8API;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.LoginTests
{
    [Collection("Sequential")]
    public class CanEmployerLoginWithWrongPassword
    {
        private Connection _connection;
        private LoginFixture _fixture;

        public CanEmployerLoginWithWrongPassword()
        {
            _fixture = new LoginFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanRegisterEmployerWrongPasswordTest()
        {

            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();
            string password = testEmployer.Password;

            testEmployer.FirstName = "Samuel";
            testEmployer.LastName = "Cunha";
            testEmployer.UserName = "samuelcunha28";
            testEmployer.Password = "samuel123";
            testEmployer.Email = "samuel123@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Aparecida";

            Employer returned = EmployerDAO.Create(testEmployer);

            Assert.False(PasswordOperations.VerifyHash("samuel", returned.Password, returned.PasswordSalt));
            _fixture.Dispose();
        }
    }
}
