﻿using GoldSavings.App.Model;
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
        foreach(var goldPrice in thisMonthPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }

        List<GoldPrice> lastYearPrices = goldClient.GetGoldPrices(DateTime.Now.AddYears(-1), DateTime.Now).GetAwaiter().GetResult();

        // Method syntax to find top 3 highest prices
        var top3HighestPricesMethodSyntax = lastYearPrices.OrderByDescending(p => p.Price).Take(3);
        Console.WriteLine("Top 3 highest prices using method syntax:");
        foreach (var price in top3HighestPricesMethodSyntax)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }

        // Query syntax to find top 3 highest prices
        var top3HighestPricesQuerySyntax = (from p in lastYearPrices
                                            orderby p.Price descending
                                            select p).Take(3);
        Console.WriteLine("\nTop 3 highest prices using query syntax:");
        foreach (var price in top3HighestPricesQuerySyntax)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }

        // Method syntax to find top 3 lowest prices
        var top3LowestPricesMethodSyntax = lastYearPrices.OrderBy(p => p.Price).Take(3);
        Console.WriteLine("\nTop 3 lowest prices using method syntax:");
        foreach (var price in top3LowestPricesMethodSyntax)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }

        // Query syntax to find top 3 lowest prices
        var top3LowestPricesQuerySyntax = (from p in lastYearPrices
                                           orderby p.Price
                                           select p).Take(3);
        Console.WriteLine("\nTop 3 lowest prices using query syntax:");
        foreach (var price in top3LowestPricesQuerySyntax)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }

        // Fetch gold prices for January 2020
        DateTime january2020Start = new DateTime(2020, 1, 1);
        DateTime january2020End = new DateTime(2020, 1, 31);
        List<GoldPrice> january2020Prices = goldClient.GetGoldPrices(january2020Start, january2020End).GetAwaiter().GetResult();

        // Calculate the percentage increase for each day compared to the price at the beginning of the month
        double initialPrice = january2020Prices.First().Price;
        var percentageIncreases = january2020Prices.Select(price =>
            new
            {
                Date = price.Date,
                PercentageIncrease = (price.Price - initialPrice) / initialPrice * 100
            });

        // Method syntax to find days with more than 5% increase
        var daysWith5PercentIncreaseMethodSyntax = percentageIncreases.Where(p => p.PercentageIncrease > 5);
        Console.WriteLine("Days with more than 5% increase using method syntax:");
        foreach (var day in daysWith5PercentIncreaseMethodSyntax)
        {
            Console.WriteLine($"Date: {day.Date}, Percentage Increase: {day.PercentageIncrease}%");
        }

        // Query syntax to find days with more than 5% increase
        var daysWith5PercentIncreaseQuerySyntax = from p in percentageIncreases
                                                  where p.PercentageIncrease > 5
                                                  select p;
        Console.WriteLine("\nDays with more than 5% increase using query syntax:");
        foreach (var day in daysWith5PercentIncreaseQuerySyntax)
        {
            Console.WriteLine($"Date: {day.Date}, Percentage Increase: {day.PercentageIncrease}%");
        }

        // Get data for the last 93 days (maximum allowed by the app)
        DateTime endDate = new DateTime(2022, 12, 31);
        DateTime startDate = endDate.AddDays(-93); // 93 days ago

        // Initialize lists to store prices for each year
        List<GoldPrice> goldPrices= new List<GoldPrice>();

        // Fetch gold prices for each year from 2019 to 2022
        while(startDate.Year > 2018)
        {
            DateTime yearStart = startDate;
            DateTime yearEnd = endDate;

            // Get gold prices for the year
            List<GoldPrice> prices = goldClient.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult();
            goldPrices.AddRange(prices);
            
            endDate = yearStart;
            startDate = yearStart.AddDays(-93);

            // Filter prices to ensure they are within the last 93 days
            prices = prices.Where(p => p.Date >= startDate && p.Date <= endDate).ToList();

        }

        goldPrices = goldPrices.Where(p => p.Date >= new DateTime(2019, 1, 1) && p.Date <= new DateTime(2022, 12, 31)).OrderByDescending(p => p.Price).Skip(10).Take(3).ToList();
        Console.WriteLine("Dates corresponding to the second ten of prices ranking:");
        foreach (var price in goldPrices)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }
        // query syntax
        DateTime endDateQuerySyntax = new DateTime(2022, 12, 31);
        DateTime startDateQuerySyntax = endDateQuerySyntax.AddDays(-93); // 93 days ago

        List<GoldPrice> goldPricesQuerySyntax = new List<GoldPrice>();

        while (startDateQuerySyntax.Year > 2018)
        {
            DateTime yearStartQuerySyntax = startDateQuerySyntax;
            DateTime yearEndQuerySyntax = endDateQuerySyntax;

            // Get gold prices for the year
            List<GoldPrice> pricesQuerySyntax = goldClient.GetGoldPrices(yearStartQuerySyntax, yearEndQuerySyntax).GetAwaiter().GetResult();
            goldPricesQuerySyntax.AddRange(pricesQuerySyntax);

            endDateQuerySyntax = yearStartQuerySyntax; // Update endDateQuerySyntax
            startDateQuerySyntax = yearStartQuerySyntax.AddDays(-93); // Update startDateQuerySyntax
        }

        // Filter and sort prices
        goldPricesQuerySyntax = goldPricesQuerySyntax
            .Where(p => p.Date >= new DateTime(2019, 1, 1) && p.Date <= new DateTime(2022, 12, 31))
            .OrderByDescending(p => p.Price)
            .Skip(10)
            .Take(3)
            .ToList();

        Console.WriteLine("Dates corresponding to the second ten of prices ranking (query syntax):");
        foreach (var priceQuery in goldPricesQuerySyntax)
        {
            Console.WriteLine($"Date: {priceQuery.Date}, Price: {priceQuery.Price}");
        }


    }



}