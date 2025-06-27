using System.Transactions;
using _011Global.JobsService.JobInterfaces;
using _011Global.Shared;
using _011Global.Shared.CustomerContext.Interfaces;
using _011Global.Shared.TransactionDbContext.Interfaces;
/*
 * In Process
namespace _011Global.JobsService.JobImplementations
{
   public class JobTransactionControl : Job, IJob
   {
      private readonly ICustomerRepository _customerRepository;
       private readonly ITransactionRepository _transactionRepository;
       private readonly IUsaePayClient _usaePayClient;

       protected override int IterationWaitTime { get { return 5000; } }

       public JobTransactionControl(CancellationTokenBase cancellationTokenBase, ILogger<JobTransactionControl> logger, ICustomerRepository customerRepository, ITransactionRepository transactionRepository, IUsaePayClient usaePayClient) : base(cancellationTokenBase, logger)
       {
           _customerRepository = customerRepository;
           _transactionRepository = transactionRepository;
           _usaePayClient = usaePayClient;
       }

       protected override async Task WorkLoad(CancellationToken cancellationToken)
       {
           var customers = await _customerRepository.GetAll();

           foreach (var customer in customers)
           {
               if (cancellationToken.IsCancellationRequested)
                   break;

               var lastTransaction = await _transactionRepository.GetLastTransactionByCustomerId(customer.Id); //Get last Client Transaction.
               var shouldCharge = false;

               if (lastTransaction == null)
                   shouldCharge = true; //Never pay

               else
               {
                   var daysSinceLastPayment = (DateTime.UtcNow - lastTransaction.Date).TotalDays;
                   if (daysSinceLastPayment >= 30)
                   {
                       shouldCharge = true;
                   }
               }

               if (shouldCharge)
               {
                   var transactionResult = new Transaction
                   {

                   }
                   //Envia 
                   await _transactionRepository.SaveAsync(transactionResult);
               }
           }
       }
   }
}
*/