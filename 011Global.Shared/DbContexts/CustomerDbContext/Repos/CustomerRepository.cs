using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.Exceptions;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace _011Global.Shared.DbContexts.CustomerDbContext.Repos
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly JobsServiceContext _context;
        private readonly ILogger<CustomerRepository> _logger;
        public CustomerRepository(JobsServiceContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Add(Customer customer)
        {
            if (await GetByEmail(customer.CustomerEmail) != null)
                throw new AddDBException("A customer with this email already exists.");
            try
            {
                await _context.Global_Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving customer {@Customer}", customer);
                throw new AddDBException("The client could not be saved", ex);
            }
        }

        public Task<IDbContextTransaction> BeginTransactionDbAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public Task<List<Customer>> GetAllSuscribedClient()
        {
            return _context.Global_Customers.Where(c => c.Subscribed).ToListAsync();
        }

        public async Task<Customer?> GetByEmail(string email)
        {
            return await _context.Global_Customers.FirstOrDefaultAsync(c => c.CustomerEmail == email);
        }

        public async Task SubscribeCustomer(Customer customer)
        {
            customer.Subscribed = true;
            _context.Global_Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribeCustomer(Customer customer)
        {
            customer.Subscribed = false;
            _context.Global_Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
