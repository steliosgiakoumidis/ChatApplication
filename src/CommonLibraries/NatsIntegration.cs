using System;
using System.Collections.Generic;
using System.Text;
using NATS.Client;
using Serilog;

namespace CommonLibraries
{
    public class NatsIntegration : INatsIntegration
    {
        private IConnection _connection;
        private IAsyncSubscription _asyncSubscription;
        public NatsIntegration()
        {
            _connection = new ConnectionFactory().CreateConnection(GetOptions());
        }

        private Options GetOptions()
        {
            Options opts = ConnectionFactory.GetDefaultOptions();
            opts.AllowReconnect = true;
            opts.ReconnectWait = 1000;
            opts.AsyncErrorEventHandler += (sender, args) =>
            {
                Log.Error($"Error: Server={args.Conn.ConnectedUrl}, Message={args.Error}, Subject={args.Subscription.Subject}");
            };
            opts.ServerDiscoveredEventHandler += (sender, args) =>
            {
                Log.Error($"A new server has joined the cluster: {String.Join(", ", args.Conn.DiscoveredServers)}");
            };
            opts.ClosedEventHandler += (sender, args) =>
            {
                Log.Error($"Connection Closed: Server= {args.Conn.ConnectedUrl}");
            };
            opts.DisconnectedEventHandler += (sender, args) =>
            {
                Log.Error($"Connection Disconnected: Server= {args.Conn.ConnectedUrl}");
            };

            return opts;
        }

        public void Subscribe(EventHandler<MsgHandlerEventArgs> handler, IEnumerable<string> listOfTopics)
        {
            foreach (var topic in listOfTopics)
            {
                _asyncSubscription = _connection.SubscribeAsync(topic);
                _asyncSubscription.MessageHandler += handler;
                _asyncSubscription.Start();
            }
        }

        public bool FlushAndUnsubscribe()
        {
            try
            {
                _asyncSubscription?.Unsubscribe();
                _connection?.Drain();
                _connection?.Close();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured when closing connection. Error: {ex}");

                return false;
            }
        }

        public bool PublishMessage(string message, string targetSubject, string sender)
        {
            try
            {
                _connection.Publish(targetSubject, Encoding.UTF8.GetBytes($"Sender: {sender}. Timestamp: {DateTime.UtcNow}. Message: {message}"));

                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured during publishing: {ex}");

                return false;
            }
        }
    }
}
