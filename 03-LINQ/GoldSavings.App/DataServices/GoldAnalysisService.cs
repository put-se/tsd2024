using System;
using System.Collections.Generic;
using System.Linq;
using GoldSavings.App.Model;

namespace GoldSavings.App.Services
{
    public class GoldAnalysisService
    {
        private readonly List<GoldPrice> _goldPrices;

        public GoldAnalysisService(List<GoldPrice> goldPrices)
        {
            _goldPrices = goldPrices;
        }
        public double GetAveragePrice()
        {
            return _goldPrices.Average(p => p.Price);
        }

        // a.
        public (List<GoldPrice> highest, List<GoldPrice> lowest) Get3TopPrices()
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);
            var lastYearPrices = _goldPrices
                .Where(p => p.Date >= oneYearAgo)
                .ToList();

            var top3Highest = lastYearPrices.OrderByDescending(p => p.Price).Take(3).ToList();
            var top3Lowest = lastYearPrices.OrderBy(p => p.Price).Take(3).ToList();

            return (top3Highest, top3Lowest);
        }
        public (List<GoldPrice> highest, List<GoldPrice> lowest) Get3TopPrices_Query()
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);
            var lastYearPrices = 
                (from price in _goldPrices
                where price.Date >= oneYearAgo
                select price).ToList();

            var top3Highest = (
                from price in lastYearPrices
                orderby price.Price descending
                select price
            ).Take(3).ToList();

            var top3Lowest = (
                from price in lastYearPrices
                orderby price.Price ascending
                select price
            ).Take(3).ToList();

            return (top3Highest, top3Lowest);
        }

        // b.
        public List<GoldPrice> Get5PercentDays()
        {            
            var januaryPrices = _goldPrices
                .Where(p => p.Date >= new DateTime(2020, 1, 1) && p.Date <= new DateTime(2020, 1, 31))
                .ToList();

            var targetDays = new List<GoldPrice>();
            foreach (var price in januaryPrices) {
                var days = _goldPrices
                    .Where(p => p.Price >= price.Price * 1.05)
                    .ToList();
                targetDays.AddRange(days);
            }

            return targetDays.Take(10).ToList();
        }

        // c.
        public List<GoldPrice> getSecondTenDates()
        {
            var dates = _goldPrices
                .Where(p => p.Date.Year >= 2019 && p.Date.Year <= 2022)
                .OrderByDescending(p => p.Price)
                .Skip(10).Take(3).ToList();

            return dates;
        }

        // d.
        public List<GoldPrice> GetYearsAvareges()
        {
            var years = new[] { 2020, 2023, 2024 };
            var averages = (
                from price in _goldPrices
                where years.Contains(price.Date.Year)
                group price by price.Date.Year into yearGroup
                select new GoldPrice
                {
                    Date = new DateTime(yearGroup.Key, 1, 1),
                    Price = yearGroup.Average(p => p.Price)
                }
            ).ToList();

            return (averages);
        }

        // e.
        public (GoldPrice buy, GoldPrice sell, double returnOfInvestment) GetBestTrade()
        {
            var filteredPrices = _goldPrices
                .Where(p => p.Date.Year >= 2020 && p.Date.Year <= 2024)
                .ToList();
            
            var bestBuyPrice = filteredPrices[0];
            var bestSellPrice = filteredPrices[1];
            var minPrice = bestBuyPrice;
            var maxProfit = 0.0;

            foreach (var price in filteredPrices) {
                if (price.Price < minPrice.Price)
                    minPrice = price;
                
                double profit = price.Price - minPrice.Price;
                if (profit > maxProfit) {
                    maxProfit = profit;
                    bestBuyPrice = minPrice;
                    bestSellPrice = price;
                }

            }
            double returnOfInvestment = (maxProfit / minPrice.Price) * 100;

            return (bestBuyPrice, bestSellPrice, returnOfInvestment);
        }

    }
}
