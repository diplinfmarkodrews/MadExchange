using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MadXchange.Common.Types
{
    public interface IInstaller
    {
        void InstallService(IServiceCollection services, IConfiguration config);
    }
}