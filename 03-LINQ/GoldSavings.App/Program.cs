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

        // Pobierz dane od 2019 do teraz, aby pokryć wszystkie lata wymagane w zadaniach
        DateTime startDate = new DateTime(2019, 1, 1);
        DateTime endDate = DateTime.Now;

        List<GoldPrice> goldPrices = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();

        if (goldPrices.Count == 0)
        {
            Console.WriteLine("No data found. Exiting.");
            return;
        }

        Console.WriteLine($"Retrieved {goldPrices.Count} records. Ready for analysis.");

        // Po pobraniu danych, a przed analizą wyświetl próbkę
        Console.WriteLine("Sample of retrieved data:");
        foreach (var price in goldPrices.Take(5))
        {
            Console.WriteLine($"  {price.Date.ToShortDateString()}: {price.Price:N2} PLN");
        }

        // Step 2: Perform analysis
        GoldAnalysisService analysisService = new GoldAnalysisService(goldPrices);

        try
        {
            // Obliczenie średniej ceny
            var avgPrice = analysisService.GetAveragePrice();
            Console.WriteLine($"Average Gold Price Last Half Year: {avgPrice:N2} PLN");

            Console.WriteLine("---------------------------------------------------");

            // a. TOP 3 highest and TOP 3 lowest prices of gold within the last year
            Console.WriteLine("a. TOP 3 highest and TOP 3 lowest prices of gold within the last year:");
            Console.WriteLine("Method Syntax:");
            var (highest, lowest) = analysisService.GetTopHighestAndLowestPrices_MethodSyntax();

            Console.WriteLine("TOP 3 Highest:");
            foreach (var price in highest)
            {
                Console.WriteLine($"  {price.Date.ToShortDateString()}: {price.Price:N2} PLN");
            }

            Console.WriteLine("TOP 3 Lowest:");
            foreach (var price in lowest)
            {
                Console.WriteLine($"  {price.Date.ToShortDateString()}: {price.Price:N2} PLN");
            }

            Console.WriteLine("\nQuery Syntax:");
            var (highestQuery, lowestQuery) = analysisService.GetTopHighestAndLowestPrices_QuerySyntax();

            Console.WriteLine("TOP 3 Highest:");
            foreach (var price in highestQuery)
            {
                Console.WriteLine($"  {price.Date.ToShortDateString()}: {price.Price:N2} PLN");
            }

            Console.WriteLine("TOP 3 Lowest:");
            foreach (var price in lowestQuery)
            {
                Console.WriteLine($"  {price.Date.ToShortDateString()}: {price.Price:N2} PLN");
            }

            Console.WriteLine("---------------------------------------------------");

            // b. Earning more than 5% if bought in January 2020
            Console.WriteLine("b. Days with more than 5% return if bought in January 2020:");
            var daysWithReturn = analysisService.GetDaysWithMoreThanFivePercentReturn();

            if (daysWithReturn.Any())
            {
                Console.WriteLine($"Found {daysWithReturn.Count} days with >5% return.");
                Console.WriteLine("First 5 days:");
                foreach (var day in daysWithReturn.Take(5))
                {
                    Console.WriteLine($"  {day.Date.ToShortDateString()}: {day.Price:N2} PLN");
                }

                if (daysWithReturn.Count > 5)
                {
                    Console.WriteLine($"  ... and {daysWithReturn.Count - 5} more days");
                }
            }
            else
            {
                Console.WriteLine("No days found with return greater than 5%");
            }

            Console.WriteLine("---------------------------------------------------");

            // c. Dates that open the second ten of the prices ranking
            Console.WriteLine("c. 3 dates that open the second ten of the prices ranking (2019-2022):");
            var secondTen = analysisService.GetSecondTenPricesRanking();

            foreach (var price in secondTen)
            {
                Console.WriteLine($"  {price.Date.ToShortDateString()}: {price.Price:N2} PLN");
            }

            Console.WriteLine("---------------------------------------------------");

            // d. Average prices by year
            Console.WriteLine("d. Average gold prices by year (Query Syntax):");
            var averagesByYear = analysisService.GetAveragePricesByYear_QuerySyntax();

            foreach (var yearAvg in averagesByYear.OrderBy(x => x.Key))
            {
                Console.WriteLine($"  {yearAvg.Key}: {yearAvg.Value:N2} PLN");
            }

            Console.WriteLine("---------------------------------------------------");

            // e. Best buy and sell days
            Console.WriteLine("e. Best buy and sell days between 2020 and 2024:");
            var (bestBuy, bestSell, roi) = analysisService.GetBestBuyAndSellDays();

            Console.WriteLine($"  Best day to buy: {bestBuy.Date.ToShortDateString()} at {bestBuy.Price:N2} PLN");
            Console.WriteLine($"  Best day to sell: {bestSell.Date.ToShortDateString()} at {bestSell.Price:N2} PLN");
            Console.WriteLine($"  Return on investment: {roi:P2}");

            Console.WriteLine("---------------------------------------------------");

            // 3. Save to XML file
            string xmlFilePath = "goldPrices.xml";
            Console.WriteLine($"3. Saving gold prices to XML file: {xmlFilePath}");
            analysisService.SavePricesToXmlFile(xmlFilePath);
            Console.WriteLine("  File saved successfully!");

            Console.WriteLine("---------------------------------------------------");

            // 4. Read from XML file
            Console.WriteLine($"4. Reading gold prices from XML file: {xmlFilePath}");
            var loadedPrices = analysisService.ReadPricesFromXmlFile(xmlFilePath);
            Console.WriteLine($"  Successfully read {loadedPrices.Count} records from file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred during analysis: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        Console.WriteLine("\nGold Analysis Queries with LINQ Completed.");
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}