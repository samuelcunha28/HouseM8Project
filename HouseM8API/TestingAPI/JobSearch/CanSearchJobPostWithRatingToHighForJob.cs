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
    public class CanSearchJobPostWithRatingToHighForJob
    {
        private Connection _connection;
        private JobSearchFixture _fixture;

        public CanSearchJobPostWithRatingToHighForJob()
        {
            _fixture = new JobSearchFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanSearchJobPostWithRatingToHighForJobTest()
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

            Assert.Empty(jobPostsArray);

            _fixture.Dispose();
        }
    }
}
