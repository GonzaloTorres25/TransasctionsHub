namespace _011Global.Shared.USAEpay.Intefaces
{
    public interface ITokenizationService
    {
        Task<string> TokenizationCard(string creditCardNumber, DateTime expiration);
    }
}
