using GoldSavings.App.Model;
using GoldSavings.App.Client;
using GoldSavings.App.Services;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Investor!");

        // Step 1: Get gold prices
        GoldDataService dataService = new GoldDataService();
        DateTime startDate = new DateTime(2019,01,01);
        DateTime endDate = new DateTime(2020,01,01);
        List<GoldPrice> goldPrices = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();

        startDate = new DateTime(2020,01,02);
        endDate = new DateTime(2021,01,01);
        List<GoldPrice> goldPrices1 = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();
        goldPrices.AddRange(goldPrices1);

        startDate = new DateTime(2021,01,02);
        endDate = new DateTime(2022,01,01);
        List<GoldPrice> goldPrices2 = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();
        goldPrices.AddRange(goldPrices2);

        startDate = new DateTime(2022,01,02);
        endDate = new DateTime(2023,01,01);
        List<GoldPrice> goldPrices3 = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();
        goldPrices.AddRange(goldPrices3);

        startDate = new DateTime(2023,01,02);
        endDate = new DateTime(2024,01,01);
        List<GoldPrice> goldPrices4 = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();
        goldPrices.AddRange(goldPrices4);

        startDate = new DateTime(2024,01,02);
        endDate = new DateTime(2025,01,01);
        List<GoldPrice> goldPrices5 = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();
        goldPrices.AddRange(goldPrices5);

        startDate = new DateTime(2025,01,02);
        endDate = DateTime.Now;
        List<GoldPrice> goldPrices6 = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();
        goldPrices.AddRange(goldPrices6);

        if (goldPrices.Count == 0)
        {
            Console.WriteLine("No data found. Exiting.");
            return;
        }

        Console.WriteLine($"Retrieved {goldPrices.Count} records. Ready for analysis.");
        dataService.GoldPricesToXml(goldPrices, "prices.xml");
        var newGoldPrices = dataService.GoldPricesFromXml("prices.xml");

        // Step 2: Perform analysis
        GoldAnalysisService analysisService = new GoldAnalysisService(goldPrices);
        var avgPrice = analysisService.GetAveragePrice();
        // a.
        var (top3Highest, top3Lowest) = analysisService.Get3TopPrices();
        var (top3Highest_query, top3Lowest_query) = analysisService.Get3TopPrices_Query();

        // b.
        var days5Percent = analysisService.Get5PercentDays();

        // c.
        var secondTenDates = analysisService.getSecondTenDates();

        // d.
        var yearsAverages = analysisService.GetYearsAvareges();

        // e.
        var (dayToBuy, dayToSell, returnOfInvestment) = analysisService.GetBestTrade();

        // Step 3: Print results
        GoldResultPrinter.PrintSingleValue(Math.Round(avgPrice, 2), "Average Gold Price Last Half Year");
        
        // a.
        GoldResultPrinter.PrintPrices(top3Highest, "Top 3 Highest Prices:");
        GoldResultPrinter.PrintPrices(top3Lowest, "Top 3 Lowest Prices:");

        GoldResultPrinter.PrintPrices(top3Highest_query, "Top 3 Highest Prices (query):");
        GoldResultPrinter.PrintPrices(top3Lowest_query, "Top 3 Lowest Prices (query):");

        // b.
        GoldResultPrinter.PrintPrices(days5Percent, "Days with more than 5% earned:");

        // c.
        GoldResultPrinter.PrintPrices(secondTenDates, "3 days from the second ten of ranking:");

        // d.
        GoldResultPrinter.PrintPrices(yearsAverages, "Yearly averages in 2020, 2023, 2024:");

        // e.
        Console.WriteLine("\n--- Best gold trade 2020-2024 ---");
        GoldResultPrinter.PrintSingleValue($"{dayToBuy.Date:yyyy-MM-dd}", "Best buying date: ");
        GoldResultPrinter.PrintSingleValue($"{dayToSell.Date:yyyy-MM-dd}", "Best selling date: ");
        GoldResultPrinter.PrintSingleValue($"{Math.Round(returnOfInvestment, 2)} %", "Return of investment: ");

        Console.WriteLine("\nGold Analyis Queries with LINQ Completed.");

    }
}
