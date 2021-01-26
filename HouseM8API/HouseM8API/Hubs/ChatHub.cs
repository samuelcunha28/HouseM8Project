using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HouseM8API.Hubs
{
    /// <summary>
    /// Classe ChatHub que extende as funcionalidades de hub, para customização
    /// do envio de mensagens , gestão de entrada e saida de grupos de chat, e autorização
    /// </summary>
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IConnection _connection;

        /// <summary>
        /// Construtor da classe ChatHub
        /// </summary>
        /// <param name="config">Conexão à BD</param>
        public ChatHub(IOptions<AppSettings> config)
        {
            _connection = new Connection(config);
        }

        /// <summary>
        /// Método para enviar uma mensagem privada para um utilizador,
        /// usando connection ID
        /// </summary>
        /// <param name="targetConnectionId">Connection ID de quem recebe a mensagem</param>
        /// <param name="senderConnectionId">Connection ID de quem envia a mensagem</param>
        /// <param name="message">Mensagem</param>
        /// <param name="time">Data e hora em que a mensagem foi enviada</param>
        /// <param name="targetId">Id do destinatário do chat</param>
        /// <param name="senderId">Id do remetente</param>
        /// <returns>Retorna uma task assincrona ReceivePrivateMessage,
        ///  para receber as mensagens enviadas para o socket</returns>
        public async Task SendPrivateMessage(string targetConnectionId, string senderConnectionId,
            string message, DateTime time, int targetId, int senderId)
        {
            try
            {

                ChatDAO chatDao = new ChatDAO(_connection);
                chatDao.AddMessage(targetConnectionId, senderConnectionId, message, time, targetId);

                int? chatId = chatDao.getChatIdWithTargetUser(senderId, targetId);

                if (chatId == null)
                {
                    throw new Exception("Não existe chat com os utilizadores correspondentes aos IDs!");
                }

                if (targetConnectionId != null)
                {
                    if (targetConnectionId.Length > 0)
                    {
                        await Clients.Clients(targetConnectionId, senderConnectionId).SendAsync("ReceivePrivateMessage", message, senderId);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Override de OnConnectedAsync
        /// Este metodo e chamado quando e estabelecida uma conexão ao hub e
        /// adiciona a conexão à BD
        /// </summary>
        /// <returns>Retorna um callback UserConnected com o id de conexão</returns>
        public override async Task OnConnectedAsync()
        {
            try
            {

                await Clients.All.SendAsync("UserConnected", Context.ConnectionId);

                int? userId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)Context.User.Identity);
                if (userId == null)
                {
                    throw new Exception("O utilizador não existe!");
                }

                ChatDAO chatDao = new ChatDAO(_connection);

                if (chatDao.GetChatConnectionFromUserId((int)userId) == null)
                {
                    chatDao.AddChatHubConnection((int)userId, Context.ConnectionId);
                }
                else
                {
                    chatDao.UpdateChatHubConnection((int)userId, Context.ConnectionId);
                }

                await base.OnConnectedAsync();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Override de OnDisconnectedAsync
        /// Este metodo e chamado quando deixa de esistir uma conexão ao hub e
        /// remove a conexão da BD
        /// </summary>
        /// <returns>Retorna uma task assincrona UserDisconnected com o id de conexão</returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            try
            {

                await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);

                int userId = (int)ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)Context.User.Identity);
                ChatDAO chatDao = new ChatDAO(_connection);
                chatDao.DeleteChatHubConnection(Context.ConnectionId);

                await base.OnDisconnectedAsync(ex);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}

