
namespace _011Global.JobsService.Services
{
    public sealed class ServiceCollectionProvider : IServiceCollectionProvider
    {
        public ServiceCollectionProvider(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        public IServiceCollection ServiceCollection { get; }
    }
}
