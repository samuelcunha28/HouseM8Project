using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System.Linq;
using Xunit;

namespace HouseM8APITests.ProfileTests
{
    [Collection("Sequential")]
    public class CanMateRemoveCategories
    {
        private Connection _connection;
        private ProfileTestsFixture _fixture;

        public CanMateRemoveCategories()
        {
            _fixture = new ProfileTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanMateRemoveCategoryTest()
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

            CategoryModel category = new CategoryModel();
            category.categories = Categories.TRANSPORTATION;

            MateDAO.RemoveCategory(returned.Id, category);

            Assert.False(MateDAO.CategoriesList(returned.Id).ToList().Exists(a => a.categories == Categories.TRANSPORTATION));

            _fixture.Dispose();
        }
    }
}
