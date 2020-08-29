using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommonLibraries;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.Api.StartUpTask
{
    public class SubscribeToMessageTopics : IStartUpTask
    {
        private IServiceProvider _serviceProvider;
        private Handlers _handlers;
        private const string SubscribeSubject = "User1";

        public SubscribeToMessageTopics(IServiceProvider serviceProvider, Handlers handlers)
        {
            _serviceProvider = serviceProvider;
            _handlers = handlers;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var natsIntegration = scope.ServiceProvider
                    .GetRequiredService<INatsIntegration>();
                natsIntegration.Subscribe(_handlers.RecordMessagesHandler(), new List<string> { SubscribeSubject });
            }
        }
    }
}

