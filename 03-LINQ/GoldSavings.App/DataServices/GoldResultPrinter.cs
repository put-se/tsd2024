using GoldSavings.App.Model;
using System;
using System.Collections.Generic;

namespace GoldSavings.App.Services
{
    public static class GoldResultPrinter
    {
        public static void PrintSingleValue(decimal value, string label)
        {
            Console.WriteLine($"{label}: {value:N2} PLN");
        }

        public static void PrintGoldPrices(List<GoldPrice> prices, string header)
        {
            Console.WriteLine(header);
            foreach (var price in prices)
            {
                Console.WriteLine($"  {price.Date.ToShortDateString()}: {price.Price:N2} PLN");
            }
        }

        public static void PrintYearlyAverages(Dictionary<int, decimal> yearlyAverages)
        {
            Console.WriteLine("Yearly Averages:");
            foreach (var entry in yearlyAverages.OrderBy(e => e.Key))
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value:N2} PLN");
            }
        }
    }
}