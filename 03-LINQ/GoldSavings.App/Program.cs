using GoldSavings.App.Model;
using GoldSavings.App.Client;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Saver!");

        GoldClient goldClient = new GoldClient();

        // GoldPrice currentPrice = goldClient.GetCurrentGoldPrice().GetAwaiter().GetResult();
        // Console.WriteLine($"The price for today is {currentPrice.Price}");

        // List<GoldPrice> thisMonthPrices = goldClient.GetGoldPrices(new DateTime(2024, 03, 01), new DateTime(2024, 03, 11)).GetAwaiter().GetResult();
        // foreach(var goldPrice in thisMonthPrices)
        // {
        //     Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        // }
        
        // TASK 3
        // Console.WriteLine($"\nTOP 3 highest prices of gold within last year are:");
        // goldClient.GetTop3HighestPrices();
        // Console.WriteLine($"\nTOP 3 lowest prices of gold within last year are:");
        // goldClient.GetTop3LowestPrices();

        // TASK 4
        //Console.WriteLine($"\nIf one had bought gold in January 2020, is it possible that they would have earned more than 5%? On which days?");
        //goldClient.WouldEearnMoreThan5();

        // TASK 5
        // Console.WriteLine($"\nWhich 3 dates of 2022-2019 opens the second ten of the prices ranking?");
        // goldClient.GetTop3ForSecondTen();

        // TASK 6
        // Console.WriteLine($"\nWhat are the averages of gold prices in 2021, 2022, 2023?");
        // goldClient.GetAverages();

        // TASK 7
        // Console.WriteLine($"When it would be best to buy gold and sell it between 2019 and 2023? What would be the return on investment?");
        // goldClient.WhenIsTheBestTime();

        // TASK 8
        goldClient.SavePricesToXML();

        // TASK 9
        goldClient.ReadPricesFromXML();
    }   


}
