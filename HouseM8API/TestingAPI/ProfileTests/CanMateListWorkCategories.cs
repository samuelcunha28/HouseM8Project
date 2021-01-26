using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HouseM8APITests.ProfileTests
{
    [Collection("Sequential")]
    public class CanMateListWorkCategories
    {
        private Connection _connection;
        private ProfileTestsFixture _fixture;

        public CanMateListWorkCategories()
        {
            _fixture = new ProfileTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanMateListWorkCategoriesTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "m@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueiró";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returned = MateDAO.Create(testMate);

            List<CategoryModel> category = MateDAO.CategoriesList(returned.Id).ToList();

            Assert.Equal(Categories.PLUMBING, category.ElementAt(0).categories);
            Assert.Equal(Categories.CLEANING, category.ElementAt(1).categories);

            _fixture.Dispose();
        }
    }
}
