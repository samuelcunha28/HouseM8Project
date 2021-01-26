using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.JobPostsTests
{
    [Collection("Sequential")]
    public class CanCreateJobPost
    {
        private Connection _connection;
        private JobPostsFixture _fixture;

        public CanCreateJobPost()
        {
            _fixture = new JobPostsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanCreateJobPostTest()
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

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost();

            testPost.Title = "Canalização Estourada";
            testPost.Category = Categories.PLUMBING;
            testPost.ImagePath = "path/image";
            testPost.Description = "Grande estouro nos canos da sanita";
            testPost.Tradable = true;
            testPost.InitialPrice = 60.6;
            testPost.Address = "Rua sem fim";
            testPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            JobPost jobReturned = jobPostDAO.Create(returned.Id, testPost);

            Assert.Equal(testPost.Title, jobReturned.Title);
            Assert.Equal(testPost.Category, jobReturned.Category);
            Assert.Equal(testPost.ImagePath, jobReturned.ImagePath);
            Assert.Equal(testPost.Description, jobReturned.Description);
            Assert.Equal(testPost.Tradable, jobReturned.Tradable);
            Assert.Equal(testPost.InitialPrice, jobReturned.InitialPrice);
            Assert.Equal(testPost.Address, jobReturned.Address);
            Assert.Equal(testPost.PaymentMethod, jobReturned.PaymentMethod);

            _fixture.Dispose();
        }
    }
}
