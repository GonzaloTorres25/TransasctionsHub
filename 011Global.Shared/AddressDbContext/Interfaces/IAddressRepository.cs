namespace _011Global.Shared.AddressDbContext.Intefaces
{
    public interface IAddressRepository
    {
        Task Add(GeneralAddress address);
    }
}
