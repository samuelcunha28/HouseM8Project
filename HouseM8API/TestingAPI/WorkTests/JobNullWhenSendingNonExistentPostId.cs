using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using System;
using Xunit;

namespace HouseM8APITests.WorkTests
{
    [Collection("Sequential")]
    public class JobNullWhenSendingNonExistentPostId
    {
        private Connection _connection;
        private WorkTestFixture _fixture;

        public JobNullWhenSendingNonExistentPostId()
        {
            _fixture = new WorkTestFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void JobNullWhenSendingNonExistentMateIdTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmlgs@gmail.com";
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
            testEmployer.Email = "marceloa@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returnedEmp = EmployerDAO.Create(testEmployer);

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

            JobPost jobReturned = jobPostDAO.Create(returnedEmp.Id, testPost);
            DateTime date = new DateTime(2020, 01, 16);
            Job job = new Job();
            job.Date = date;
            job.Mate = returned.Id;
            job.JobPost = 50;
            job.FinishedConfirmedByEmployer = false;
            job.FinishedConfirmedByMate = false;
            job.Employer = returnedEmp.Id;

            IWorkDAO workDAO = new WorkDAO(_connection);
            Job returnedJob = workDAO.Create(returnedEmp.Id, job);

            Assert.Null(returnedJob);

            _fixture.Dispose();
        }
    }
}
