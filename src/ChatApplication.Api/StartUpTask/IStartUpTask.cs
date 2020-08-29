using System.Threading;
using System.Threading.Tasks;

namespace ChatApplication.Api.StartUpTask
{
    public interface IStartUpTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
