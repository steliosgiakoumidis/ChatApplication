using System;
using System.Text;
using NATS.Client;

namespace ChatApplication.Api
{
    public class Handlers
    {
        private InMemoryStorage _inMemoryStorage;

        public Handlers(InMemoryStorage inMemoryStorage)
        {
            _inMemoryStorage = inMemoryStorage;
        }

        public EventHandler<MsgHandlerEventArgs> RecordMessagesHandler()
        {
            EventHandler<MsgHandlerEventArgs> handler = (sender, args) =>
            {
                var message = args.Message.Data;
                var messageAsString = Encoding.UTF8.GetString(message);
                var username = args.Message.Subject;
                _inMemoryStorage.AddMessage(username, messageAsString);
            };

            return handler;
        }
    }
}
