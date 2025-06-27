using _011Global.Shared.CustomerContext.Interfaces;
using _011Global.Shared.CustomerDbContext;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore;

namespace _011Global.Shared.CustomerContext.Repos
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly JobsServiceContext _context;
        public CustomerRepository(JobsServiceContext context)
        {
            _context = context;
        }

        public async Task Add(Customer customer)
        {
            await _context.Global_Customers.AddAsync(customer);
        }

        public async Task<List<Customer>> GetAll()
        {
            return await _context.Global_Customers.ToListAsync();
        }
    }
}
