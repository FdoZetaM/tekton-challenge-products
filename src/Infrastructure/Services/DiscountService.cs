namespace TektonChallengeProducts.Infrastructure.Services;

using Application.Services;
using Microsoft.Extensions.Configuration;

public class DiscountService : IDiscountService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly string discountApiUrl;
    private const int maxRetries = 3;
    private const int retryDelayMilliseconds = 500;

    public DiscountService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this.httpClientFactory = httpClientFactory;
        discountApiUrl = configuration["DiscountApiUrl"]!;
    }

    public async Task<byte> GetDiscountToApplyAsync()
    {
        var client = httpClientFactory.CreateClient(nameof(DiscountService));

        bool isSuccess = false;
        HttpResponseMessage response;
        byte discount = 0;

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                response = await client.GetAsync(discountApiUrl);
                isSuccess = response.IsSuccessStatusCode;

                var content = await response.Content.ReadAsStringAsync();

                if (isSuccess && byte.TryParse(content.Trim(), out discount))
                {
                    return discount;
                }

                await Task.Delay(retryDelayMilliseconds);
            }
            catch (HttpRequestException)
            {
                // Log the exception or handle it as needed
            }
        }

        return discount;
    }
}
