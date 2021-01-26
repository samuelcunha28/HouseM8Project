using HouseM8API.Data_Access;
using Xunit;

namespace HouseM8APITests.ChatTests
{
    [Collection("Sequential")]
    public class CanCreateChatId
    {
        private Connection _connection;
        private ChatFixture _fixture;

        public CanCreateChatId()
        {
            _fixture = new ChatFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanCreateChatIdTest()
        {
            ChatDAO chatDao = new ChatDAO(_connection);

            Assert.True(chatDao.CreateChatId() > 0);

            _fixture.Dispose();
        }
    }
}