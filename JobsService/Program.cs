using _011Global.JobsService.JobInterfaces;
using _011Global.JobsService;
using _011Global.JobsService.Services;
using _011Global.Shared;



 //Read
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {

        services.AddSingleton<CancellationTokenSource>(_ => (new CancellationTokenSource()))
        .AddTransient<CancellationTokenBase, WorkerCancellationToken>()
        .RegisterDBContexts(host.Configuration.GetConnectionString("TransactionsHubDB"))
        .LoadInterfacesSingleton<IJob>()
        .AddHostedService<Worker>();


    })
    .UseSystemd()
    .ConfigureAppConfiguration(configBuilder=>
    {
        var env =  Environment.GetEnvironmentVariables()["DOTNET_ENVIRONMENT"];
        configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", true, true);
    })
    .Build();
 

host.Run();
