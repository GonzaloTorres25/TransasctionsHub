using _011Global.JobsService.JobInterfaces;
using _011Global.Shared;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.DbContexts.TransactionDbContext.Interfaces;

namespace _011Global.JobsService.JobImplementations
{
    public class JobTransaction : Job, IJob
   {
        private readonly IServiceProvider _serviceProvider;

        protected override int IterationWaitTime { get { return 5000; } }

        public JobTransaction(IServiceProvider serviceProvider, CancellationTokenBase cancellationTokenBase, ILogger<JobTransaction> logger)
             : base(cancellationTokenBase, logger)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task WorkLoad(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
            var creditCardRepository = scope.ServiceProvider.GetRequiredService<ICreditCardRepository>();
            var paymentService = scope.ServiceProvider.GetRequiredService<IUSAEpayService>();

            var customers = await customerRepository.GetAll();

            foreach (var customer in customers)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var lastTransaction = await transactionRepository.GetLastVerificationRequiredTransactionByCustomerId(customer.CustomerId);
                var shouldCharge = false;

                if (lastTransaction == null)
                    shouldCharge = true;
                else
                {
                    var daysSinceLastPayment = (DateTime.UtcNow - lastTransaction.CreationDate).TotalDays;
                    if (daysSinceLastPayment >= 30)
                    {
                        shouldCharge = true;
                    }
                }

                if (shouldCharge)
                {
                    try
                    {
                        var creditCard = creditCardRepository.getByCustomerId(customer.CustomerId);
                        var paymentResult = await paymentService.ChargeAsync(customer, creditCard);

                        if (paymentResult.Success)
                        {
                            await transactionRepository.SaveTransaction(paymentResult);
                            logger.LogInformation($"Customer {customer.CustomerId} charged successfully.");
                        }
                        else
                        {
                            logger.LogWarning($"Failed to charge customer {customer.CustomerId} - {paymentResult.ErrorMessage}");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error charging customer {customer.CustomerId}");
                    }
                }
            }
        }
    }
}