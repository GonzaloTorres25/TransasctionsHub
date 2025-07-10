using _011Global.Shared.DbContexts.AddressDbContext.Interfaces;
using _011Global.Shared.Exceptions;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _011Global.Shared.DbContexts.AddressDbContext.Repos
{
    public class AddressRepository : IAddressRepository
    {
        private readonly JobsServiceContext _context;
        private readonly ILogger<AddressRepository> _logger;
        public AddressRepository(JobsServiceContext context, ILogger<AddressRepository> logger)
        {
            _context = context;
            _logger = logger;
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
                _logger.LogError(ex, "Error saving address {@Address}", address);
                throw new AddDBException("The address could not be saved ", ex);
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
