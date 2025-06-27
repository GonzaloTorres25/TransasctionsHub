using _011Global.Shared.CustomerDbContext;

namespace _011Global.Shared.CustomerContext.Interfaces
{
    public interface ICustomerRepository
    {
        Task Add (Customer customer);
        Task<Customer?> GetById(int id);      
        Task Unscuscribe(Customer customer);
    }
}
