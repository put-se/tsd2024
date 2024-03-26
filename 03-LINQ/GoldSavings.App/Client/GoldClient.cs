using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Linq;
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

    public IEnumerable<double> Get3Lowest(List<GoldPrice> goldPrices, int syntax) 
    {
        IEnumerable<double> lowest;
        if (syntax == 0) {
            lowest = (from gp in goldPrices
                        orderby gp.Price
                        select gp.Price).Take(3);
        }
        else {
            lowest = goldPrices.OrderBy(gp => gp.Price)
                                .Select(gp => gp.Price)
                                .Take(3);
        }

        return lowest;
    }

    public IEnumerable<double> Get3Highest(List<GoldPrice> goldPrices, int syntax) 
    {
        IEnumerable<double> highest;
        if (syntax == 0) {
            highest = (from gp in goldPrices
                        orderby gp.Price descending 
                        select gp.Price).Take(3);
        }
        else {
            highest = goldPrices.OrderByDescending(gp => gp.Price)
                                .Select(gp => gp.Price)
                                .Take(3);
        }

        return highest;
    }

    public IEnumerable<DateTime> GetProfitDates(List<GoldPrice> goldPrices, int syntax) 
    {
        IEnumerable<DateTime> profitDates;
        if (syntax == 1) {
            profitDates = from jp in goldPrices
                            from unp in goldPrices
                            where jp.Date >= new DateTime(2020, 1, 1) && jp.Date <= new DateTime(2020, 1, 31)
                            && unp.Date >= jp.Date
                            && unp.Price/jp.Price >= 1.05
                            select unp.Date;
        }
        else {
            profitDates = goldPrices.Where(jp => jp.Date >= new DateTime(2020, 1, 1) && jp.Date <= new DateTime(2020, 1, 31))
                                    .SelectMany(jp => goldPrices.Where(unp => unp.Date >= jp.Date && unp.Price / jp.Price >= 1.05)
                                    .Select(unp => unp.Date));
        }

        return profitDates;
    }

    public IEnumerable<DateTime> Get3Dates(List<GoldPrice> goldPrices, int syntax) 
    {
        IEnumerable<DateTime> dates;
        if(syntax == 0) {
            dates = (from gp in goldPrices
                        orderby gp.Price descending
                        select gp.Date).Skip(10).Take(3);
        }
        else {
            dates = goldPrices.OrderByDescending(gp => gp.Price)
                                 .Select(gp => gp.Date)
                                 .Skip(10)
                                 .Take(3);
        }

        return dates;
    }

    public double GetAverage(List<GoldPrice> goldPrices, int syntax)
    {
        double avg;
        if(syntax == 0) {
            avg = (from gp in goldPrices select gp.Price).Average();
        }
        else {
            avg = goldPrices.Select(gp => gp.Price).Average();
        }

        return avg;
    }

    public void GetInvestment(List<GoldPrice> goldPrices, int syntax)
    {
        var bestTrade = (syntax == 0) ?
        goldPrices.Where(gp => gp.Date.Year >= 2019 && gp.Date.Year <= 2023)
                .SelectMany(buyPrice =>
                    goldPrices.Where(sellPrice => sellPrice.Date > buyPrice.Date)
                                .Select(sellPrice =>
                                new
                                {
                                    BuyDate = buyPrice.Date,
                                    SellDate = sellPrice.Date,
                                    Profit = sellPrice.Price/buyPrice.Price - 1
                                }))
                .OrderByDescending(trade => trade.Profit)
                .FirstOrDefault() :

        (from buyPrice in goldPrices
        where buyPrice.Date.Year >= 2019 && buyPrice.Date.Year <= 2023
        from sellPrice in goldPrices
        where sellPrice.Date > buyPrice.Date
        orderby (sellPrice.Price/buyPrice.Price - 1) descending
        select new
        {
            BuyDate = buyPrice.Date,
            SellDate = sellPrice.Date,
            Profit = sellPrice.Price/buyPrice.Price - 1
        }).FirstOrDefault();
        

        if (bestTrade != null)
        {
            Console.WriteLine($"Best time to buy: {bestTrade.BuyDate.ToShortDateString()}");
            Console.WriteLine($"Best time to sell: {bestTrade.SellDate.ToShortDateString()}");
            Console.WriteLine($"Maximum profit: {bestTrade.Profit}");
        }
        else
        {
            Console.WriteLine("No profitable trade found within the specified time frame.");
        }
    }

    public void SaveToXML(List<GoldPrice> goldPrices, int syntax)
    {
        var xml = syntax == 0 ?
        new XElement("GoldPrices", goldPrices.Select(gp => new XElement("goldPrice",
                                                new XAttribute("Date", gp.Date),
                                                new XAttribute("Price", gp.Price)))) :
        new XElement("GoldPrices",
            from gp in goldPrices
            select new XElement("goldPrice",
                new XAttribute("Date", gp.Date),
                new XAttribute("Price", gp.Price)));

        xml.Save("test.xml");
    }

    public void ReadFromXML() => Console.WriteLine(XDocument.Load("test.xml").ToString());
}