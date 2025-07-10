using Microsoft.EntityFrameworkCore;
using _011Global.Shared.JobsServiceDBContext;
using _011Global.CustomerApplication.Interfaces;
using _011Global.CustomerApplication.Services;
using _011Global.Shared.DbContexts.AddressDbContext.Interfaces;
using _011Global.Shared.DbContexts.AddressDbContext.Repos;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.DbContexts.CustomerDbContext.Repos;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Repos;
using _011Global.Shared.USAEpay.Services;
using _011Global.Shared.USAEpay.Intefaces;
using _011Global.JobsService.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<JobsServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICreditCardRepository, CreditCardRepository>();
builder.Services.AddScoped<IUnsubscritionService, UnsubscriptionService>();
builder.Services.AddScoped<IAutorizationService, AutorizationService>();

builder.Services.AddHttpClient<ITokenizationService, TokenizationService>(client =>
{
    client.BaseAddress = new Uri("https://sandbox.usaepay.com/api/v2/");
});

builder.Services.Configure<USAEpaySettings>(
    builder.Configuration.GetSection("USAEpaySettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
