using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.LoginTests
{
    [Collection("Sequential")]
    public class FindByEmailNullTest
    {
        private Connection _connection;
        private LoginFixture _fixture;

        public FindByEmailNullTest()
        {
            _fixture = new LoginFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanFindByEmailNullTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();
            string password = testEmployer.Password;

            // Se tirar o user falham mais testes sem ser este
            testEmployer.FirstName = "Samuel";
            testEmployer.LastName = "Cunha";
            testEmployer.UserName = "samuelcunha28";
            testEmployer.Password = "samuel123";
            testEmployer.Email = "jorge@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Aparecida";

            Employer returned = EmployerDAO.Create(testEmployer);

            UserDAO userDAO = new UserDAO(_connection);

            Assert.Null(userDAO.FindUserByEmail("xxx@gmail.com"));

            _fixture.Dispose();
        }
    }
}
