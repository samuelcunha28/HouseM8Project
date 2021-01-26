using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.ProfileTests
{
    [Collection("Sequential")]
    public class CanGetMateWithDescriptionById
    {
        private Connection _connection;
        private ProfileTestsFixture _fixture;

        public CanGetMateWithDescriptionById()
        {
            _fixture = new ProfileTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanGetMateWithDescriptionByIdTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Jessica";
            testMate.LastName = "Coelho";
            testMate.UserName = "Jessijow";
            testMate.Password = "123";
            testMate.Email = "jessicas@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Ordem";
            testMate.Categories = new[] { Categories.FURNITURE_ASSEMBLE, Categories.TRANSPORTATION };
            testMate.Rank = Ranks.MATE;
            testMate.Range = 20;

            Mate returnedMate = MateDAO.Create(testMate);

            Mate m = MateDAO.FindMateById(returnedMate.Id);

            Assert.Equal(returnedMate.Id, m.Id);
            Assert.Equal(returnedMate.UserName, m.UserName);
            Assert.Equal(returnedMate.FirstName, m.FirstName);
            Assert.Equal(returnedMate.LastName, m.LastName);
            Assert.Equal(returnedMate.Email, m.Email);
            Assert.Equal(returnedMate.Description, m.Description);
            Assert.Equal(returnedMate.Address, m.Address);

            _fixture.Dispose();
        }
    
}
}
