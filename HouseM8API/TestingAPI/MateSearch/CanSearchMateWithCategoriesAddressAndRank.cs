using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using System.Collections.Generic;
using Xunit;

namespace HouseM8APITests.MateSearch
{
    [Collection("Sequential")]
    public class CanSearchMateWithCategoriesAddressAndRank
    {
        private Connection _connection;
        private MateSearchFixture _fixture;

        public CanSearchMateWithCategoriesAddressAndRank()
        {
            _fixture = new MateSearchFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanSearchMateWithCategoriesAddressAndRankTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);

            Mate firstMate = new Mate();

            firstMate.FirstName = "Marcelo";
            firstMate.LastName = "Carvalho";
            firstMate.UserName = "VeryGoodDev";
            firstMate.Password = "123";
            firstMate.Email = "ghjghj@gmail.com";
            firstMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            firstMate.Address = "Sra. Aparecida, Lousada";
            firstMate.Categories = new[] { Categories.CLEANING, Categories.ELECTRICITY };
            firstMate.Rank = Ranks.MATE;
            firstMate.Range = 50;

            Mate returnedFirst = MateDAO.Create(firstMate);

            Mate secondMate = new Mate();

            secondMate.FirstName = "Samuel";
            secondMate.LastName = "Cunha";
            secondMate.UserName = "VeryGoodDev";
            secondMate.Password = "123";
            secondMate.Email = "asdasd@gmail.com";
            secondMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            secondMate.Address = "Rua de Salgueiros, Penafiel";
            secondMate.Categories = new[] { Categories.GARDENING, Categories.FURNITURE_ASSEMBLE };
            secondMate.Range = 20;

            Mate returnedSecond = MateDAO.Create(secondMate);
            MateDAO.UpdateRank(Ranks.SUPER_MATE, returnedSecond.Id);

            Categories[] categories = { Categories.CLEANING, Categories.GARDENING };
            string myAddress = "Rua de Salgueiros, Penafiel";

            List<Mate> mates = MateDAO.GetMates(categories, myAddress, Ranks.SUPER_MATE, null, null);

            Mate[] matesArray = mates.ToArray();

            Assert.Equal(returnedSecond.FirstName, matesArray[0].FirstName);
            Assert.Equal(returnedSecond.LastName, matesArray[0].LastName);
            Assert.Equal(returnedSecond.UserName, matesArray[0].UserName);
            Assert.Equal(returnedSecond.Email, matesArray[0].Email);
            Assert.Equal(returnedSecond.Description, matesArray[0].Description);
            Assert.Equal(returnedSecond.Address, matesArray[0].Address);
            Assert.Equal(returnedSecond.Categories, matesArray[0].Categories);
            Assert.Equal(Ranks.SUPER_MATE, matesArray[0].Rank);
            Assert.Equal(returnedSecond.Range, matesArray[0].Range);

            Assert.Single(matesArray);

            _fixture.Dispose();
        }
    }
}
