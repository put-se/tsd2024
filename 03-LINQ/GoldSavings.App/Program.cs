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
        /*foreach(var goldPrice in thisMonthPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }*/

        /*var lowest = goldClient.GetLowest1(thisMonthPrices);
        Console.WriteLine("Lowest prices 1:");
        foreach(var goldPrice in lowest)
        {
            Console.WriteLine(goldPrice);
        }
        lowest = goldClient.GetLowest2(thisMonthPrices);
        Console.WriteLine("Lowest prices 2:");
        foreach(var goldPrice in lowest)
        {
            Console.WriteLine(goldPrice);
        }

        var highest = goldClient.GetHighest1(thisMonthPrices);
        Console.WriteLine("Highest prices 1:");
        foreach(var goldPrice in highest)
        {
            Console.WriteLine(goldPrice);
        }
        highest = goldClient.GetHighest2(thisMonthPrices);
        Console.WriteLine("Highest prices 2:");
        foreach(var goldPrice in highest)
        {
            Console.WriteLine(goldPrice);
        }*/

        List<GoldPrice> goldPrices2019 = goldClient.GetGoldPrices(new DateTime(2019, 01, 01), new DateTime(2019, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2020 = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2021 = goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2022 = goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2023 = goldClient.GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2024 = goldClient.GetGoldPrices(new DateTime(2024, 01, 01), DateTime.Today).GetAwaiter().GetResult();

        List<GoldPrice> goldPrices = goldPrices2020.Concat(goldPrices2021).Concat(goldPrices2022).Concat(goldPrices2023).Concat(goldPrices2024).ToList();

        /*var profitDates = goldClient.GetProfitDates(goldPrices);
        Console.WriteLine("Profitable dates:");
        foreach(var date in profitDates)
        {
            Console.WriteLine(date);
        }*/

        /*var dates = goldClient.Get3Dates(goldPrices);
        Console.WriteLine("Dates:");
        foreach(var date in dates)
        {
            Console.WriteLine(date);
        }*/

        /*var avg = goldClient.GetAverage(goldPrices2021);
        Console.WriteLine("Average in 2021: " + avg);
        avg = goldClient.GetAverage(goldPrices2022);
        Console.WriteLine("Average in 2022: " + avg);
        avg = goldClient.GetAverage(goldPrices2023);
        Console.WriteLine("Average in 2023: " + avg);*/

        var dates = goldClient.GetInvestment(goldPrices);
        Console.WriteLine("Best investment:");
        Console.WriteLine("Buy on ");
    
}
