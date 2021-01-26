using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.RegisterTests
{
    [Collection("Sequential")]
    public class CanRegisterEmployer
    {
        private Connection _connection;
        private RegisterTestsFixture _fixture;

        public CanRegisterEmployer()
        {
            _fixture = new RegisterTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanRegisterEmployerTest()
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

            Assert.Equal(testEmployer.FirstName, returned.FirstName);
            Assert.Equal(testEmployer.LastName, returned.LastName);
            Assert.Equal(testEmployer.UserName, returned.UserName);
            Assert.Equal(testEmployer.Password, returned.Password);
            Assert.Equal(testEmployer.Email, returned.Email);
            Assert.Equal(testEmployer.Description, returned.Description);
            Assert.Equal(testEmployer.Address, returned.Address);

            _fixture.Dispose();
        }
    }
}
