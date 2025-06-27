using _011Global.Shared.AddressDbContext.Intefaces;
using _011Global.Shared.Exceptions;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                await _context.Global_Addresses.AddAsync(address);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AddDBException("The address could not be saved", ex);
            }
        }

        public async Task<GeneralAddress?> FindAddressMatch(string countryIso2, string stateIso2, string city, string zipCode, string addressLine)
        {
            return await _context.Global_Addresses.FirstOrDefaultAsync(a =>
                a.CountryIso2 == countryIso2 &&
                a.StateIso2 == stateIso2 &&
                a.City == city &&
                a.ZipCode == zipCode &&
                a.Address == addressLine);
        }
    }
}
