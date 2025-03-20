using GoldSavings.App.Model;
using GoldSavings.App.Services;
namespace GoldSavings.App;

using System.Globalization;
using System.Xml.Linq;

class Program
{

    static void savePricesToXml(List<GoldPrice> price, string filepath)
    {
        XDocument xmlDocument = new XDocument(
            new XComment("Gold Prices from NBP"),
            new XElement("goldPrices",
                price.Select(e => new XElement("goldPrice",
                    new XAttribute("date", e.Date),
                    new XAttribute("price", e.Price))
                )
            )
        );
        xmlDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
        xmlDocument.Save(filepath);
    }

    static List<GoldPrice> LoadPricesXml(string path)
    {
        return (
            from e in XElement.Load(path).Elements("goldPrice")
            let dateAttr = e.Attribute("date")
            let priceAttr = e.Attribute("price")
            where dateAttr != null && priceAttr != null
            select new GoldPrice
            {
                Date = DateTime.Parse(dateAttr.Value),
                Price = double.Parse(priceAttr.Value, CultureInfo.InvariantCulture)

            }
        ).ToList();
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Investor!");

        // Step 1: Get gold prices
        GoldDataService dataService = new GoldDataService();
        DateTime startDate = new DateTime(2020,01,01);
        DateTime endDate = new DateTime(2020,12,31);
        List<GoldPrice> goldPrices = dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();

        if (goldPrices.Count == 0)
        {
            Console.WriteLine("No data found. Exiting.");
            return;
        }

        Console.WriteLine($"Retrieved {goldPrices.Count} records. Ready for analysis.");

        // Step 2: Perform analysis
        GoldAnalysisService analysisService = new GoldAnalysisService(goldPrices);
        var avgPrice = analysisService.GetAveragePrice();

        // Step 3: Print results
        GoldResultPrinter.PrintSingleValue(Math.Round(avgPrice, 2), "Average Gold Price Last Half Year");

        Console.WriteLine("\nGold Analyis Queries with LINQ Completed.");

        // Step 4: Save prices to XML
        string filePath = "gold_prices2020.xml";
        savePricesToXml(goldPrices, filePath);
        Console.WriteLine("2020 Gold prices saved to XML file.");

        // Step 5: Load from XML
        List<GoldPrice> loadedPrices = LoadPricesXml(filePath);

        // Step 6: Print loaded prices to verify
        GoldResultPrinter.PrintPrices(loadedPrices, "Gold Prices Loaded from XML");

        // === WORK : Step2

        // a.
        var lastYear = DateTime.Now.Year - 1;
        DateTime startDate1 = new DateTime(lastYear,01,01);
        DateTime endDate1 = new DateTime(lastYear,12,01);
        List<GoldPrice> goldPrices1 = dataService.GetGoldPrices(startDate1, endDate1).GetAwaiter().GetResult();
        GoldAnalysisService analysisService1 = new GoldAnalysisService(goldPrices1);

        Console.WriteLine("\n\n2.a. With method syntax");
   
        var highestPriceLastYear = analysisService1.highestPricesMethod(3);
        var lowestPriceLastYear = analysisService1.lowestPricesMethod(3);

        GoldResultPrinter.PrintPrices(highestPriceLastYear, "Top 3 Highest price within last Year");
        GoldResultPrinter.PrintPrices(lowestPriceLastYear, "Top 3 Lowest price last year");

        Console.WriteLine("2.a. With query syntax");

        var highestPriceLastYearQ = analysisService1.highestPricesQuery(3);
        var lowestPriceLastYearQ = analysisService1.lowestPricesQuery(3);

        GoldResultPrinter.PrintPrices(highestPriceLastYearQ, "Top 3 Highest price within last Year");
        GoldResultPrinter.PrintPrices(lowestPriceLastYearQ, "Top 3 Lowest price last year");

        // b.
        DateTime startDate2 = new DateTime(2020,01,01);
        DateTime endDate2 = new DateTime(2020,12,01);
        List<GoldPrice> goldPrices2 = dataService.GetGoldPrices(startDate2, endDate2).GetAwaiter().GetResult();
        GoldAnalysisService analysisService2 = new GoldAnalysisService(goldPrices2);

        var lowestPriceInJanuary2020 = analysisService2.lowestPricesMethod(1)[0];
        var fivePercentMore = lowestPriceInJanuary2020.Price * 1.05;

        for (int year = 2020; year <= DateTime.Now.Year - 1; year++)
        {
            DateTime startDatetmp = new DateTime(year, 1, 1);
            DateTime endDatetmp = new DateTime(year, 12, 31);

            List<GoldPrice> goldPricestmp = dataService.GetGoldPrices(startDatetmp, endDatetmp).GetAwaiter().GetResult();
            GoldAnalysisService analysisServicetmp = new GoldAnalysisService(goldPricestmp);

            var goodPrices = analysisServicetmp.upperPrices(fivePercentMore);

            GoldResultPrinter.PrintPrices(goodPrices, "2.b. Days in ~" + year + "~ with 5% benefit when buying gold in January 2020");
        }

        // c.        
        List<GoldPrice> top12eachyear = new List<GoldPrice>();

        for (int year = 2019; year <= 2022; year++)
        {
            DateTime startDatetmp = new DateTime(year, 1, 1);
            DateTime endDatetmp = new DateTime(year, 12, 31);

            List<GoldPrice> goldPricestmp = dataService.GetGoldPrices(startDatetmp, endDatetmp).GetAwaiter().GetResult();
            GoldAnalysisService analysisServicetmp = new GoldAnalysisService(goldPricestmp);

            top12eachyear.AddRange(analysisServicetmp.highestPricesWithDateMethod(12));
        }

        GoldAnalysisService analysisService3 = new GoldAnalysisService(top12eachyear);
        var secondTenPrices = analysisService3.secondTenPrices();

        GoldResultPrinter.PrintPrices(secondTenPrices, "2.c. The 3 dates of 2022-2019 that open the second ten of the prices ranking");

        // d.        
        int[] years2 = { 2020, 2023, 2024 };

        foreach (var year in years2)
        {
            DateTime startDatetmp = new DateTime(year, 1, 1);
            DateTime endDatetmp = new DateTime(year, 12, 31);
            List<GoldPrice> goldPricestmp = dataService.GetGoldPrices(startDatetmp, endDatetmp).GetAwaiter().GetResult();
            GoldAnalysisService analysisServicetmp = new GoldAnalysisService(goldPricestmp);

            var averagePrice = analysisServicetmp.GetAveragePriceQuery();

            GoldResultPrinter.PrintSingleValue(Math.Round(averagePrice, 2), "2.d. Average gold price in " + year);
        }

        // e.
        List<GoldPrice> minEachYear = new List<GoldPrice>();

        for (int year = 2020; year <= 2024; year++)
        {
            DateTime startDatetmp = new DateTime(year, 1, 1);
            DateTime endDatetmp = new DateTime(year, 12, 31);

            List<GoldPrice> goldPricestmp = dataService.GetGoldPrices(startDatetmp, endDatetmp).GetAwaiter().GetResult();
            GoldAnalysisService analysisServicetmp = new GoldAnalysisService(goldPricestmp);

            minEachYear.AddRange(analysisServicetmp.lowestPricesWithDateMethod(1));
        }

        GoldAnalysisService analysisService4 = new GoldAnalysisService(minEachYear);
        GoldPrice minGlobal = analysisService4.lowestPricesWithDateMethod(1)[0];    

        List<GoldPrice> maxEachYear = new List<GoldPrice>();

        for (int year = minGlobal.Date.Year; year <= 2024; year++)
        {
            DateTime startDatetmp;
            DateTime endDatetmp;

            if (year == minGlobal.Date.Year) {
                startDatetmp = minGlobal.Date.AddDays(1);
                endDatetmp = new DateTime(year, 12, 31);
            } else {
                startDatetmp = new DateTime(year, 1, 1);
                endDatetmp = new DateTime(year, 12, 31);                
            }

            List<GoldPrice> goldPricestmp = dataService.GetGoldPrices(startDatetmp, endDatetmp).GetAwaiter().GetResult();
            GoldAnalysisService analysisServicetmp = new GoldAnalysisService(goldPricestmp);

            maxEachYear.AddRange(analysisServicetmp.highestPricesWithDateMethod(1));
        }
        
        GoldAnalysisService analysisService5 = new GoldAnalysisService(maxEachYear);
        GoldPrice maxAfterMinGlobal = analysisService5.highestPricesWithDateMethod(1)[0];  
    
        var buyPrice = minGlobal.Price;
        var sellPrice = maxAfterMinGlobal.Price;
        var roi = (sellPrice - buyPrice) / buyPrice * 100;

        GoldResultPrinter.PrintSingleValue(minGlobal.Date, "2.e. Best time to buy");
        GoldResultPrinter.PrintSingleValue(maxAfterMinGlobal.Date, "2.e. Best time to sell");
        GoldResultPrinter.PrintSingleValue(Math.Round(roi, 2), "2.e. Return on investment (percentage)");
    }
}
