using _011Global.Shared.CustomerDbContext;

namespace _011Global.Shared.CustomerContext.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAll();
        Task Add (Customer customer);
    }
}
