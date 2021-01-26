using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.RegisterTests
{
    [Collection("Sequential")]
    public class CanFindUserById
    {
        private Connection _connection;
        private RegisterTestsFixture _fixture;

        public CanFindUserById()
        {
            _fixture = new RegisterTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanFindUserByIdTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "marcelo@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returned = EmployerDAO.Create(testEmployer);

            IUserDAO<User> userDAO = new UserDAO(_connection);

            User user = userDAO.FindById(returned.Id);

            Assert.NotNull(user);
            Assert.Equal(returned.Id, user.Id);

            _fixture.Dispose();
        }
    }
}
