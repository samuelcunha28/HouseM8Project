using HouseM8API.Configs;
using HouseM8API.Data_Access;
using HouseM8API.Entities;
using HouseM8API.Helpers;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using HouseM8API.Models.ReturnedMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace HouseM8API.Controllers
{
    /// <summary>
    /// Classe Chat Controller, aqui estao todas as funcoes necessarias para realizar o CRUD de mensagens,
    /// e retornar as imagens em tempo real atraves da instancia da classe Hub do SignalR
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IConnection _connection;

        /// <summary>
        /// Construtor de classe ChatController
        /// </summary>
        /// <param name="config">Config com a conexão á BD</param>
        public ChatController(IOptions<AppSettings> config)
        {
            _connection = new Connection(config);
        }

        /// <summary>
        /// Retorna todas as mensagens associadas a um ChatId
        /// </summary>
        /// <remarks>
        /// 
        ///     Get /getMessagesFromChat/{chatId}
        ///
        /// </remarks>  
        /// <param name="chatId">Id do chat</param>
        /// <returns>Retorna a lista de Messagens</returns>
        /// <response code="200">Lista de mensagens</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("getMessagesFromChat/{chatId}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult<List<Message>> GetMessages(int chatId)
        {
            int? loggedUser = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);
            if (loggedUser == null)
            {
                return Unauthorized("Utilizador não autorizado ou inexistente!");
            }

            try
            {

                ChatDAO chatDAO = new ChatDAO(_connection);
                List<Message> messageList = chatDAO.GetMessageList(chatId, (int)loggedUser);
                return Ok(messageList);

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Retorna todos os Ids de chats associados ao user que fez login
        /// </summary>
        /// <remarks>
        /// 
        ///     Get /getChatsFromUser
        ///
        /// </remarks>  
        /// <returns>Retorna os Chats onde o utilizador está presente</returns>
        /// <response code="200">Chats onde o utilizador está presente</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("getChatsFromUser")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult<Chat[]> GetChatsArray()
        {
            int? loggedUser = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (loggedUser == null)
            {
                return Unauthorized("Utilizador não autorizado ou inexistente!");
            }

            try
            {

                ChatDAO chatDAO = new ChatDAO(_connection);
                Chat[] chatArray = chatDAO.GetChats((int)loggedUser);

                return Ok(chatArray);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// Rota que cria um chat para dois utilizadores,
        /// neste caso para o utilizador logado, e o seu alvo para chat 
        /// </summary>
        /// <remarks>
        /// 
        ///     Post /createChat/{matchId}
        ///
        /// </remarks> 
        /// <param name="matchId">Id do utilizador alvo para iniciar chat</param>
        /// <returns>Retorna Mensagem de sucesso caso chat seja criado,
        /// mensagem de erro caso contrário</returns>
        /// <response code="200">Mensagem de sucesso</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Route("createChat/{matchId}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult CreateChat(int matchId)
        {
            int? userId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (userId == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não tem autorização ou não existe!"));
            }

            try
            {

                ChatDAO chatDAO = new ChatDAO(_connection);

                if (chatDAO.getChatIdWithTargetUser((int)userId, matchId) == null)
                {


                    Chat chat = new Chat();
                    chat.UserId = (int)userId;
                    chat.ChatId = chatDAO.CreateChatId();

                    chatDAO.CreateChat(chat);

                    chat.UserId = matchId;
                    chatDAO.CreateChat(chat);

                    return Ok(new SuccessMessageModel("Chat criado!"));
                }

                return Ok(new SuccessMessageModel("O Chat já existe!"));

            }
            catch (Exception e)
            {

                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Rota para obter um chatID de um utilizador alvo ao chat
        /// </summary>
        /// <remarks>
        /// 
        ///     Get /chatConnection/{matchId}
        ///
        /// </remarks> 
        /// <param name="matchId">Id do utilizador alvo para iniciar chat</param>
        /// <returns>Retorna dados da conexão</returns>
        /// <response code="200">Chat Connection</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("chatConnection/{matchId}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult<ChatConnection> getConnectionId(int matchId)
        {

            int? userId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (userId == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não tem autorização ou não existe!"));
            }

            try
            {

                ChatDAO chatDao = new ChatDAO(_connection);
                ChatConnection connect = chatDao.GetChatConnectionFromUserId(matchId);

                if (connect != null)
                {
                    return Ok(connect);
                }

                return BadRequest(new ErrorMessageModel("Não existe connection ID!"));

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Rota para obter um chatID de um utilizador com sessão iniciada
        /// </summary>
        /// <remarks>
        /// 
        ///     Get /chatConnection
        ///
        /// </remarks> 
        /// <returns>Retorna dados da conexão</returns>
        /// <response code="200">Chat Connection</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("chatConnection")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult<ChatConnection> getMyConnectionId()
        {

            int? userId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (userId == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não tem autorização ou não existe!"));
            }

            try
            {

                ChatDAO chatDao = new ChatDAO(_connection);
                ChatConnection connect = chatDao.GetChatConnectionFromUserId((int)userId);

                if (connect != null)
                {
                    return Ok(connect);
                }

                return BadRequest(new ErrorMessageModel("Não existe connection ID!"));

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }

        /// <summary>
        /// Rota para obter a última mensagem de um chat
        /// </summary>
        /// <remarks>
        /// 
        ///     Get /lastMessage/{id}
        ///
        /// </remarks> 
        /// <param name="id">Id do chat</param>
        /// <returns>Retorna última mensagem do chat</returns>
        /// <response code="200">Última mensagem do chat</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("lastMessage/{id}")]
        [Authorize(Roles = "M8,EMPLOYER")]
        public ActionResult<Message> getLastMessageFromChat(int id)
        {

            int? userId = ClaimHelper.GetIdFromClaimIdentity((ClaimsIdentity)this.ControllerContext.HttpContext.User.Identity);

            if (userId == null)
            {
                return Unauthorized(new ErrorMessageModel("Utilizador não tem autorização ou não existe!"));
            }

            try
            {

                ChatDAO chatDao = new ChatDAO(_connection);
                Message message = chatDao.GetLastMessageFromChat(id, (int)userId);

                if (message != null)
                {
                    return Ok(message);
                }

                return BadRequest(new ErrorMessageModel("Não existe connection ID!"));

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageModel(e.Message));
            }
        }
    }
}
