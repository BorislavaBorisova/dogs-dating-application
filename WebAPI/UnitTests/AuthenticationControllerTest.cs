using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebAPI.Controllers;
using WebAPI.Models;
using WebAPI.Services;


namespace UnitTests
{
    [TestClass]
    public class AuthenticationControllerTest
    {
        [TestMethod]
        public void Register_NullUser_ThrowsException()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.Register(null))
                .Throws(new ArgumentNullException());
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object);
            var result = (BadRequestObjectResult)authenticationController.Register(null);
            var json = JsonConvert.SerializeObject(result.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)dictionary["Success"] == false);
        }

        [TestMethod]
        public void Register_NotUniqueUsername_ThrowsException()
        {
            UserForRegister userForRegister = new UserForRegister();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.Register(userForRegister))
                .Throws(new InvalidOperationException("Could not register user. Username already exists."));
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object);
            var result = (BadRequestObjectResult)authenticationController.Register(userForRegister);
            var json = JsonConvert.SerializeObject(result.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)dictionary["Success"] == false);
            Assert.AreEqual("Could not register user. Username already exists.", dictionary["Error"]);
        }

        [TestMethod]
        public void Register_NotMatchingPasswords_ThrowsException()
        {
            UserForRegister userForRegister = new UserForRegister();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.Register(userForRegister))
                .Throws(new InvalidOperationException("Could not register user. Passwords don't match."));
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object);
            var result = (BadRequestObjectResult)authenticationController.Register(userForRegister);
            var json = JsonConvert.SerializeObject(result.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)dictionary["Success"] == false);
            Assert.AreEqual("Could not register user. Passwords don't match.", dictionary["Error"]);
        }

        [TestMethod]
        public void Register_ValidUser_IsSuccessful()
        {
            UserForRegister userForRegister = new UserForRegister();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.Register(userForRegister));
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object);
            var result = (OkObjectResult)authenticationController.Register(userForRegister);
            var json = JsonConvert.SerializeObject(result.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)dictionary["Success"] == true);
        }

        [TestMethod]
        public void LogIn_NullUser_ThrowsException()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.LogIn(null))
                .Throws(new ArgumentNullException());
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object);
            var result = (BadRequestObjectResult)authenticationController.LogIn(null);
            var json = JsonConvert.SerializeObject(result.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)dictionary["Success"] == false);
        }

        [TestMethod]
        public void LogIn_UnregisteredUser_ThrowsException()
        {
            UserForLogIn userForLogIn = new UserForLogIn();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.LogIn(userForLogIn))
                .Throws(new InvalidOperationException("Login failed: No such user."));
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object);
            var result = (BadRequestObjectResult)authenticationController.LogIn(userForLogIn);
            var json = JsonConvert.SerializeObject(result.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)dictionary["Success"] == false);
            Assert.AreEqual("Login failed: No such user.", dictionary["Error"]);
        }

        [TestMethod]
        public void LogIn_UserWithWrongPassword_ThrowsException()
        {
            UserForLogIn userForLogIn = new UserForLogIn();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.LogIn(userForLogIn))
                .Throws(new InvalidOperationException("Login failed: Wrong password."));
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object);
            var result = (BadRequestObjectResult)authenticationController.LogIn(userForLogIn);
            var json = JsonConvert.SerializeObject(result.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)dictionary["Success"] == false);
            Assert.AreEqual("Login failed: Wrong password.", dictionary["Error"]);
        }

        [TestMethod]
        public void LogIn_ValidUser_IsSuccessful()
        {
            UserForLogIn userForLogIn = new UserForLogIn();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.LogIn(userForLogIn))
                .Returns("Some JWT");
            var authenticationController = new AuthenticationController(authenticationServiceMock.Object);
            var result = (OkObjectResult)authenticationController.LogIn(userForLogIn);
            var json = JsonConvert.SerializeObject(result.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            Assert.IsTrue((bool)dictionary["Success"] == true);
        }
    }
}
