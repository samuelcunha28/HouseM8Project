using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using Xunit;

namespace HouseM8APITests.JobPostsTests
{
    [Collection("Sequential")]
    public class CanRemovePaymentType
    {
        private Connection _connection;
        private JobPostsFixture _fixture;

        public CanRemovePaymentType()
        {
            _fixture = new JobPostsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanRemovePaymentTypeTest()
        {
            IEmployerDAO<Employer> employerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Ema";
            testEmployer.LastName = "Coelho";
            testEmployer.UserName = "EmiRegi";
            testEmployer.Password = "123";
            testEmployer.Email = "ema@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lousada";

            Employer returnedEmployer = employerDAO.Create(testEmployer);

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost();

            testPost.Title = "Canalização Estourada";
            testPost.Category = Categories.PLUMBING;
            testPost.ImagePath = "path/image";
            testPost.Description = "Grande estouro nos canos da sanita";
            testPost.Tradable = true;
            testPost.InitialPrice = 60.6;
            testPost.Address = "Lousada";
            testPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            JobPost jobReturned = jobPostDAO.Create(returnedEmployer.Id, testPost);

            PaymentModel paymentModel = new PaymentModel();
            paymentModel.payments = Payment.PAYPAL;

            jobPostDAO.RemovePayment(jobReturned.Id, paymentModel);
            JobPost post = jobPostDAO.FindById(jobReturned.Id);

            Assert.DoesNotContain(Payment.PAYPAL, post.PaymentMethod);

            _fixture.Dispose();
        }
    }
}
