using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.ProfileTests
{
    [Collection("Sequential")]
    public class CanMateUpdateProfile
    {
        private Connection _connection;
        private ProfileTestsFixture _fixture;

        public CanMateUpdateProfile()
        {
            _fixture = new ProfileTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanMateUpdateProfileTest()
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

            returned.FirstName = "Ema";
            returned.LastName = "Coelho";
            returned.UserName = "EmiRegi";
            returned.Description = "Sou um mate!";
            returned.Address = "Lousada";
            returned.Range = 40;

            Mate updated = MateDAO.Update(returned, returned.Id);

            Assert.Equal(returned.FirstName, updated.FirstName);
            Assert.Equal(returned.LastName, updated.LastName);
            Assert.Equal(returned.UserName, updated.UserName);
            Assert.Equal(returned.Description, updated.Description);
            Assert.Equal(returned.Address, updated.Address);
            Assert.Equal(returned.Range, updated.Range);

            _fixture.Dispose();
        }
    }
}
