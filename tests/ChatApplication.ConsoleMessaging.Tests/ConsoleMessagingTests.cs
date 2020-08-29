using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChatApplication.ConsoleMessaging.Handlers;
using CommonLibraries;
using Moq;
using NATS.Client;
using Xunit;

namespace ChatApplication.ConsoleMessaging.Tests
{
    public class ConsoleMessagingTests
    {
        private Mock<INatsIntegration> _mockINatsIntegration = new Mock<INatsIntegration>();
        private Mock<IConsoleHandler> _mockConsoleHandler = new Mock<IConsoleHandler>();
        [Fact]
        public async Task WhenQIsTypedChatClientCloses()
        {
            _mockINatsIntegration.Setup(x => x.Subscribe(It.IsAny<EventHandler<MsgHandlerEventArgs>>(), It.IsAny<List<string>>()));
            _mockConsoleHandler.Setup(x => x.ReadConsole()).Returns("q");
            var worker = new Worker(_mockINatsIntegration.Object, _mockConsoleHandler.Object);
            await worker.StartAsync(new CancellationToken());

            _mockINatsIntegration.Verify(x => x.PublishMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
