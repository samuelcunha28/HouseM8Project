using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.RegisterTests
{
    [Collection("Sequential")]
    public class CanRegisterMateWithoutDescription
    {
        private Connection _connection;
        private RegisterTestsFixture _fixture;

        public CanRegisterMateWithoutDescription()
        {
            _fixture = new RegisterTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanRegisterMateWithoutDescriptionTest()
        {

            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "mlgxd@gmail.com";
            testMate.Address = "Figueiró";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returned = MateDAO.Create(testMate);

            Assert.Equal(testMate.FirstName, returned.FirstName);
            Assert.Equal(testMate.LastName, returned.LastName);
            Assert.Equal(testMate.UserName, returned.UserName);
            Assert.Equal(testMate.Password, returned.Password);
            Assert.Equal(testMate.Email, returned.Email);
            Assert.Equal(testMate.Address, returned.Address);
            Assert.Equal(testMate.Categories, returned.Categories);
            Assert.Equal(testMate.Rank, returned.Rank);
            Assert.Equal(testMate.Range, returned.Range);

            _fixture.Dispose();
        }

    }
}
