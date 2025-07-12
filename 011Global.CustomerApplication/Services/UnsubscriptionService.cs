using _011Global.CustomerApplication.Common;
using _011Global.CustomerApplication.Interfaces;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;

namespace _011Global.CustomerApplication.Services
{
    public class UnsubscriptionService : IUnsubscritionService
    {
        private readonly ICustomerRepository _customerRepository;
        public UnsubscriptionService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ServiceResult> UnsubscribeCustomer(string email)
        {
            ServiceResult serviceresult = new ServiceResult
            {
                Success = true,
                Message = "Customer unsubscribe successfully"
            };
            var customer = await _customerRepository.GetByEmail(email); 
            if (customer == null)
            {
                serviceresult.Success = false;
                serviceresult.Message = $"Customer with email {email} not found";
            }else
            {
                await _customerRepository.UnsubscribeCustomer(customer);
            }
            return serviceresult;
        }
    }
}
