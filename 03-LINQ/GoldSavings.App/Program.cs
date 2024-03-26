using GoldSavings.App.Model;
using GoldSavings.App.Client;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Saver!");

        GoldClient goldClient = new GoldClient();

        List<GoldPrice> goldPrices2019 = goldClient.GetGoldPrices(new DateTime(2019, 01, 01), new DateTime(2019, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2020 = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2021 = goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2022 = goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2023 = goldClient.GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> goldPrices2024 = goldClient.GetGoldPrices(new DateTime(2024, 01, 01), DateTime.Today).GetAwaiter().GetResult();

        List<GoldPrice> goldPrices = goldPrices2019.Concat(goldPrices2020).Concat(goldPrices2021).Concat(goldPrices2022).Concat(goldPrices2023).Concat(goldPrices2024).ToList();

        List<GoldPrice> thisMonthPrices = goldClient.GetGoldPrices(new DateTime(2024, 03, 01), new DateTime(2024, 03, 11)).GetAwaiter().GetResult();
        List<GoldPrice> lastYearPrices = goldClient.GetGoldPrices(DateTime.Now.AddYears(-1), DateTime.Now).GetAwaiter().GetResult();

        
        // TASK 3 //
        
        /*var lowest = goldClient.Get3Lowest(lastYearPrices, 0);
        Console.WriteLine("Lowest prices 1:");
        foreach(var goldPrice in lowest)
        {
            Console.WriteLine(goldPrice);
        }
        lowest = goldClient.Get3Lowest(lastYearPrices, 1);
        Console.WriteLine("Lowest prices 2:");
        foreach(var goldPrice in lowest)
        {
            Console.WriteLine(goldPrice);
        }

        var highest = goldClient.Get3Highest(thisMonthPrices, 0);
        Console.WriteLine("Highest prices 1:");
        foreach(var goldPrice in highest)
        {
            Console.WriteLine(goldPrice);
        }
        highest = goldClient.Get3Highest(thisMonthPrices, 1);
        Console.WriteLine("Highest prices 2:");
        foreach(var goldPrice in highest)
        {
            Console.WriteLine(goldPrice);
        }*/


        // TASK 4 //
        
        /*var profitDates = goldClient.GetProfitDates(goldPrices, 0);
        Console.WriteLine("Profitable dates for selling gold bought in January 2020:");
        foreach(var date in profitDates)
        {
            Console.WriteLine(date);
        }*/


        // TASK 5 //

        /*var dates = goldClient.Get3Dates(goldPrices, 0);
        Console.WriteLine("Dates:");
        foreach(var date in dates)
        {
            Console.WriteLine(date);
        }*/


        // TASK 6 //

        /*var avg = goldClient.GetAverage(goldPrices2021, 1);
        Console.WriteLine("Average in 2021: " + avg);
        avg = goldClient.GetAverage(goldPrices2022, 1);
        Console.WriteLine("Average in 2022: " + avg);
        avg = goldClient.GetAverage(goldPrices2023, 1);
        Console.WriteLine("Average in 2023: " + avg);*/


        // TASK 7 //

        //goldClient.GetInvestment(goldPrices, 0);


        // TASK 8 //

        //goldClient.SaveToXML(goldPrices2022, 0);

        
        // TASK 9 //

        //goldClient.ReadFromXML();
    }
}
