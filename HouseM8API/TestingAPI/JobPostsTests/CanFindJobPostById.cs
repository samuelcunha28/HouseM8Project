using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.JobPostsTests
{
    [Collection("Sequential")]
    public class CanFindJobPostById
    {
        private Connection _connection;
        private JobPostsFixture _fixture;

        public CanFindJobPostById()
        {
            _fixture = new JobPostsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanFindJobPostByIdOrReturnNullWhenNotFoundTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "xcv@gmail.com";
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

            JobPost returnedPost = jobPostDAO.Create(returned.Id, testPost);

            Assert.Equal(returnedPost.Id, jobPostDAO.FindById(returnedPost.Id).Id);

            //when not found
            Assert.Null(jobPostDAO.FindById(200));

            _fixture.Dispose();

        }
    }
}
