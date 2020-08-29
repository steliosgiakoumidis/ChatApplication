using ChatApplication.ConsoleMessaging.Handlers;
using CommonLibraries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatApplication.ConsoleMessaging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<INatsIntegration, NatsIntegration>();
                    services.AddSingleton<IConsoleHandler, ConsoleHandler>();
                    services.AddHostedService<Worker>();
                });
    }
}
