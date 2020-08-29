using System;
using System.Collections.Generic;
using NATS.Client;

namespace CommonLibraries
{
    public interface INatsIntegration
    {
        void Subscribe(EventHandler<MsgHandlerEventArgs> handler, IEnumerable<string> listOfTopics);
        bool FlushAndUnsubscribe();
        bool PublishMessage(string message, string targetSubject, string sender);
    }
}
