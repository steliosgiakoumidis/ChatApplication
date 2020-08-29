using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatApplication.ConsoleMessaging.Handlers;
using CommonLibraries;
using Microsoft.Extensions.Hosting;
using NATS.Client;
using Serilog;

namespace ChatApplication.ConsoleMessaging
{
    public class Worker : BackgroundService
    {
        private const string Sender = "User1";
        private const string Target = "User1";
        private INatsIntegration _natsIntegration;
        private IConsoleHandler _consoleHandler;

        public Worker(INatsIntegration natsIntegration, IConsoleHandler consoleHandler)
        {
            _natsIntegration = natsIntegration;
            _consoleHandler = consoleHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var messageHandler = GetEventHandler();
            _natsIntegration.Subscribe(messageHandler, new List<string> { Target });
            var inputMessage = String.Empty;
            while (inputMessage != "q")
            {
                Console.WriteLine("Please enter your message below");
                inputMessage = _consoleHandler.ReadConsole();
                if (inputMessage == "q" || inputMessage == String.Empty)
                    continue;

                if (!_natsIntegration.PublishMessage(inputMessage, Target, Sender))
                {
                    Log.Error($"An error occured whne publishing message. Input message: {inputMessage}" +
                        $"Target: {Target}, Sender: {Sender}");
                }
            }

            _natsIntegration.FlushAndUnsubscribe();
        }

        private EventHandler<MsgHandlerEventArgs> GetEventHandler()
        {
            EventHandler<MsgHandlerEventArgs> handler = (sender, args) =>
            {
                var message = args.Message.Data;
                var messageString = Encoding.UTF8.GetString(message);
                Console.WriteLine(messageString);
            };

            return handler;
        }
    }
}
