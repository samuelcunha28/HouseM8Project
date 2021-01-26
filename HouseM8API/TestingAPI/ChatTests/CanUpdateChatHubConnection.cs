using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using Xunit;

namespace HouseM8APITests.ChatTests
{
    [Collection("Sequential")]
    public class CanUpdateChatHubConnection
    {
        private Connection _connection;
        private ChatFixture _fixture;

        public CanUpdateChatHubConnection()
        {
            _fixture = new ChatFixture();
            this._connection = _fixture.GetConnection();
        }
        [Fact]
        public void CanUpdateChatHubConnectionTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "mc@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returned = EmployerDAO.Create(testEmployer);

            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmlg@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueiró";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returnedMate = MateDAO.Create(testMate);

            ChatDAO chatDao = new ChatDAO(_connection);

            chatDao.AddChatHubConnection(returned.Id, "connection1");
            chatDao.AddChatHubConnection(returnedMate.Id, "connection2");

            chatDao.UpdateChatHubConnection(returned.Id, "connection1Replaced");
            chatDao.UpdateChatHubConnection(returnedMate.Id, "connection2Replaced");

            ChatConnection connection1 = chatDao.GetChatConnectionFromUserId(returned.Id);
            ChatConnection connection2 = chatDao.GetChatConnectionFromUserId(returnedMate.Id);

            Assert.Equal("connection1Replaced", connection1.Connection);
            Assert.Equal(returned.Id, connection1.UserId);
            Assert.Equal("connection2Replaced", connection2.Connection);
            Assert.Equal(returnedMate.Id, connection2.UserId);

            _fixture.Dispose();
        }
    }
}