using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System.Collections.Generic;
using TestingAPI;
using Xunit;

namespace HouseM8APITests.JobSearch
{
    [Collection("Sequential")]
    public class CanSearchJobPostWithRatingOneCategory
    {
        private Connection _connection;
        private JobSearchFixture _fixture;

        public CanSearchJobPostWithRatingOneCategory()
        {
            _fixture = new JobSearchFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanSearchJobPostWithRatingTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "marcfdasfsdafads@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";
            testEmployer.AverageRating = 5;

            Employer returned = EmployerDAO.Create(testEmployer);

            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmlgas@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueiró";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returnedMate = MateDAO.Create(testMate);

            Review review = new Review();
            review.Rating = 4.5d;

            ReviewEmployerDAO reviewDAO = new ReviewEmployerDAO(_connection);
            reviewDAO.ReviewEmployer(returned.Id, review);

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost();

            testPost.Title = "Canalização Estourada";
            testPost.Category = Categories.PLUMBING;
            testPost.Description = "Grande estouro nos canos da sanita";
            testPost.Tradable = true;
            testPost.InitialPrice = 60.6;
            testPost.Address = "Rua sem fim";
            testPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            jobPostDAO.Create(returned.Id, testPost);

            Categories[] categories = { Categories.PLUMBING, Categories.GARDENING };
            List<JobPostReturnedModel> jobPosts = jobPostDAO.GetJobs(categories, null, null, 4, 1);
            JobPostReturnedModel[] jobPostsArray = jobPosts.ToArray();

            Assert.Equal("Canalização Estourada", jobPostsArray[0].Title);
            Assert.Equal(Categories.PLUMBING, jobPostsArray[0].Category);
            Assert.Equal("Grande estouro nos canos da sanita", jobPostsArray[0].Description);
            Assert.True(jobPostsArray[0].Tradable);
            Assert.Equal(60.6, jobPostsArray[0].InitialPrice);
            Assert.Equal("Rua sem fim", jobPostsArray[0].Address);
            Assert.Single(jobPostsArray);

            _fixture.Dispose();
        }
    }
}
