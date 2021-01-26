using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.EmployerReviewTests
{
    [Collection("Sequential")]
    public class CanMateAddRating
    {
        private Connection _connection;
        private ReviewTestsFixture _fixture;

        public CanMateAddRating()
        {
            _fixture = new ReviewTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanMateAddRatingTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "marc@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returned = EmployerDAO.Create(testEmployer);

            IReviewEmployerDAO ReviewEmployerDAO = new ReviewEmployerDAO(_connection);
            Review reviewsTest = new Review();
            reviewsTest.Id = 5;
            reviewsTest.Rating = 4;

            Review revReturned = ReviewEmployerDAO.ReviewEmployer(returned.Id, reviewsTest);

            Assert.Equal(reviewsTest.Id, revReturned.Id);
            Assert.Equal(reviewsTest.Rating, revReturned.Rating);

            _fixture.Dispose();
        }
    }
}
