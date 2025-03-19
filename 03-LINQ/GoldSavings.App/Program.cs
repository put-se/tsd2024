using System.Xml.Linq;
using GoldSavings.App.Model;
using GoldSavings.App.Services;

namespace GoldSavings.App;

internal class Program
{
    private static void SaveToXml(string path, List<GoldPrice> price)
    {
        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("GoldPrices",
                price.Select(p => new XElement("entry",
                    new XAttribute("date", p.Date),
                    new XAttribute("price", p.Price))
                )
            )
        );

        doc.Save(path);
    }

    private static List<GoldPrice> LoadXml(string path)
    {
        return (
            from entry in XElement.Load(path).Elements("entry")
            select new GoldPrice
            {
                Date = DateTime.Parse(entry.Attribute("date").Value),
                Price = double.Parse(entry.Attribute("price").Value)
            }
        ).ToList();
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Investor!");

        // Step 1: Get gold prices
        var dataService = new GoldDataService();
        var startDate = new DateTime(2019, 01, 01);
        var endDate = DateTime.Now;
        List<GoldPrice> goldPrices2 = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();

        SaveToXml("/Users/tomchauvel/Documents/GitHub/tsd2024/03-LINQ/GoldSavings.App.xml", goldPrices2);

        var goldPrices = LoadXml("/Users/tomchauvel/Documents/GitHub/tsd2024/03-LINQ/GoldSavings.App.xml");

        if (goldPrices.Count == 0)
        {
            Console.WriteLine("No data found. Exiting.");
            return;
        }

        Console.WriteLine($"Retrieved {goldPrices.Count} records. Ready for analysis.");

        // Step 2: Perform analysis
        var analysisService = new GoldAnalysisService(goldPrices);
        var avgPrice2020 = analysisService.GetAveragePrice(new DateTime(2020, 01, 01), new DateTime(2021, 01, 01));
        var avgPrice2023 = analysisService.GetAveragePrice(new DateTime(2023, 01, 01), new DateTime(2024, 01, 01));
        var avgPrice2024 = analysisService.GetAveragePrice(new DateTime(2024, 01, 01), new DateTime(2025, 01, 01));

        var top3 = analysisService.Top(new DateTime(2024, 01, 01), new DateTime(2025, 01, 01), 3);
        var bot3 = analysisService.Bottom(new DateTime(2024, 01, 01), new DateTime(2025, 01, 01), 3);
        var dates = analysisService.BestEarned(new DateTime(2020, 01, 01), endDate, new DateTime(2020, 02, 01), 0.05);
        var secondten = analysisService.SecondTen(new DateTime(2019, 01, 01), new DateTime(2023, 01, 01), 3);

        // Step 3: Print results

        GoldResultPrinter.PrintPrices(top3.ToList(), "Top 3");
        GoldResultPrinter.PrintPrices(bot3.ToList(), "Bot 3");
        GoldResultPrinter.PrintPrices(secondten.ToList(), "PriceSecondTen");

        GoldResultPrinter.PrintSingleValue(Math.Round(avgPrice2020, 2), "Average Gold Price 2020");
        GoldResultPrinter.PrintSingleValue(Math.Round(avgPrice2023, 2), "Average Gold Price 2023");
        GoldResultPrinter.PrintSingleValue(Math.Round(avgPrice2024, 2), "Average Gold Price 2024");

        var top = analysisService.Top(new DateTime(2020, 01, 01), new DateTime(2025, 01, 01), 1);
        var bot = analysisService.Bottom(new DateTime(2019, 01, 01), new DateTime(2025, 01, 01), 1);

        GoldResultPrinter.PrintPrices(top.ToList(), "Best price");
        GoldResultPrinter.PrintPrices(bot.ToList(), "Lowest price");
        GoldResultPrinter.PrintSingleValue((top[0].Price / bot[0].Price - 1) * 100, "Return of investment in %");

        Console.WriteLine("\nGold Analyis Queries with LINQ Completed.");
    }
}