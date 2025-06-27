using _011Global.Shared.AddressDbContext.Intefaces;
using _011Global.Shared.JobsServiceDBContext;

namespace _011Global.Shared.AddressDbContext.Repos
{
    public class AddressRepository : IAddressRepository
    {
        private readonly JobsServiceContext _context;
        public AddressRepository(JobsServiceContext context)
        {
            _context = context;
        }

        public async Task Add(GeneralAddress address)
        {
            await _context.Global_Addresses.AddAsync(address);
        }
    }
}
