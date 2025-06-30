using _011Global.CustomerApplication.DTO;

namespace _011Global.CustomerApplication.Interfaces
{
    public interface IUnsubscritionService
    {
        Task<ServiceResult> UnsubscribeCustomer(string email);
    }
}
