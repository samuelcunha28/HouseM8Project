using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.ChatTests
{
    [Collection("Sequential")]
    public class CanVerifyIfUserIsNotFromChat
    {
        private Connection _connection;
        private ChatFixture _fixture;

        public CanVerifyIfUserIsNotFromChat()
        {
            _fixture = new ChatFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanVerifyIfUserIsNotFromChatTest()
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

            int chatId = chatDao.CreateChatId();

            Chat chat = new Chat();
            chat.UserId = returned.Id;
            chat.ChatId = chatId;

            chatDao.CreateChat(chat);
            chat.UserId = returnedMate.Id;
            chatDao.CreateChat(chat);

            bool user1IsFromChat = chatDao.IsUserFromChat(999999999, returned.Id);
            bool user2IsFromChat = chatDao.IsUserFromChat(999999999, returnedMate.Id);

            Assert.False(user1IsFromChat);
            Assert.False(user2IsFromChat);

            _fixture.Dispose();
        }
    }
}