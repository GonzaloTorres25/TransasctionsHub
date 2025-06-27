using _011Global.Shared.CustomerContext.Interfaces;
using _011Global.Shared.CustomerDbContext;
using _011Global.Shared.Exceptions;
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
            try
            {
                await _context.Global_Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AddDBException("The client could not be saved", ex);
            }
        }

        public async Task<Customer?> GetById(int id)
        {
            return await _context.Global_Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task Unscuscribe(Customer customer)
        {
            customer.Subscribed = false;
            _context.Global_Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
