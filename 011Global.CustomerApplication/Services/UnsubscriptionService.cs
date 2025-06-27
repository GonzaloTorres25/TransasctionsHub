using _011Global.CustomerApplication.DTO;
using _011Global.CustomerApplication.Interfaces;
using _011Global.Shared.CustomerContext.Interfaces;

namespace _011Global.CustomerApplication.Services
{
    public class UnsubscriptionService : IUnsubscritionService
    {
        private readonly ICustomerRepository _customerRepository;
        public UnsubscriptionService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ServiceResult> UnsubscribeCustomer(int customerId)
        {
            var customer = await _customerRepository.GetById(customerId);
            if (customer == null)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Customer with Id {customerId} not found"
                };
            }
            await _customerRepository.Unscuscribe(customer);

            return new ServiceResult
            {
                Success = true,
                Message = "Customer unsubscribe successfully"
            };
        }
    }
}
