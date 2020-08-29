using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatApplication.Api.StartUpTask
{
    public static class StartUpTaskWebHostExtension
    {
        public static async Task RunWithTaskAsync(this IHost webhost,
                CancellationToken token = default)
        {
            var startupTasks = webhost.Services.GetServices<IStartUpTask>();
            foreach (var task in startupTasks)
            {
                await task.ExecuteAsync(token);
            }
            await webhost.RunAsync(token);
        }
    }
}
