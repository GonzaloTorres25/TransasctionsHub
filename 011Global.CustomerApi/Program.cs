using Microsoft.EntityFrameworkCore;
using _011Global.Shared.JobsServiceDBContext;
using _011Global.CustomerApplication.Interfaces;
using _011Global.CustomerApplication.Services;
using _011Global.Shared.CustomerContext.Interfaces;
using _011Global.Shared.CustomerContext.Repos;
using _011Global.Shared.AddressDbContext.Intefaces;
using _011Global.Shared.AddressDbContext.Repos;
using _011Global.Shared.CreditCardsDbContext.Intefaces;
using _011Global.Shared.CreditCardsDbContext.Repos;

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
