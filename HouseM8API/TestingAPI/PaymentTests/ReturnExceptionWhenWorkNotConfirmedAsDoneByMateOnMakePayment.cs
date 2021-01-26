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
    public class ReturnExceptionWhenWorkNotConfirmedAsDoneByMateOnMakePayment
    {
        private Connection _connection;
        private PaymentTestFixture _fixture;

        public ReturnExceptionWhenWorkNotConfirmedAsDoneByMateOnMakePayment()
        {
            _fixture = new PaymentTestFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void ReturnExceptionWhenWorkNotConfirmedAsDoneByMateOnMakePaymentTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmlgasx@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueiró";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returned = MateDAO.Create(testMate);

            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "marceloasx@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returnedEmp = EmployerDAO.Create(testEmployer);

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost();

            testPost.Title = "Canalização Estourada";
            testPost.Category = Categories.PLUMBING;
            testPost.ImagePath = "path/image";
            testPost.Description = "Grande estouro nos canos da sanita";
            testPost.Tradable = false;
            testPost.InitialPrice = 60.6;
            testPost.Address = "Rua sem fim";
            testPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            JobPost jobReturned = jobPostDAO.Create(returnedEmp.Id, testPost);

            DateTime date = new DateTime(2020, 01, 16);
            Job job = new Job();
            job.Date = date;
            job.Mate = returned.Id;
            job.JobPost = jobReturned.Id;
            job.FinishedConfirmedByEmployer = false;
            job.FinishedConfirmedByMate = false;
            job.Employer = returnedEmp.Id;

            IWorkDAO workDAO = new WorkDAO(_connection);
            Job returnedJob = workDAO.Create(returnedEmp.Id, job);

            workDAO.MarkJobAsDone(returnedJob.Id, returnedEmp.Id);

            Invoice invoice = new Invoice();
            invoice.Value = 60.6;
            invoice.PaymentType = Payment.MONEY;

            PaymentDAO paymentDAO = new PaymentDAO(_connection);
            Assert.Throws<Exception>(() => paymentDAO.makePayment(invoice, returnedJob.Id, returnedEmp.Id));

            _fixture.Dispose();
        }
    }
}
