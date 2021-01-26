using System;
using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using Xunit;

namespace HouseM8APITests.WorkTests
{
    [Collection("Sequential")]
    public class CanFindWorkByIdWithAssociatedEmployer
    {
        private Connection _connection;
        private WorkTestFixture _fixture;

        public CanFindWorkByIdWithAssociatedEmployer()
        {
            _fixture = new WorkTestFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanFindWorkByIdWithAssociatedEmployerTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmlgas@gmail.com";
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
            testEmployer.Email = "marceloas@gmail.com";
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
            job.JobPost = jobReturned.Id;
            job.FinishedConfirmedByEmployer = false;
            job.FinishedConfirmedByMate = false;
            job.Employer = returnedEmp.Id;

            IWorkDAO workDAO = new WorkDAO(_connection);
            Job created = workDAO.Create(returnedEmp.Id, job);
            WorkDetailsModel found = workDAO.FindById(created.Id, returnedEmp.Id);

            Assert.NotNull(found);
            
            Assert.Equal(created.Date, found.Date);
            Assert.Equal(created.Mate, found.Mate.Id);
            Assert.Equal(created.JobPost, found.JobPost.Id);
            Assert.Equal(created.FinishedConfirmedByEmployer, found.FinishedConfirmedByEmployer);
            Assert.Equal(created.FinishedConfirmedByMate, found.FinishedConfirmedByMate);
            Assert.Equal(created.Employer, found.Employer.Id);
            
            _fixture.Dispose();
        }
    }
}