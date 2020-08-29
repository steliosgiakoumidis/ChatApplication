using System.Collections.Concurrent;
using System.Collections.Generic;
using Serilog;

namespace ChatApplication.Api
{
    public class InMemoryStorage
    {
        private ConcurrentDictionary<string, List<string>> _inMemoryStorage;

        //The instantiation of the List is taking place on the constructor, 
        //that implies that the lifetime of the injected object is critical to be used carefully.
        //It is stringly suggested the class to be injected as singleton otherwise data may be compromised
        public InMemoryStorage()
        {
            _inMemoryStorage = new ConcurrentDictionary<string, List<string>>();
        }

        public void AddMessage(string username, string message)
        {
            if (!_inMemoryStorage.TryGetValue(username, out var listOfMessages))
            {
                _inMemoryStorage.TryAdd(username, new List<string> { message });
            }
            else
            {
                var updatedListOfMessages = listOfMessages;
                updatedListOfMessages.Add(message);
                if (!_inMemoryStorage.TryUpdate(username, updatedListOfMessages, listOfMessages))
                {
                    Log.Error($"Message for user: {username} was not stored properly");
                };
            }
        }

        public List<string> GetMessagesByUserName(string username)
        {
            if (!_inMemoryStorage.TryGetValue(username, out var listOfMessages))
            {
                return new List<string>();
            };

            return listOfMessages;
        }
    }
}
