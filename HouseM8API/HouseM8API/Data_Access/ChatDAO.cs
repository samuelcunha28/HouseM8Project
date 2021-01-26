using HouseM8API.Entities;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HouseM8API.Data_Access
{
    /// <summary>
    /// Classe ChatDAO que comunica com a BD para realizar operacoes CRUD de mensagens e chats
    /// </summary>
    public class ChatDAO
    {
        private IConnection _connection;

        /// <summary>
        /// Construtor de classe ChatDAO
        /// </summary>
        /// <param name="connection">Conexão á BD</param>
        public ChatDAO(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Método que adciona uma mensagem a um chat com um ID específico
        /// </summary>
        /// <param name="targetConnectionId">Connection Id do utilizador destinatario do chat</param>
        /// <param name="senderConnectionId">Connection Id do utilizador remetente do chat</param>
        /// <param name="messageText">Mensagem</param>
        /// <param name="time">Data e hora da mensagem</param>
        /// <param name="targetId">Id do destinatário do chat</param>
        /// <returns> true caso seja adicionada a mensagem a DB falso caso contrario</returns>
        public bool AddMessage(string targetConnectionId, string senderConnectionId,
            string messageText, DateTime time, int targetId)
        {
            try
            {
                ChatConnection senderConnection = GetChatConnectionFromConnectionId(senderConnectionId);

                ChatConnection targetConnection;
                int? chatId;
                if (targetConnectionId == null)
                {
                    chatId = getChatIdWithTargetUser(senderConnection.UserId, targetId);
                }
                else
                {
                    targetConnection = GetChatConnectionFromConnectionId(targetConnectionId);
                    chatId = getChatIdWithTargetUser(senderConnection.UserId, targetConnection.UserId);
                }

                if (chatId == null)
                {
                    throw new Exception("Nao existe chat para as duas connections dadas!");
                }

                Message message = new Message();
                message.ChatId = (int)chatId;
                message.MessageSend = messageText;
                message.SenderId = senderConnection.UserId;
                message.Time = time;

                bool returnValue = false;

                if (message.MessageSend != null)
                    using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        {
                            cmd.CommandText = "INSERT INTO dbo.Message(ChatId, Message, SenderId, Time) " +
                                "VALUES(@chatId, @msg, @senderId, @time); SELECT @@Identity";

                            cmd.Parameters.Add("@chatId", SqlDbType.Int).Value = message.ChatId;
                            cmd.Parameters.Add("@msg", SqlDbType.NVarChar).Value = message.MessageSend;
                            cmd.Parameters.Add("@senderId", SqlDbType.Int).Value = message.SenderId;
                            cmd.Parameters.Add("@time", SqlDbType.DateTime).Value = message.Time;
                            cmd.ExecuteScalar();
                            returnValue = true;
                        }
                    }

                return returnValue;

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para obter o Id do chat que um remetente
        /// tem com um destinatário
        /// </summary>
        /// <param name="senderId">Connection Id do utilizador remetente do chat</param>
        /// <param name="targetId">Connection Id do utilizador destinatario do chat</param>
        /// <returns>Retorna o Id caso o chat exista,
        /// null caso contrário</returns>
        public int? getChatIdWithTargetUser(int senderId, int targetId)
        {
            try
            {

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT TOP 1 a.ChatId " +
                                    "FROM dbo.[UserChat] a " +
                                    "INNER JOIN (" +
                                        "SELECT " +
                                            "ChatId, " +
                                            "COUNT(DISTINCT UserId) AS cnt " +
                                        "FROM dbo.[UserChat] " +
                                        "WHERE UserId IN (@Sid, @Tid) " +
                                        "GROUP BY ChatId " +
                                        "HAVING COUNT(DISTINCT UserId) = 2) AS b ON a.ChatId = b.ChatId " +
                                    "WHERE a.UserId IN (@Sid, @Tid)";

                    cmd.Parameters.Add("@Sid", SqlDbType.Int).Value = senderId;
                    cmd.Parameters.Add("@Tid", SqlDbType.Int).Value = targetId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            int chatId = reader.GetInt32(0);
                            return chatId;
                        }
                    }
                }

                return null;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método que cria um novo chat
        /// </summary>
        /// <param name="chat">Objeto chat com id do user e chat ID</param>
        /// <returns> Retorna o objecto Chat criado</returns>
        public Chat CreateChat(Chat chat)
        {
            if (chat.ChatId != 0 && chat.UserId != 0)
            {

                try
                {

                    using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        {
                            cmd.CommandText = "INSERT INTO dbo.UserChat(UserId, ChatId ) " +
                                "VALUES(@userId, @chatId); ";

                            cmd.Parameters.Add("@userId", SqlDbType.Int).Value = chat.UserId;
                            cmd.Parameters.Add("@chatId", SqlDbType.Int).Value = chat.ChatId;
                            cmd.ExecuteScalar();
                        }
                    }

                    return chat;

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
            else
            {
                throw new Exception("Erro! Chat não criado!");
            }
        }

        /// <summary>
        /// Método que cria novo Chat Id 
        /// </summary>
        /// <returns>Retorna o chatId criado</returns>
        public int CreateChatId()
        {
            try
            {

                int chatId;

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    {
                        cmd.CommandText = "INSERT INTO dbo.[Chat] DEFAULT VALUES; SELECT @@Identity";

                        cmd.Parameters.Add("@val", SqlDbType.Int).Value = DBNull.Value;
                        chatId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                return chatId;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Metodo para obter todas as mensagens com userId dado
        /// </summary>
        /// <param name="chatId">Id do chat</param>
        /// <param name="userLogged">Id do user logado</param>
        /// <returns>Retorna Lista de Mensagens</returns>
        public List<Message> GetMessageList(int chatId, int userLogged)
        {
            //obter mensagens do chat
            try
            {

                if (IsUserFromChat(chatId, userLogged) == false)
                {
                    throw new Exception("O utilizador nao pertence a este chat!");
                }

                List<Message> returnMessages = new List<Message>();
                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    if (chatId != 0)
                    {
                        cmd.CommandText = "Select * From dbo.[Message] " +
                            "Where ChatId = @id";
                    }
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = chatId;

                    using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adpt.Fill(table);

                        foreach (DataRow row in table.Rows)
                        {
                            Message message = new Message
                            {
                                Id = int.Parse(row["Id"].ToString()),
                                ChatId = int.Parse(row["ChatId"].ToString()),
                                MessageSend = row["Message"].ToString(),
                                SenderId = int.Parse(row["SenderId"].ToString()),
                                Time = (DateTime)row["Time"]

                            };

                            returnMessages.Add(message);
                        }
                    }

                    //ordenar da mais antiga para a mais recente
                    returnMessages.OrderByDescending(m => m.Time);
                    return returnMessages;

                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Metodo para obter todos chats Ids, dado um userId
        /// </summary>
        /// <param name="userId">Id do utilizador</param>
        /// <returns>Retorna um array de Chats</returns>
        public Chat[] GetChats(int userId)
        {
            try
            {

                List<Chat> chats = new List<Chat>();
                Chat chat;

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    if (userId != 0)
                    {
                        cmd.CommandText = "Select dbo.[UserChat].ChatId From dbo.[UserChat] " +
                            "Where dbo.[UserChat].UserId = @id";
                    }

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;

                    using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adpt.Fill(table);

                        foreach (DataRow row in table.Rows)
                        {
                            chat = new Chat
                            {
                                ChatId = (int)row["ChatId"]
                            };

                            using (SqlCommand cmd2 = _connection.Fetch().CreateCommand())
                            {
                                cmd2.CommandType = CommandType.Text;
                                cmd2.CommandText = "SELECT UserId FROM dbo.[UserChat] " +
                                        "WHERE ChatId=@Cid AND UserId NOT IN (@id)";

                                cmd2.Parameters.Add("@id", SqlDbType.Int).Value = userId;
                                cmd2.Parameters.Add("@Cid", SqlDbType.Int).Value = chat.ChatId;

                                using (SqlDataReader reader = cmd2.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Read();
                                        chat.UserId = reader.GetInt32(0);
                                    }
                                }
                            }

                            chats.Add(chat);
                        }
                    }
                }



                return chats.ToArray();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para adicionar à BD a conexão do Hub
        /// </summary>
        /// <param name="userId">Id do user</param>
        /// <param name="Connection">String de conexão</param>
        public void AddChatHubConnection(int userId, String Connection)
        {

            try
            {

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    {
                        cmd.CommandText = "INSERT INTO dbo.ChatConnections(UserId, ConnectionId) " +
                                    "VALUES(@Uid, @Cid); SELECT @@Identity";

                        cmd.Parameters.Add("@Uid", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@Cid", SqlDbType.NVarChar).Value = Connection;

                        cmd.ExecuteScalar();
                    }
                }

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);

            }

        }

        /// <summary>
        /// Método para apagar uma conexão de Hub da BD
        /// </summary>
        /// <param name="Connection">String de conexão</param>
        public void DeleteChatHubConnection(String Connection)
        {
            try
            {

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM dbo.[ChatConnections] WHERE ConnectionId=@id;";

                    cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = Connection;

                    if (cmd.ExecuteNonQuery() <= 0)
                    {
                        throw new Exception("Conexão nao apagada ou inexistente!");
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para atualizar uma conexão de Hub na BD
        /// </summary>
        /// <param name="userId">Id do user</param>
        /// <param name="Connection">String de conexão</param>
        public void UpdateChatHubConnection(int userId, String Connection)
        {
            try
            {

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "UPDATE dbo.[ChatConnections] " +
                    "SET ConnectionId=@Cid " +
                    "WHERE UserId=@Uid";

                    cmd.Parameters.Add("@Cid", SqlDbType.NVarChar).Value = Connection;
                    cmd.Parameters.Add("@Uid", SqlDbType.Int).Value = userId;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para obter os dados da conexão,
        /// usando a string de conexão como parametro
        /// </summary>
        /// <param name="Connection">string de conexão do ChatHub</param>
        /// <returns>Retorna um objeto ChatConnection</returns>
        public ChatConnection GetChatConnectionFromConnectionId(String Connection)
        {
            try
            {

                ChatConnection connect = null;

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM dbo.[ChatConnections] " +
                        "WHERE ConnectionId=@id";

                    cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = Connection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            connect = new ChatConnection();
                            reader.Read();
                            connect.UserId = reader.GetInt32(0);
                            connect.Connection = reader.GetString(1);
                        }
                    }
                }

                return connect;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método para obter os dados da conexão,
        /// usando o id do utilizador como parametro
        /// </summary>
        /// <param name="userId">Id de utilizador</param>
        /// <returns>Retorna um objeto ChatConnection</returns>
        public ChatConnection GetChatConnectionFromUserId(int userId)
        {
            try
            {

                ChatConnection connect = null;

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM dbo.[ChatConnections] " +
                        "WHERE UserId=@id";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            connect = new ChatConnection();
                            reader.Read();
                            connect.UserId = reader.GetInt32(0);
                            connect.Connection = reader.GetString(1);
                        }
                    }
                }

                return connect;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método que retorna a última mensagem de um chat
        /// </summary>
        /// <param name="chatId">Id do chat</param>
        /// <param name="userId">Id do User</param>
        /// <returns>Retorna objeto da última mensagem do chat</returns>
        public Message GetLastMessageFromChat(int chatId, int userId)
        {

            try
            {

                if (IsUserFromChat(chatId, userId) == false)
                {
                    throw new Exception("O utilizador nao pertence a este chat!");
                }

                Message message = null;

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT TOP 1 * " +
                                "FROM dbo.[Message] " +
                                "WHERE ChatId = @id " +
                                "ORDER BY Id DESC;";

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = chatId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            message = new Message();
                            reader.Read();
                            message.Id = reader.GetInt32(0);
                            message.ChatId = reader.GetInt32(1);
                            message.MessageSend = reader.GetString(2);
                            message.SenderId = reader.GetInt32(3);
                            message.Time = reader.GetDateTime(4);
                        }
                    }
                }

                return message;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Método que verifica se um user está num chat
        /// </summary>
        /// <param name="chatId">Id do chat</param>
        /// <param name="userId">Id do user</param>
        /// <returns>Retorna True caso pertença ao chat, falso caso contrário</returns>
        public bool IsUserFromChat(int chatId, int userId)
        {
            try
            {

                using (SqlCommand cmd = _connection.Fetch().CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM dbo.[UserChat] " +
                                "WHERE UserID=@Uid AND ChatId = @Cid ";

                    cmd.Parameters.Add("@Uid", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@Cid", SqlDbType.Int).Value = chatId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            int id = reader.GetInt32(1);

                            if (id == chatId)
                            {
                                return true;
                            }
                            else
                            {
                                throw new Exception("Os chatIDs são diferentes");
                            }
                        }
                    }
                }

                return false;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
