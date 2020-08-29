using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using ChatApplication.Api.Controllers;
using ChatApplication.Api.Models;
using CommonLibraries;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ChatApplication.Api.Tests
{
    public class PostEndpoint
    {
        private Mock<INatsIntegration> _mockNatIntegration = new Mock<INatsIntegration>();
        [Fact]
        public async Task ChatMessageObjectInIncorrect()
        {
            _mockNatIntegration.Setup(x => x.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var inMemoryStorage = new InMemoryStorage();
            var controller = new ChatController(inMemoryStorage, _mockNatIntegration.Object);
            var chatMessage = new ChatMessage { From = "User1", Message = "hihihi"};
            var sut = await controller.SendMessageToUser(chatMessage);

            Assert.IsType<BadRequestObjectResult>(sut);
        }

        [Fact]
        public async Task PublishingMessageFailed()
        {
            _mockNatIntegration.Setup(x => x.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var inMemoryStorage = new InMemoryStorage();
            var controller = new ChatController(inMemoryStorage, _mockNatIntegration.Object);
            var chatMessage = new ChatMessage { From = "User1", Message = "hihihi" , To = "User1"};
            var sut = await controller.SendMessageToUser(chatMessage);

            Assert.IsType<Microsoft.AspNetCore.Mvc.StatusCodeResult>(sut);
        }

        [Fact]
        public async Task PublishingMessageCompleted()
        {
            _mockNatIntegration.Setup(x => x.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var inMemoryStorage = new InMemoryStorage();
            var controller = new ChatController(inMemoryStorage, _mockNatIntegration.Object);
            var chatMessage = new ChatMessage { From = "User1", Message = "hihihi", To = "User1" };
            var sut = await controller.SendMessageToUser(chatMessage);

            Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(sut);
        }
    }
}
