using _011Global.JobsService.JobInterfaces;
using _011Global.JobsService;
using _011Global.Shared;
using _011Global.JobsService.Entities;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configBuilder) =>
    {
        var env = hostingContext.HostingEnvironment;

        configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        if (env.IsDevelopment())
        {
            configBuilder.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((hostContext, services) =>
    {
        var connString = hostContext.Configuration.GetConnectionString("TransactionsHubDB");
        services.Configure<USAEpaySettings>(hostContext.Configuration.GetSection("USAEpaySettings"));
        services.AddSingleton<CancellationTokenSource>(_ => new CancellationTokenSource())
                .AddTransient<CancellationTokenBase, WorkerCancellationToken>()
                .RegisterDBContexts(connString)
                .LoadInterfacesSingleton<IJob>()
                .AddHostedService<Worker>();
    })
    .UseSystemd()
    .Build();

host.Run();