using _011Global.Shared.JobsServiceDBContext;
using _011Global.Shared.JobsServiceDBContext.Interfaces;
using _011Global.Shared.JobsServiceDBContext.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace _011Global.Shared
{
    public static class DBContextsService
    {
        public static IServiceCollection RegisterDBContexts(this IServiceCollection _services, string? connectionString)
        {
            _services.AddDbContext<JobsServiceContext>(options => options.UseSqlServer(connectionString,
            sqlServerOptions => sqlServerOptions.CommandTimeout(120).EnableRetryOnFailure()));

            _services.AddTransient<IJobsServiceRepository, JobsServiceRepository>();

            return _services; 

        }
    }
}
