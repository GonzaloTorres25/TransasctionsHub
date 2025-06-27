using _011Global.CustomerApplication.DTO;

namespace _011Global.CustomerApplication.Interfaces
{
    public interface ISubscriptionService
    {
        Task<ServiceResult> SubscribeCustomer(SubscribeRequest request);
        Task<ServiceResult> UnsuscribeCustomer(int customerId);
    }
}
