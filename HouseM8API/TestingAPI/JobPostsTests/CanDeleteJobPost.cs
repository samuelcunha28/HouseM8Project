using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System.Collections.Generic;
using Xunit;

namespace HouseM8APITests.JobPostsTests
{
    [Collection("Sequential")]
    public class CanDeleteJobPost
    {
        private Connection _connection;
        private JobPostsFixture _fixture;

        public CanDeleteJobPost()
        {
            _fixture = new JobPostsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanDeleteJobPostTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "vbn@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returned = EmployerDAO.Create(testEmployer);

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost();

            testPost.Title = "Canalização Aberta";
            testPost.Category = Categories.PLUMBING;
            testPost.Description = "Cozinha sem canalização";
            testPost.Tradable = true;
            testPost.InitialPrice = 66.6;
            testPost.Address = "Rua aberta";
            testPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            JobPost returnedPost = jobPostDAO.Create(returned.Id, testPost);

            //Delete
            Assert.True(jobPostDAO.Delete(returnedPost));

            //After delete
            Categories[] categories = { };
            List<JobPostReturnedModel> jobPosts = jobPostDAO.GetJobs(categories, null, null, null, 1);
            Assert.Empty(jobPosts);

            _fixture.Dispose();
        }
    }
}
