using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.ChatTests
{
    [Collection("Sequential")]
    public class CanGetChats
    {
        private Connection _connection;
        private ChatFixture _fixture;

        public CanGetChats()
        {
            _fixture = new ChatFixture();
            this._connection = _fixture.GetConnection();
        }
        [Fact]
        public void CanGetChatsTest()
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
            int chatId2 = chatDao.CreateChatId();

            Chat chat = new Chat();
            chat.UserId = returned.Id;
            chat.ChatId = chatId;

            chatDao.CreateChat(chat);
            chat.UserId = returnedMate.Id;
            chatDao.CreateChat(chat);

            Chat chat2 = new Chat();
            chat2.UserId = returned.Id;
            chat2.ChatId = chatId2;

            chatDao.CreateChat(chat2);
            chat2.UserId = returnedMate.Id;
            chatDao.CreateChat(chat2);

            Chat[] user1ChatArray = chatDao.GetChats(returned.Id);
            Chat[] user2ChatArray = chatDao.GetChats(returnedMate.Id);

            Assert.Equal(2, user1ChatArray.Length);
            Assert.Equal(2, user2ChatArray.Length);

            Assert.Equal(chatId, user1ChatArray[0].ChatId);
            Assert.Equal(returnedMate.Id, user1ChatArray[0].UserId);
            Assert.Equal(chatId, user2ChatArray[0].ChatId);
            Assert.Equal(returned.Id, user2ChatArray[0].UserId);

            Assert.Equal(chatId2, user1ChatArray[1].ChatId);
            Assert.Equal(returnedMate.Id, user1ChatArray[1].UserId);
            Assert.Equal(chatId2, user2ChatArray[1].ChatId);
            Assert.Equal(returned.Id, user2ChatArray[1].UserId);

            _fixture.Dispose();
        }

    }
}
