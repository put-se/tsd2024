using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GoldSavings.App.Model;

namespace GoldSavings.App.Client;

public class GoldClient
{
    private HttpClient _client;
    public GoldClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://api.nbp.pl/api/");
        _client.DefaultRequestHeaders.Accept.Clear();

    }
    public async Task<GoldPrice> GetCurrentGoldPrice()
    {
        HttpResponseMessage responseMsg = _client.GetAsync("cenyzlota/").GetAwaiter().GetResult();
        if (responseMsg.IsSuccessStatusCode)
        {
            string content = await responseMsg.Content.ReadAsStringAsync();
            List<GoldPrice> prices = JsonConvert.DeserializeObject<List<GoldPrice>>(content);
            if (prices != null && prices.Count == 1)
            {
                return prices[0];
            }
        }
        return null;
    }

    public async Task<List<GoldPrice>> GetGoldPrices(DateTime startDate, DateTime endDate)
    {
        string dateFormat = "yyyy-MM-dd";
        string requestUri = $"cenyzlota/{startDate.ToString(dateFormat)}/{endDate.ToString(dateFormat)}";
        HttpResponseMessage responseMsg = _client.GetAsync(requestUri).GetAwaiter().GetResult();
        if (responseMsg.IsSuccessStatusCode)
        {
            string content = await responseMsg.Content.ReadAsStringAsync();
            List<GoldPrice> prices = JsonConvert.DeserializeObject<List<GoldPrice>>(content);
            return prices;
        }
        else
        {
            return null;
        }

    }

    public IEnumerable<double> GetLowest1(List<GoldPrice> goldPrices) 
    {
        IEnumerable<double> lowest = (from gp in goldPrices
                        orderby gp.Price
                        select gp.Price).Take(3);

        return lowest;
    }

    public IEnumerable<double> GetLowest2(List<GoldPrice> goldPrices) 
    {
        IEnumerable<double> lowest = goldPrices.OrderBy(gp => gp.Price)
                                                .Select(gp => gp.Price)
                                                .Take(3);

        return lowest;
    }

    public IEnumerable<double> GetHighest1(List<GoldPrice> goldPrices) 
    {
        IEnumerable<double> highest = (from gp in goldPrices
                        orderby gp.Price descending 
                        select gp.Price).Take(3);

        return highest;
    }

    public IEnumerable<double> GetHighest2(List<GoldPrice> goldPrices) 
    {
        IEnumerable<double> highest = goldPrices.OrderByDescending(gp => gp.Price)
                                                .Select(gp => gp.Price)
                                                .Take(3);

        return highest;
    }

    public IEnumerable<DateTime> GetProfitDates(List<GoldPrice> goldPrices) 
    {
        IEnumerable<DateTime> profitDates = from jp in goldPrices
                                            from unp in goldPrices
                                            where jp.Date >= new DateTime(2020, 1, 1) && jp.Date <= new DateTime(2020, 1, 31)
                                            && unp.Date >= new DateTime(2020, 1, 1)
                                            && unp.Price/jp.Price >= 1.05
                                            select unp.Date;

        return profitDates;
    }

    public IEnumerable<DateTime> Get3Dates(List<GoldPrice> goldPrices) 
    {
        IEnumerable<DateTime> dates = (from gp in goldPrices
                                      orderby gp.Price descending
                                      select gp.Date).Skip(10).Take(3);

        return dates;
    }

    public double GetAverage(List<GoldPrice> goldPrices)
    {
        return (from gp in goldPrices select gp.Price).Average();
    }

    public List<GoldPrice> GetBestInvestment(List<GoldPrice> goldPrices)
    {
        GoldPrice minimum = (from gp in goldPrices
                            select gp).Min();
        GoldPrice maximum = (from gp in goldPrices
                            select gp).Max();

        return new List<GoldPrice>{minimum, maximum};
    }
}