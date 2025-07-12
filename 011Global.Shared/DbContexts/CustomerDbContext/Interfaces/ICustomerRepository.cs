using Microsoft.EntityFrameworkCore.Storage;

namespace _011Global.Shared.DbContexts.CustomerDbContext.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByEmail(string email);
        Task Add(Customer customer);
        Task UnsubscribeCustomer(Customer customer);
        Task<List<Customer>> GetAllSuscribedClient();
        Task<IDbContextTransaction> BeginTransactionDbAsync();
        Task SubscribeCustomer(Customer customer);
    }
}
