using _011Global.JobsService.JobInterfaces;
using _011Global.JobsService.Services;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Repos;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.DbContexts.CustomerDbContext.Repos;
using _011Global.Shared.DbContexts.TransactionDbContext.Interfaces;
using _011Global.Shared.DbContexts.TransactionDbContext.Repos;
using _011Global.Shared.JobsServiceDBContext;
using _011Global.Shared.JobsServiceDBContext.Interfaces;
using _011Global.Shared.JobsServiceDBContext.Repos;
using _011Global.Shared.USAEpay.Intefaces;
using _011Global.Shared.USAEpay.Services;
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

            _services.AddScoped<IJobsServiceRepository, JobsServiceRepository>();
            _services.AddScoped<ICustomerRepository, CustomerRepository>();
            _services.AddScoped<ITransactionRepository, TransactionRepository>();
            _services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            _services.AddScoped<ITokenizationService, TokenizationService>();
            _services.AddHttpClient<ITokenizationService, TokenizationService>(client =>
            {
                client.BaseAddress = new Uri("https://sandbox.usaepay.com/api/v2/");
            });

            _services.AddScoped<IAutorizationService, AutorizationService>();

            _services.AddHttpClient<ITransactionService, TransactionService>(client =>
            {
                client.BaseAddress = new Uri("https://sandbox.usaepay.com/api/v2/");
            });

            return _services; 

        }
    }
}
