using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Services;

namespace UnitTests
{
    [TestClass]
    public class UserManagementControllerTest
    {
        [TestMethod]
        public void GetUser_GetQuerySuccessfullyExecute_ReturnsOkObjectResult()
        {
            var id = Guid.NewGuid();
            var user = new UserForDisplay();

            var userManagementServiceMock = new Mock<IUserManagementService>();
            userManagementServiceMock.Setup(x => x.GetUserById(id))
                .Returns(user);
            var userManagementController = new UserManagementController(userManagementServiceMock.Object);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(claimsPrincipal);

            var actionContext = new ActionContext()
            {
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            };
            actionContext.HttpContext = httpContextMock.Object;

            var controllerContext = new ControllerContext(actionContext);

            userManagementController.ControllerContext = controllerContext;

            var result = userManagementController.GetUser();

            Assert.IsTrue(result.GetType() == typeof(OkObjectResult));
        }

        public void GetUserById_GetQuerySuccessfullyExecute_ReturnsOkObjectResult()
        {
            var id = Guid.NewGuid();
            var user = new UserForDisplay();

            var userManagementServiceMock = new Mock<IUserManagementService>();
            userManagementServiceMock.Setup(x => x.GetUserById(id))
                .Returns(user);
            var userManagementController = new UserManagementController(userManagementServiceMock.Object);

            var result = userManagementController.GetUserById(id);

            Assert.IsTrue(result.GetType() == typeof(OkObjectResult));
        }
       
        [TestMethod]
        public void UpdateUser_PutQuerySuccessfullyExecuted_ReturnsOkObjectResult()
        {
            var id = Guid.NewGuid();
            var user = new UserForUpdate();

            var userManagementServiceMock = new Mock<IUserManagementService>();
            userManagementServiceMock.Setup(x => x.UpdateUser(id, user));

            var userManagementController = new UserManagementController(userManagementServiceMock.Object);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(claimsPrincipal);

            var actionContext = new ActionContext()
            {
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            };
            actionContext.HttpContext = httpContextMock.Object;

            var controllerContext = new ControllerContext(actionContext);

            userManagementController.ControllerContext = controllerContext;

            var result = userManagementController.Update(user);

            Assert.IsTrue(result.GetType() == typeof(OkObjectResult));
        }

        [TestMethod]
        public void AddDog_PostQuerySuccessfullyExecuted_ReturnsOkObjectresult()
        {
            var id = Guid.NewGuid();
            var dog = new DogToBeAdded();

            var userManagementServiceMock = new Mock<IUserManagementService>();
            userManagementServiceMock.Setup(x => x.AddDog(dog, id));

            var userManagementController = new UserManagementController(userManagementServiceMock.Object);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(claimsPrincipal);

            var actionContext = new ActionContext()
            {
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
                RouteData = new Microsoft.AspNetCore.Routing.RouteData()
            };
            actionContext.HttpContext = httpContextMock.Object;

            var controllerContext = new ControllerContext(actionContext);

            userManagementController.ControllerContext = controllerContext;

            var result = userManagementController.Add(dog);

            Assert.IsTrue(result.GetType() == typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateDog_PutQuerySuccessfullyExecuted_ReturnsOkObjectResult()
        {
            var id = Guid.NewGuid();
            var dog = new DogForUpdate();

            var userManagementServiceMock = new Mock<IUserManagementService>();
            userManagementServiceMock.Setup(x => x.UpdateDog(id, dog));

            var userManagementController = new UserManagementController(userManagementServiceMock.Object);

            var result = userManagementController.Update(id, dog);

            Assert.IsTrue(result.GetType() == typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetDogById_GetQuerySuccessfullyExecuted_ReturnsOkObjectResult()
        {
            var id = Guid.NewGuid();
            var dog = new DogFromDB();

            var userManagementServiceMock = new Mock<IUserManagementService>();
            userManagementServiceMock.Setup(x => x.GetDogById(id))
                .Returns(dog);
            var userManagementController = new UserManagementController(userManagementServiceMock.Object);

            var result = userManagementController.GetDog(id);

            Assert.IsTrue(result.GetType() == typeof(OkObjectResult));
        }
    }
}

