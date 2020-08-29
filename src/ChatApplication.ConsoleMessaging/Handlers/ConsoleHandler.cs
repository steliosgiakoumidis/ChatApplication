using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApplication.ConsoleMessaging.Handlers
{
    public class ConsoleHandler : IConsoleHandler
    {
        public string ReadConsole()
        {
            return Console.ReadLine();
        }
    }
}
