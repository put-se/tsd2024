using GoldSavings.App.Model;
using Newtonsoft.Json;

namespace GoldSavings.App.Client;

public class GoldClient
{
    private readonly HttpClient _client;

    public GoldClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://api.nbp.pl/api/");
        _client.DefaultRequestHeaders.Accept.Clear();
    }

    public async Task<GoldPrice> GetCurrentGoldPrice()
    {
        try
        {
            var responseMsg = _client.GetAsync("cenyzlota/").GetAwaiter().GetResult();
            if (responseMsg.IsSuccessStatusCode)
            {
                var content = await responseMsg.Content.ReadAsStringAsync();
                List<GoldPrice>? prices = JsonConvert.DeserializeObject<List<GoldPrice>>(content);
                if (prices != null && prices.Count == 1) return prices[0];
            }

            return null;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"API Request Error: {e.Message}");
            return null;
        }
    }

    public async Task<List<GoldPrice>> GetGoldPrices(DateTime startDate, DateTime endDate)
    {
        var dateFormat = "yyyy-MM-dd";
        var requestUri = $"cenyzlota/{startDate.ToString(dateFormat)}/{endDate.ToString(dateFormat)}";
        var responseMsg = _client.GetAsync(requestUri).GetAwaiter().GetResult();
        if (responseMsg.IsSuccessStatusCode)
        {
            var content = await responseMsg.Content.ReadAsStringAsync();
            List<GoldPrice> prices = JsonConvert.DeserializeObject<List<GoldPrice>>(content);
            return prices;
        }

        return null;
    }
}