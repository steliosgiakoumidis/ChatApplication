using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.Api.StartUpTask
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IStartUpTask
            => services.AddTransient<IStartUpTask, T>();
    }
}
