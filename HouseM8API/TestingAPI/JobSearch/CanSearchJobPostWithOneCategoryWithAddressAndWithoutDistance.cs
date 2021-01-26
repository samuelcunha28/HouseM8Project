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
    public class CanSearchJobPostWithOneCategoryWithAddressAndWithoutDistance
    {
        private Connection _connection;
        private JobSearchFixture _fixture;

        public CanSearchJobPostWithOneCategoryWithAddressAndWithoutDistance()
        {
            _fixture = new JobSearchFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanSearchJobPostWithOneCategoryWithAddressAndWithoutDistanceTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "marcfdasfsdafads@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Rua Eng. Luís Carneiro Leão, Figueiró";

            Employer returned = EmployerDAO.Create(testEmployer);

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost();

            testPost.Title = "Canalização Estourada";
            testPost.Category = Categories.PLUMBING;
            testPost.ImagePath = "path/image";
            testPost.Description = "Grande estouro nos canos da sanita";
            testPost.Tradable = true;
            testPost.InitialPrice = 60.6;
            testPost.Address = "London";
            testPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            jobPostDAO.Create(returned.Id, testPost);

            Categories[] categories = { Categories.PLUMBING };

            List<JobPostReturnedModel> jobPosts = jobPostDAO.GetJobs(categories, testEmployer.Address, null, null, 1);

            JobPostReturnedModel[] jobPostsArray = jobPosts.ToArray();

            Assert.Equal("Canalização Estourada", jobPostsArray[0].Title);
            Assert.Equal(Categories.PLUMBING, jobPostsArray[0].Category);
            Assert.Equal("Grande estouro nos canos da sanita", jobPostsArray[0].Description);
            Assert.True(jobPostsArray[0].Tradable);
            Assert.Equal(60.6, jobPostsArray[0].InitialPrice);
            Assert.Equal("London", jobPostsArray[0].Address);
            Assert.Single(jobPostsArray);

            _fixture.Dispose();
        }
    }
}
