using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.ProfileTests
{
    [Collection("Sequential")]
    public class CanEmployerUpdateProfile
    {
        private Connection _connection;
        private ProfileTestsFixture _fixture;

        public CanEmployerUpdateProfile()
        {
            _fixture = new ProfileTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanEmployerUpdateProfileTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Jessica";
            testEmployer.LastName = "Coelho";
            testEmployer.UserName = "Jessijow";
            testEmployer.Password = "123";
            testEmployer.Email = "jess@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Ordem";

            Employer returned = EmployerDAO.Create(testEmployer);

            returned.FirstName = "Ema";
            returned.LastName = "Coelho";
            returned.UserName = "EmiRegi";
            returned.Description = "Sou um empregador!";
            returned.Address = "Lousada";

            Employer updated = EmployerDAO.Update(returned, returned.Id);

            Assert.Equal(returned.FirstName, updated.FirstName);
            Assert.Equal(returned.LastName, updated.LastName);
            Assert.Equal(returned.UserName, updated.UserName);
            Assert.Equal(returned.Description, updated.Description);
            Assert.Equal(returned.Address, updated.Address);

            _fixture.Dispose();
        }
    }
}
