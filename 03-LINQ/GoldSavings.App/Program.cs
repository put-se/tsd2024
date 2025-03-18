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
        DateTime endDate = DateTime.Now;
        List<GoldPrice> qa = dataService.GetGoldPrices(new DateTime(2024,01,01),new DateTime(2025,01,01)).GetAwaiter().GetResult();
        List<GoldPrice> qb = dataService.GetGoldPrices(new DateTime(2024,01,01),new DateTime(2025,01,01)).GetAwaiter().GetResult();
        List<GoldPrice> qc = dataService.GetGoldPrices(new DateTime(2020,01,01), DateTime.Now).GetAwaiter().GetResult();
        List<GoldPrice> qd = dataService.GetGoldPrices(new DateTime(2019,01,01),new DateTime(2023,01,01)).GetAwaiter().GetResult();
        List<GoldPrice> qe = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();
        List<GoldPrice> qf = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();

        if (qa.Count == 0)
        {
            Console.WriteLine("No data found. Exiting.");
            return;
        }
        
        Console.WriteLine($"Retrieved {qa.Count} records. Ready for analysis.");

        // Step 2: Perform analysis
        GoldAnalysisService analysisServiceQA = new GoldAnalysisService(qa);
        GoldAnalysisService analysisServiceQB = new GoldAnalysisService(qb);
        GoldAnalysisService analysisServiceQC = new GoldAnalysisService(qc);
        GoldAnalysisService analysisServiceQD = new GoldAnalysisService(qd);
        GoldAnalysisService analysisServiceQE = new GoldAnalysisService(qe);
        GoldAnalysisService analysisServiceQF = new GoldAnalysisService(qf);
        
        var top3 = analysisServiceQA.Top(3);
        var bot3 = analysisServiceQA.Bottom(3);
        var dates = analysisServiceQB.BestEarned(new DateTime(2024,02,01),0.05);
        var secondten = analysisServiceQC.SecondTen();
        
        // Step 3: Print results

        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Top 3: {top3[i]}");
        }
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Bot 3: {bot3[i]}");
        }
        for (int i = 0; i < dates.Count; i++)
        {
            Console.WriteLine($"Date 3: {dates[i]}");
        }
        for (int i = 0; i < secondten.Length; i++)
        {
            Console.WriteLine($"PriceSecondTen 3: {secondten[i]}");
        }

        //GoldResultPrinter.PrintSingleValue(Math.Round(avgPrice, 2), "Average Gold Price Last Half Year");

        Console.WriteLine("\nGold Analyis Queries with LINQ Completed.");

    }
}
