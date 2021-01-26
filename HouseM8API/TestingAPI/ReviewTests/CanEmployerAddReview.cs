using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8APITests.EmployerReviewTests;
using Models;
using Xunit;

namespace HouseM8APITests.ReviewTests
{
    [Collection("Sequential")]
    public class CanEmployerAddReview
    {
        private Connection _connection;
        private ReviewTestsFixture _fixture;

        public CanEmployerAddReview()
        {
            _fixture = new ReviewTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanEmployerAddReviewTest()
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

            Employer returnedEmp = EmployerDAO.Create(testEmployer);

            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmadsdaslg@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueir";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returned = MateDAO.Create(testMate);

            IReviewMateDAO ReviewMateDAO = new ReviewMateDAO(_connection);
            MateReview reviewsTest = new MateReview();
            reviewsTest.Id = 5;
            reviewsTest.Rating = 4;
            reviewsTest.Comment = "O trolha fez um trabalho excelente!";

            MateReview revReturned = ReviewMateDAO.ReviewMate(returnedEmp.Id, returned.Id, reviewsTest);

            Assert.Equal(reviewsTest.Id, revReturned.Id);
            Assert.Equal(reviewsTest.Rating, revReturned.Rating);
            Assert.Equal(reviewsTest.Comment, revReturned.Comment);

            _fixture.Dispose();
        }
    }
}
