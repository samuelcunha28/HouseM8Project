using Models;
using System;
using Enums;
using Xunit;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;

namespace HouseM8APITests.WorkTests
{
    [Collection("Sequential")]
    public class ReturnExceptionWhenJobIDisInvalidOnMarkJobAsDone
    {
        private Connection _connection;
        private WorkTestFixture _fixture;

        public ReturnExceptionWhenJobIDisInvalidOnMarkJobAsDone()
        {
            _fixture = new WorkTestFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void ReturnExceptionWhenJobIDisInvalidOnMarkJobAsDoneTest()
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

            IWorkDAO workDAO = new WorkDAO(_connection);

            Assert.Throws<Exception>(() => workDAO.MarkJobAsDone(1000, returnedEmp.Id));
            Assert.Throws<Exception>(() => workDAO.MarkJobAsDone(1000, returned.Id));

            _fixture.Dispose();
        }
    }
}
