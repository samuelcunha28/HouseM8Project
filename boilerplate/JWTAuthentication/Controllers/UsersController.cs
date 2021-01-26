using JWTAuthentication.Models;
using JWTAuthentication.Services;
using JWTAuthentication.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;

namespace JWTAuthentication.Controllers
{

    [Route("v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult<List<UserDTO>> Get()
        {
            List<User> listUser = _userService.Get();
            List<UserDTO> result = new List<UserDTO>();

            foreach (User user in listUser)
            {
                result.Add(UserToDTO(user));
            }

            return Ok(result);
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<UserDTO> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound(new { message = "ID do Utilizador Inválido!"});
            }

            return UserToDTO(user);
        }

        [HttpPost]
        public ActionResult<UserDTO> Create(User newUser)
        {
            var password = newUser.Password;
            password = Helper.ConvertPasswordToHash(password);
            newUser.Password = password;
            Helper.ConvertHashToPassword(password);

            _userService.Create(newUser);

            return CreatedAtRoute("GetUser", new { id = newUser.Id.ToString() }, UserToDTO(newUser));
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User updatedUser)
        {

            var user = _userService.Get(id);
            updatedUser.Id = id;

            var password = updatedUser.Password;
            password = Helper.ConvertPasswordToHash(password);
            updatedUser.Password = password;

            if (user == null)
            {
                return NotFound(new { message = "ID do Utilizador Inválido!"});
            }

            _userService.Update(id, updatedUser);

            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        //[Authorize]
        public IActionResult Delete(string id)
        {

            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound(new { message = "ID do Utilizador Inválido!"});
            }

            _userService.Remove(id);

            return NoContent();
        }

        private static UserDTO UserToDTO(User user) =>
        new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Role = user.Role
        };
    }

}