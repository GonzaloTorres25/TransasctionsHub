using _011Global.Shared.DbContexts.CustomerDbContext;

namespace _011Global.Shared.DbContexts.CustomerDbContext.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByEmail(string email);
        Task Add(Customer customer);
        Task Unscuscribe(Customer customer);
        Task<List<Customer>> GetAllSuscribedClient();
    }
}
