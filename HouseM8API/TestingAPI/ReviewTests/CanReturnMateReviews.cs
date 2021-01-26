using System.Collections.Generic;
using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using Xunit;

namespace HouseM8APITests.EmployerReviewTests
{
    [Collection("Sequential")]
    public class CanReturnMateReviews
    {
        private Connection _connection;
        private ReviewTestsFixture _fixture;

        public CanReturnMateReviews()
        {
            _fixture = new ReviewTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanReturnMateReviewsTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "marc23278@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returnedEmp = EmployerDAO.Create(testEmployer);

            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmadsdaslg23@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueir";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returned = MateDAO.Create(testMate);

            MateReview reviewsTest = new MateReview();
            reviewsTest.Rating = 4;
            reviewsTest.Comment = "O trolha fez um trabalho excelente!";

            MateReview reviewsTest2 = new MateReview();
            reviewsTest2.Rating = 2;
            reviewsTest2.Comment = "O trolha fez um trabalho mau!";

            IReviewMateDAO ReviewMateDAO = new ReviewMateDAO(_connection);
            MateReview returnedReview1 = ReviewMateDAO.ReviewMate(returnedEmp.Id, returned.Id, reviewsTest);
            MateReview returnedReview2 = ReviewMateDAO.ReviewMate(returnedEmp.Id, returned.Id, reviewsTest2);

            List<MateReviewsModel> ReviewsList = ReviewMateDAO.MateReviewsList(returned.Id);
            MateReviewsModel[] array = ReviewsList.ToArray();

            Assert.Equal(2,array.Length);

            Assert.Equal(reviewsTest.Rating, array[0].Rating);
            Assert.Equal(reviewsTest.Comment, array[0].Comment);
            Assert.Equal(returnedEmp.UserName, array[0].Author);

            Assert.Equal(reviewsTest2.Rating, array[1].Rating);
            Assert.Equal(reviewsTest2.Comment, array[1].Comment);
            Assert.Equal(returnedEmp.UserName, array[1].Author);

            _fixture.Dispose();
        }
    }
}