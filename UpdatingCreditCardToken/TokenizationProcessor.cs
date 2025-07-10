using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.USAEpay.Intefaces;

public class TokenizationProcessor
{
    private readonly ICreditCardRepository _repo;
    private readonly ITokenizationService _tokenService;

    public TokenizationProcessor(ICreditCardRepository repo, ITokenizationService tokenService)
    {
        _repo = repo;
        _tokenService = tokenService;
    }

    public async Task RunAsync()
    {
        var cards = await _repo.getAllCreditCards();

        foreach (var card in cards)
        {
            try
            {
                if (!IsPotentialCardNumber(card.Token))
                    continue;

                var token = await _tokenService.TokenizationCard(card.Token, new DateTime(int.Parse(card.ExpirationYear), int.Parse(card.ExpirationMonth), 1));
                await _repo.UpdateCreditCardToken(card.CreditCardId, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: something happend, {card.CreditCardId}: {ex.Message}");
            }
        }
    }
    bool IsPotentialCardNumber(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        if (!token.All(char.IsDigit))
            return false;

        if (token.Length < 13 || token.Length > 19)
            return false;

        return true;
    }
}
