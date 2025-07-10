using _011Global.CustomerApplication.Common;

namespace _011Global.CustomerApplication.Interfaces
{
    public interface IUnsubscritionService
    {
        Task<ServiceResult> UnsubscribeCustomer(string email);
    }
}
