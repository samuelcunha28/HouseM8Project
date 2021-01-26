using JWTAuthentication.Controllers;
using JWTAuthentication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace web_api_tests
{
    public class UserControllerTests
    {

        UserController _controller;
        IUserService _service;

        public UserControllerTests()
        {
            _service = new UserServiceFake();
            _controller = new UserController(_service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOKResult()
        {
            var okResult = _controller.Get();
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
    }
}
