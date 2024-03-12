using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
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

    public async void GetTop3HighestPrices() {
        List<GoldPrice> prices = await GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31));
        List<double> pricesQS = (from price in prices orderby price.Price descending select price.Price).Take(3).ToList();
        List<double> pricesMS = prices.OrderByDescending(prices => prices.Price).Select(prices => prices.Price).Take(3).ToList();
        Console.WriteLine($"Query syntax:");
        foreach(var price in pricesQS) {
            Console.Write($"{price}; ");
        }
        Console.WriteLine($"\nMethod syntax:");
        foreach(var price in pricesMS) {
            Console.Write($"{price}; ");
        }
        Console.WriteLine();
    }

    public async void GetTop3LowestPrices() {
        List<GoldPrice> prices = await GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31));
        List<double> pricesQS = (from price in prices orderby price.Price ascending select price.Price).Take(3).ToList();
        List<double> pricesMS = prices.OrderBy(prices => prices.Price).Select(prices => prices.Price).Take(3).ToList();
        Console.WriteLine($"\nQuery syntax:");
        foreach(var price in pricesQS) {
            Console.Write($"{price}; ");
        }
        Console.WriteLine($"\nMethod syntax:");
        foreach(var price in pricesMS) {
            Console.Write($"{price}; ");
        }
        Console.WriteLine();
    }

    public async void WouldEearnMoreThan5() {
        List<GoldPrice> prices = new List<GoldPrice>();
        DateTime startDate = new DateTime(2019, 01, 01);
        DateTime endDate = startDate.AddYears(1);
        prices.AddRange(await GetGoldPrices(startDate, endDate));
        for (int i = 0; i < 3; i++) {
            startDate = endDate;
            endDate = startDate.AddYears(1);
            prices.AddRange(await GetGoldPrices(startDate, endDate));
        }
        
        
        double buyingPrice = prices.First().Price;
        List<DateTime> datesQS = (from price in prices where price.Price >= 1.05 * buyingPrice select price.Date).ToList();
        List<DateTime> datesMS = prices.Where(price => price.Price >= 1.05 * buyingPrice).Select(price => price.Date).ToList();
        Console.WriteLine("\nQuery syntax:");
        foreach(var date in datesQS) {
            Console.Write($"{date}; ");
        }
        Console.WriteLine("\nMethod syntax:");
        foreach(var date in datesMS) {
            Console.Write($"{date}; ");
        }
        Console.WriteLine();

    }

    public async void GetTop3ForSecondTen() {
        List<GoldPrice> prices = new List<GoldPrice>();
        DateTime startDate = new DateTime(2019, 01, 01);
        DateTime endDate = startDate.AddYears(1);
        prices.AddRange(await GetGoldPrices(startDate, endDate));
        for (int i = 0; i < 3; i++) {
            startDate = endDate;
            endDate = startDate.AddYears(1);
            prices.AddRange(await GetGoldPrices(startDate, endDate));
        }

        List<GoldPrice> rankedPricesQS = (from price in prices orderby price.Price descending select price).ToList();
        var tempQS = rankedPricesQS.Skip(10).Take(3);
        List<DateTime> datesQS = (from price in tempQS select price.Date).ToList();
        List<GoldPrice> rankedPricesMS = prices.OrderByDescending(price => price.Price).ToList();
        var tempMS = rankedPricesQS.Skip(10).Take(3);
        List<DateTime> datesMS = tempMS.Select(price => price.Date).ToList();

        Console.WriteLine($"\nQuery syntax:");
        foreach(var date in datesQS) {
            Console.Write($"{date}; ");
        }
        Console.WriteLine("\nMethod syntax:");
        foreach(var date in datesMS) {
            Console.Write($"{date}; ");
        }
        Console.WriteLine();
    }

    public async void GetAverages() {
        List<GoldPrice> prices2021 = await GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31));
        List<GoldPrice> prices2022 = await GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31));
        List<GoldPrice> prices2023 = await GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31));
        
        var avg2021QS = (from price in prices2021 select price.Price).Average();
        var avg2022QS = (from price in prices2022 select price.Price).Average();
        var avg2023QS = (from price in prices2023 select price.Price).Average();
        var avg2021MS = prices2021.Select(price => price.Price).Average();
        var avg2022MS = prices2023.Select(price => price.Price).Average();
        var avg2023MS = prices2023.Select(price => price.Price).Average();

        Console.WriteLine($"Query syntax averages: {avg2021QS}, {avg2022QS}, {avg2023QS}");
        Console.WriteLine($"Method syntax averages: {avg2021MS}, {avg2022MS}, {avg2023MS}");
    }

    public async void WhenIsTheBestTime() {
        List<GoldPrice> prices = new List<GoldPrice>();
        DateTime startDate = new DateTime(2019, 01, 01);
        DateTime endDate = startDate.AddYears(1);
        prices.AddRange(await GetGoldPrices(startDate, endDate));
        for (int i = 0; i < 4; i++) {
            startDate = endDate;
            endDate = startDate.AddYears(1);
            prices.AddRange(await GetGoldPrices(startDate, endDate));
        }

        GoldPrice buyQS = (from price in prices orderby price.Price ascending select price).ToList().First();
        GoldPrice sellQS = (from price in prices where price.Date > buyQS.Date orderby price.Price descending select price).ToList().First();
        Console.WriteLine($"When to buy: {buyQS.Date}, price: {buyQS.Price}");
        Console.WriteLine($"When to sell: {sellQS.Date}, price: {sellQS.Price}");
        Console.WriteLine($"Return on investment: {(sellQS.Price - buyQS.Price) / buyQS.Price * 100}");
    }

    public async void SavePricesToXML() {
        List<GoldPrice> prices = GetGoldPrices(new DateTime(2024, 03, 01), new DateTime(2024, 03, 11)).GetAwaiter().GetResult();
        XDocument doc = new XDocument();
        XElement root = new XElement("GoldPrices");
        foreach (var price in prices)
        {
            XElement priceElement = new XElement("GoldPrice",
                new XElement("Price", price.Price),
                new XElement("Date", price.Date.ToString("yyyy-MM-dd"))
            );
            root.Add(priceElement);
        }
        doc.Add(root);
        doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
        doc.Save(@"priceExport.xml");
    }

    public void ReadPricesFromXML() => Console.WriteLine(XDocument.Load("priceExport.xml").ToString());

}