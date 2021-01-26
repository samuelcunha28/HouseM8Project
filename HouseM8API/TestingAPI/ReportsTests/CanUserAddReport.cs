using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Entities.Enums;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.ReportsTests
{
    [Collection("Sequential")]
    public class CanUserAddReport
    {
        private Connection _connection;
        private ReportTestsFixture _fixture;

        public CanUserAddReport()
        {
            _fixture = new ReportTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanUserAddReportTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmadsdaslg@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueir";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returnedMate = MateDAO.Create(testMate);

            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "marc@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returnedEmployer = EmployerDAO.Create(testEmployer);

            IReportDAO ReportDAO = new ReportDAO(_connection);
            Report reportTest = new Report();
            reportTest.Id = 1;
            reportTest.Reason = Reasons.INADEQUATE_BEHAVIOUR;
            reportTest.ReportedId = 35;
            reportTest.ReporterId = 34;
            reportTest.Comment = "O trabalho que fez nao presta";

            Report repReturned = ReportDAO.ReportUser(returnedEmployer.Id, returnedMate.Id, reportTest);

            Assert.Equal(reportTest.Id, repReturned.Id);
            Assert.Equal(reportTest.Reason, repReturned.Reason);
            Assert.Equal(reportTest.ReportedId, repReturned.ReportedId);
            Assert.Equal(reportTest.ReporterId, repReturned.ReporterId);
            Assert.Equal(reportTest.Comment, repReturned.Comment);

            _fixture.Dispose();
        }
    }
}
