using Models;
using System;
using Enums;
using Xunit;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Entities;

namespace HouseM8APITests.PaymentTests
{
    [Collection("Sequential")]
    public class CanMakePaymentPayPal
    {
        private readonly Connection _connection;
        private readonly PaymentTestFixture _fixture;

        public CanMakePaymentPayPal()
        {
            _fixture = new PaymentTestFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanMakePaymentPayPalTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate
            {
                FirstName = "Miguel",
                LastName = "Dev",
                UserName = "DevMig",
                Password = "123",
                Email = "DevMIGmlgas@gmail.com",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                Address = "Figueiró",
                Categories = new[] { Categories.CLEANING, Categories.PLUMBING },
                Rank = Ranks.SUPER_MATE,
                Range = 20
            };

            Mate returned = MateDAO.Create(testMate);

            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer
            {
                FirstName = "Marcelo",
                LastName = "Carvalho",
                UserName = "VeryGoodDev",
                Password = "123",
                Email = "marceloas@gmail.com",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                Address = "Lixa"
            };

            Employer returnedEmp = EmployerDAO.Create(testEmployer);

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost
            {
                Title = "Canalização Estourada",
                Category = Categories.PLUMBING,
                ImagePath = "path/image",
                Description = "Grande estouro nos canos da sanita",
                Tradable = false,
                InitialPrice = 60.6,
                Address = "Rua sem fim",
                PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY }
            };

            JobPost jobReturned = jobPostDAO.Create(returnedEmp.Id, testPost);

            DateTime date = new DateTime(2020, 01, 16);
            Job job = new Job
            {
                Date = date,
                Mate = returned.Id,
                JobPost = jobReturned.Id,
                FinishedConfirmedByEmployer = false,
                FinishedConfirmedByMate = false,
                Employer = returnedEmp.Id
            };

            IWorkDAO workDAO = new WorkDAO(_connection);
            Job returnedJob = workDAO.Create(returnedEmp.Id, job);

            workDAO.MarkJobAsDone(returnedJob.Id, returnedEmp.Id);
            workDAO.MarkJobAsDone(returnedJob.Id, returned.Id);

            Invoice invoice = new Invoice
            {
                Value = 60.6,
                PaymentType = Payment.PAYPAL
            };

            string email = "sb-w7dsp4008171@business.example.com";
            string paypalLink = String.Format("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&amount={0}&currency_code=EUR&business={1}&item_name={2}&return=Page", testPost.InitialPrice, email, testPost.Title);

            PaymentDAO paymentDAO = new PaymentDAO(_connection);
            Invoice result = paymentDAO.makePayment(invoice, returnedJob.Id, returnedEmp.Id);

            Assert.Equal(invoice.Value, result.Value);
            Assert.Equal(invoice.PaymentType, result.PaymentType);
            Assert.False(result.ConfirmedPayment);
            Assert.Equal(paypalLink, result.Link);

            _fixture.Dispose();
        }
    }
}
