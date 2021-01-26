using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.RegisterTests
{
    [Collection("Sequential")]
    public class CanRegisterEmployerWithoutDescription
    {
        private Connection _connection;
        private RegisterTestsFixture _fixture;

        public CanRegisterEmployerWithoutDescription()
        {
            _fixture = new RegisterTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanRegisterEmployerWithoutDescriptionTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "carvalho@gmail.com";
            testEmployer.Address = "Lixa";

            Employer returned = EmployerDAO.Create(testEmployer);

            Assert.Equal(testEmployer.FirstName, returned.FirstName);
            Assert.Equal(testEmployer.LastName, returned.LastName);
            Assert.Equal(testEmployer.UserName, returned.UserName);
            Assert.Equal(testEmployer.Password, returned.Password);
            Assert.Equal(testEmployer.Email, returned.Email);
            Assert.Equal(testEmployer.Address, returned.Address);

            _fixture.Dispose();
        }
    }
}
