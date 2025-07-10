using System.Globalization;
using _011Global.JobsService.JobInterfaces;
using _011Global.Shared;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.DbContexts.TransactionDbContext;
using _011Global.Shared.DbContexts.TransactionDbContext.Interfaces;
using _011Global.Shared.USAEpay.Intefaces;

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
            var paymentService = scope.ServiceProvider.GetRequiredService<ITransactionService>();
            var tokenizationService = scope.ServiceProvider.GetRequiredService<ITokenizationService>();

            var customers = await customerRepository.GetAllSuscribedClient();

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
                        if (IsPotentialCardNumber(creditCard.Token))
                        {
                            var TokenizedCreditCard = await tokenizationService.TokenizationCard(creditCard.Token, new DateTime(int.Parse(creditCard.ExpirationYear), int.Parse(creditCard.ExpirationMonth), 1));
                            await creditCardRepository.UpdateCreditCardToken(creditCard.CreditCardId, TokenizedCreditCard);
                        }
                        var paymentResult = await paymentService.Charge(customer, creditCard);

                        var transaction = new Transaction
                        {
                            CustomerID = customer.CustomerId,
                            Amount = double.Parse(paymentResult.auth_amount, CultureInfo.InvariantCulture),
                            TransactionStatusID =  VerificationStatus(paymentResult.result),
                            PaymentGWTransID = paymentResult.key,
                            AuthCode = paymentResult.authcode,
                            ResponseCode = paymentResult.result,
                            SubErrorDesc = paymentResult.error,
                            CreationDate = DateTime.UtcNow,
                            CreditCardID = creditCard.CreditCardId
                        };

                        await transactionRepository.SaveTransaction(transaction);
                        if (paymentResult.result == "Approved")                            
                            logger.LogInformation($"Customer {customer.CustomerId} charged successfully.");
                        else
                            logger.LogWarning($"Failed to charge customer {customer.CustomerId} - status {paymentResult.result} error {paymentResult.error}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error charging customer {customer.CustomerId}");
                    }
                }
            }
        }

        private bool IsPotentialCardNumber(string token)
        {
            if (!token.All(char.IsDigit))
                return false;

            if (token.Length < 13 || token.Length > 19)
                return false;

            return true;
        }

        private byte VerificationStatus(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
            {
                return 5;
            }
            var normalized = result.Trim().ToLowerInvariant();

            return normalized switch
            {
                "approved" => 1,
                "partially approved" => 2,
                "declined" => 3,
                "error" => 4,
                _ => 5
            };
        }
    }
}