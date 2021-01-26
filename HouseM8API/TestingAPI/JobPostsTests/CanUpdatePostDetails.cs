using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.JobPostsTests
{
    [Collection("Sequential")]
    public class CanUpdatePostDetails
    {
        private Connection _connection;
        private JobPostsFixture _fixture;

        public CanUpdatePostDetails()
        {
            _fixture = new JobPostsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanUpdatePostDetailsTest()
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

            //Change variables from old post to new ones
            JobPost OldPost = jobReturned;
            OldPost.Id = jobReturned.Id;
            OldPost.Title = "Cadeira dificil de Montar";
            OldPost.Category = Categories.FURNITURE_ASSEMBLE;
            OldPost.ImagePath = "";
            OldPost.Description = "Cadeira super complicada";
            OldPost.Tradable = true;
            OldPost.InitialPrice = 63.6;
            OldPost.Address = "Rua com fim";
            OldPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            JobPost newPost = jobPostDAO.UpdatePostDetails(OldPost);


            Assert.Equal(OldPost.Title, newPost.Title);
            Assert.Equal(OldPost.Category, newPost.Category);
            Assert.Equal(OldPost.ImagePath, newPost.ImagePath);
            Assert.Equal(OldPost.Description, newPost.Description);
            Assert.Equal(OldPost.Tradable, newPost.Tradable);
            Assert.Equal(OldPost.InitialPrice, newPost.InitialPrice);
            Assert.Equal(OldPost.Address, newPost.Address);

            _fixture.Dispose();
        }
    }
}
