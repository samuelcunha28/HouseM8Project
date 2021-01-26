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
    public class CanEmployerListFavoriteMates
    {
        private Connection _connection;
        private ProfileTestsFixture _fixture;

        public CanEmployerListFavoriteMates()
        {
            _fixture = new ProfileTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanEmployerListFavoriteMatesTest()
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

            IEmployerDAO<Employer> employerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Ema";
            testEmployer.LastName = "Coelho";
            testEmployer.UserName = "EmiRegi";
            testEmployer.Password = "123";
            testEmployer.Email = "ema@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lousada";

            Employer returnedEmployer = employerDAO.Create(testEmployer);

            employerDAO.AddFavorite(returnedEmployer.Id, returnedMate.Id);

            List<FavoriteModel> favoriteModels = employerDAO.FavoritesList(returnedEmployer.Id).ToList();

            Assert.Equal(returnedMate.Email, favoriteModels.Find(a => a.Email == returnedMate.Email).Email);

            _fixture.Dispose();
        }
    }
}
