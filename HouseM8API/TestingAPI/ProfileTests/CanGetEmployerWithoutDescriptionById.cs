using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.ProfileTests
{
    [Collection("Sequential")]
    public class CanGetEmployerWithoutDescriptionById
    {
        private Connection _connection;
        private ProfileTestsFixture _fixture;

        public CanGetEmployerWithoutDescriptionById()
        {
            _fixture = new ProfileTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanGetEmployerWithoutDescriptionByIdTest()
        {
            IEmployerDAO<Employer> employerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Ema";
            testEmployer.LastName = "Coelho";
            testEmployer.UserName = "EmiRegi";
            testEmployer.Password = "123";
            testEmployer.Email = "ema@gmail.com";
            testEmployer.Address = "Lousada";

            Employer returnedEmployer = employerDAO.Create(testEmployer);

            Employer e = employerDAO.FindEmployerById(returnedEmployer.Id);

            Assert.Equal(returnedEmployer.Id, e.Id);
            Assert.Equal(returnedEmployer.UserName, e.UserName);
            Assert.Equal(returnedEmployer.FirstName, e.FirstName);
            Assert.Equal(returnedEmployer.LastName, e.LastName);
            Assert.Equal(returnedEmployer.Email, e.Email);
            Assert.Equal(returnedEmployer.Address, e.Address);

            _fixture.Dispose();
        }
    }
}
