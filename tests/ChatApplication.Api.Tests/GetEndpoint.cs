using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApplication.Api.Controllers;
using CommonLibraries;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ChatApplication.Api.Tests
{
    public class GetEndpoint
    {
        private Mock<INatsIntegration> _mockNatIntegration = new Mock<INatsIntegration>();
        [Fact]
        public async Task MessagesListIsEmpty()
        {
            var inMemorStorage = new InMemoryStorage();
            var controller = new ChatController(inMemorStorage, _mockNatIntegration.Object);

            var sut = await controller.GetReceivedMessagesForUser("foo");

            Assert.IsType<NotFoundObjectResult>(sut);
        }

        [Fact]
        public async Task ParameterIsEmpty()
        {
            var inMemorStorage = new InMemoryStorage();
            var controller = new ChatController(inMemorStorage, _mockNatIntegration.Object);

            var sut = await controller.GetReceivedMessagesForUser("");

            Assert.IsType<BadRequestObjectResult>(sut);
        }

        [Fact]
        public async Task ListIsReturned()
        {
            var inMemorStorage = new InMemoryStorage();
            inMemorStorage.AddMessage("foo", "hihiihi");
            var controller = new ChatController(inMemorStorage, _mockNatIntegration.Object);

            var sut = await controller.GetReceivedMessagesForUser("foo");

            Assert.IsType<OkObjectResult>(sut);
        }

        [Fact]
        public async Task ListContainsTwoMessages()
        {
            var inMemorStorage = new InMemoryStorage();
            inMemorStorage.AddMessage("foo", "hihihiha");
            inMemorStorage.AddMessage("foo", "hahahaha");
            var controller = new ChatController(inMemorStorage, _mockNatIntegration.Object);

            var sut = await controller.GetReceivedMessagesForUser("foo") as OkObjectResult;

            var a = sut.Value as List<string>;

            Assert.Equal(2, a.Count);
        }
    }
}
