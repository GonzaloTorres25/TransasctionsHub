using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Repos;
using _011Global.Shared.JobsServiceDBContext;
using _011Global.Shared.USAEpay.Intefaces;
using _011Global.Shared.USAEpay.Services;
using _011Global.JobsService.Entities;

class Program
{
    public static async Task Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddUserSecrets<Program>();
                    })
                    .ConfigureServices((context, services) =>
                    {
                        var config = context.Configuration;

                        services.AddDbContext<JobsServiceContext>(options =>
                            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

                        services.AddScoped<ICreditCardRepository, CreditCardRepository>();
                        services.AddHttpClient<ITokenizationService, TokenizationService>(client =>
                        {
                            client.BaseAddress = new Uri("https://sandbox.usaepay.com/api/v2/");
                        });
                        services.AddScoped<IAutorizationService, AutorizationService>();
                        services.AddScoped<TokenizationProcessor>();

                        services.Configure<USAEpaySettings>(
                            config.GetSection("USAEpaySettings"));
                    })
                    .Build();

        var processor = host.Services.GetRequiredService<TokenizationProcessor>();
        await processor.RunAsync();
    }
}