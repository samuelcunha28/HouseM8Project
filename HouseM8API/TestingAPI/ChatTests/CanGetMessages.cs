using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HouseM8APITests.ChatTests
{

    [Collection("Sequential")]
    public class CanGetMessages
    {

        private Connection _connection;
        private ChatFixture _fixture;

        public CanGetMessages()
        {
            _fixture = new ChatFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanGetMessagesTest()
        {
            IEmployerDAO<Employer> employerDAO = new EmployerDAO(_connection);
            Employer testEmployerC = new Employer();

            testEmployerC.FirstName = "Ema";
            testEmployerC.LastName = "Coelho";
            testEmployerC.UserName = "EmiRegi";
            testEmployerC.Password = "123";
            testEmployerC.Email = "ema@gmail.com";
            testEmployerC.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployerC.Address = "Lousada";

            // Employe a utilizar
            Employer returnedEmployer = employerDAO.Create(testEmployerC);

            IMateDAO<Mate> MateDAO = new MateDAO(_connection);

            Mate firstMate = new Mate();

            firstMate.FirstName = "Marcelddo";
            firstMate.LastName = "Carvalho";
            firstMate.UserName = "VeryGoodDev";
            firstMate.Password = "123";
            firstMate.Email = "ghjghdddj@gmail.com";
            firstMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            firstMate.Address = "Sra. Aparecida, Lousada";
            firstMate.Categories = new[] { Categories.CLEANING, Categories.ELECTRICITY };
            firstMate.Rank = Ranks.MATE;
            firstMate.Range = 50;


            Mate returnMate = MateDAO.Create(firstMate);

            ChatDAO chatDAO = new ChatDAO(_connection);

            chatDAO.AddChatHubConnection(returnedEmployer.Id, "connection1");
            chatDAO.AddChatHubConnection(returnMate.Id, "connection2");

            int chatId = chatDAO.CreateChatId();

            Chat chat = new Chat();
            chat.ChatId = chatId;
            chat.UserId = returnedEmployer.Id;
            chatDAO.CreateChat(chat);
            chat.UserId = returnMate.Id;
            chatDAO.CreateChat(chat);
    

            ChatConnection connect1 = chatDAO.GetChatConnectionFromUserId(returnedEmployer.Id);
            ChatConnection connect2 = chatDAO.GetChatConnectionFromUserId(returnMate.Id);

            String MessageSend = "message test";
            String MessageSend2 = "message test 2";

            chatDAO.AddMessage(connect1.Connection, connect2.Connection, MessageSend, DateTime.Now, returnedEmployer.Id);
            chatDAO.AddMessage(connect2.Connection, connect1.Connection, MessageSend2, DateTime.Now, returnMate.Id);

            List<Message> messages = chatDAO.GetMessageList(chatId, returnMate.Id);
            List<Message> messages2 = chatDAO.GetMessageList(chatId, returnedEmployer.Id);

            Assert.Equal(messages.Count, messages2.Count);
            Assert.Equal(2, messages.Count);
            Assert.Equal(2, messages2.Count);

            messages.ToArray();
            messages2.ToArray();

            Assert.Equal("message test", messages[0].MessageSend);
            Assert.Equal("message test", messages2[0].MessageSend);
            Assert.Equal("message test 2", messages[1].MessageSend);
            Assert.Equal("message test 2", messages2[1].MessageSend);

            _fixture.Dispose();
        }
    }
}
