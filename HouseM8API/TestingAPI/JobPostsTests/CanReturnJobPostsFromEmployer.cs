using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using System.Collections.Generic;
using Xunit;

namespace HouseM8APITests.JobPostsTests
{
    [Collection("Sequential")]
    public class CanReturnJobPostsFromEmployer
    {
        private Connection _connection;
        private JobPostsFixture _fixture;

        public CanReturnJobPostsFromEmployer()
        {
            _fixture = new JobPostsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanReturnJobPostsFromEmployerTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "awsdf@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

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

            JobPost testPost2 = new JobPost();

            testPost2.Title = "Cortar Relva";
            testPost2.Category = Categories.GARDENING;
            testPost2.Description = "Isto parece a amazonia!";
            testPost2.Tradable = true;
            testPost2.InitialPrice = 60.6;
            testPost2.Address = "Rua sem fim";
            testPost2.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            jobPostDAO.Create(returned.Id, testPost);
            jobPostDAO.Create(returned.Id, testPost2);

            List<JobPost> returnedList = jobPostDAO.GetEmployerPosts(returned.Id);

            Assert.Equal(2, returnedList.Count);
            Assert.Equal(testPost.Id, returnedList.Find(a => a.Id == testPost.Id).Id);
            Assert.Equal(testPost2.Id, returnedList.Find(a => a.Id == testPost2.Id).Id);

            _fixture.Dispose();
        }
    }
}
