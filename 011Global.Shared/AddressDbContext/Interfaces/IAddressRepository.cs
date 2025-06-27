namespace _011Global.Shared.AddressDbContext.Intefaces
{
    public interface IAddressRepository
    {
        Task Add(GeneralAddress address);
        Task<GeneralAddress?> FindAddressMatch(string countryIso2, string stateIso2, string city, string zipCode, string addressLine);
    }
}
