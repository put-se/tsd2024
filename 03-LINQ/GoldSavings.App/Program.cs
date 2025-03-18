using GoldSavings.App.Model;
using GoldSavings.App.Services;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Investor!");

        // Step 1: Get gold prices
        GoldDataService dataService = new GoldDataService();
        List<GoldPrice> goldPrices = new List<GoldPrice>();
        
        for (int year = 2020; year <= DateTime.Now.Year; year++)
        {
            var startDate = new DateTime(year, 01, 01);
            var endDate = new DateTime(year, 12, 31);
            goldPrices.AddRange(dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult());
        }
        
        if (goldPrices.Count == 0)
        {
            Console.WriteLine("No data found. Exiting.");
            return;
        }

        Console.WriteLine($"Retrieved {goldPrices.Count} records. Ready for analysis.");

        // Step 2: Perform analysis
        GoldAnalysisService analysisService = new GoldAnalysisService(goldPrices);
        var top3Prices = analysisService.Top3GoldPrices();
        var bottom3Prices = analysisService.Bottom3GoldPrices();
        var profitOver5Percent = analysisService.EarnedMoreThan(new DateTime(2020, 01, 01), 5, new DateTime(2020, 02, 10));
        var secondTenPrices = analysisService.GetSecondTen(new DateTime(2019, 01, 01), new DateTime(2022, 12, 31));
        var years = new List<int>()
        {
            2020, 2023, 2024
        };
        var avgPrices = analysisService.GetAveragePrice(years);
        var bestToBuyAndSell = analysisService.BestToBuyAndSell(new DateTime(2020, 01, 01), new DateTime(2024, 12, 31));

        // Step 3: Print results
        GoldResultPrinter.PrintPrices(top3Prices, "Top 3 Gold Prices");
        GoldResultPrinter.PrintPrices(bottom3Prices, "Bottom 3 Gold Prices");
        GoldResultPrinter.PrintPrices(profitOver5Percent, "Gold Prices with 5% Profit");
        GoldResultPrinter.PrintPrices(secondTenPrices, "Second Ten Gold Prices");
        GoldResultPrinter.PrintPrices(avgPrices, "Average Gold Prices");
        GoldResultPrinter.PrintPrices(bestToBuyAndSell, "Best Time to Buy and Sell Gold");
        
        dataService.SaveToXML(goldPrices, "GoldPrices.xml");
        var loadedPrices = dataService.LoadFromXML("GoldPrices.xml");
        Console.WriteLine($"Retrived prices from XML file: {loadedPrices.Count}");
        
        Console.WriteLine("\nGold Analyis Queries with LINQ Completed.");
    }
}
