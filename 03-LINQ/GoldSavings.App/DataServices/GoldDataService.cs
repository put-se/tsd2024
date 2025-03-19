using GoldSavings.App.Client;
using GoldSavings.App.Model;

namespace GoldSavings.App.Services;

public class GoldDataService
{
    private readonly GoldClient _goldClient;

    public GoldDataService()
    {
        _goldClient = new GoldClient();
    }

    public async Task<List<GoldPrice>> GetGoldPrices(DateTime startDate, DateTime endDate)
    {
        var prices = new List<GoldPrice>();

        while (endDate - startDate > TimeSpan.FromDays(365))
        {
            var startDate2 = startDate.AddDays(365);
            prices = prices.Concat(await _goldClient.GetGoldPrices(startDate, startDate2)).ToList();
            startDate = startDate2;
        }

        prices = prices.Concat(await _goldClient.GetGoldPrices(startDate, endDate)).ToList();

        return prices; // Prevent null values
    }
}