using System.Xml.Linq;
using GoldSavings.App.Model;
using GoldSavings.App.Client;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Saver!");

        GoldClient goldClient = new GoldClient();

        GoldPrice currentPrice = goldClient.GetCurrentGoldPrice().GetAwaiter().GetResult();
        Console.WriteLine($"The price for today is {currentPrice.Price}");

        List<GoldPrice> thisMonthPrices = goldClient.GetGoldPrices(new DateTime(2024, 03, 01), new DateTime(2024, 03, 11)).GetAwaiter().GetResult();
        foreach(var goldPrice in thisMonthPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }
        
        var orderedPrices = goldClient.GetGoldPrices(new DateTime(2024, 01, 01), new DateTime(2024, 12, 31)).GetAwaiter().GetResult()
            .OrderBy(price => price.Price);

        var last3 = orderedPrices.TakeLast(3).ToList();
        foreach (var goldPrice in last3)
        {
            Console.WriteLine($"The worst prices are {goldPrice.Price} at date: {goldPrice.Date}");
        }

        var top3 = orderedPrices.Take(3).ToList();
        foreach (var goldPrice in top3)
        {
            Console.WriteLine($"The best prices are {goldPrice.Price} at date: {goldPrice.Date}");
        }

        var avgJanuary2020GoldPrice = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 01, 31))
            .GetAwaiter().GetResult().Average(price => price.Price) * 1.05;
        Console.WriteLine(avgJanuary2020GoldPrice);
        
        var goldPricesForBeating2020 = goldClient.GetGoldPrices(new DateTime(2024, 01, 01), new DateTime(2024, 12, 31)).GetAwaiter().GetResult()
            .Where(price => price.Price >= avgJanuary2020GoldPrice);
        foreach (var goldPrice in goldPricesForBeating2020)
        {
            Console.WriteLine($"The prices and dates that you would gain 5 percent if you bought in January 2020: {goldPrice.Date} is {goldPrice.Price}");
        }
        
        List<GoldPrice> prices2019 = goldClient.GetGoldPrices(new DateTime(2019, 01, 01), new DateTime(2019, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> prices2020 = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> prices2021 = goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> prices2022 = goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31)).GetAwaiter().GetResult();

        var worstSecondTenthThreePrices = prices2019.Concat(prices2020).Concat(prices2021).Concat(prices2022).OrderBy(price => price.Price).Take(13).TakeLast(3).ToList();
        Console.WriteLine("Worst prices for 2019-2022");
        foreach(var goldPrice in worstSecondTenthThreePrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }
        
        List<GoldPrice> prices2023 = goldClient.GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> prices2024 = goldClient.GetGoldPrices(new DateTime(2024, 01, 01), new DateTime(2024, 12, 31)).GetAwaiter().GetResult();

        Console.WriteLine($"Average prices for 2020, 2023, 2024: {prices2020.Average(price => price.Price)}, {prices2023.Average(price => price.Price)}, {prices2024.Average(price => price.Price)}");

        var prices20202024 = prices2020.Concat(prices2021)
            .Concat(prices2022)
            .Concat(prices2023)
            .Concat(prices2024).OrderBy(price => price.Price).ToList();
        var bestDayToBuy = prices20202024.First();
        var bestDayToSell = prices20202024.Last();
        Console.WriteLine($"Best day to buy Price and Date: {bestDayToBuy.Price} {bestDayToBuy.Date}, Best day to sell Price and Date: {bestDayToSell.Price} {bestDayToSell.Date}");
        
        Console.WriteLine("Writing prices to XML");
        SaveToXml(prices2024);
        
        Console.WriteLine("Reading prices from XML");
        var goldPricesFromXml = LoadFromXml("GoldPrices.xml");
        foreach(var goldPrice in goldPricesFromXml)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }
    }

    public static void SaveToXml(List<GoldPrice> goldPrices)
    {
        XDocument objXDoc = new XDocument(
            new XElement("GoldPrices",
                goldPrices.Select(price =>
                    new XElement("GoldPrice",
                        new XElement("Date", price.Date.ToString("yyyy-MM-dd")),
                        new XElement("Price", price.Price)
                    )
                )
            )
        );

        objXDoc.Save("GoldPrices.xml");
    }

    public static List<GoldPrice> LoadFromXml(string filePath)
    {
        return XDocument.Load(filePath).Descendants("GoldPrice").Select(x => new GoldPrice
        {
            Date = DateTime.Parse(x.Element("Date").Value),
            Price = double.Parse(x.Element("Price").Value)
        }).ToList();
    }
}
