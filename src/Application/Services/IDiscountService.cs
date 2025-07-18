namespace TektonChallengeProducts.Application.Services;

public interface IDiscountService
{
    Task<byte> GetDiscountToApplyAsync();
}
