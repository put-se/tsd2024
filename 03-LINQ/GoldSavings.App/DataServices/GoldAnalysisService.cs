using GoldSavings.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace GoldSavings.App.Services
{
    public class GoldAnalysisService
    {
        private readonly List<GoldPrice> _goldPrices;

        public GoldAnalysisService(List<GoldPrice> goldPrices)
        {
            _goldPrices = goldPrices ?? throw new ArgumentNullException(nameof(goldPrices));
        }

        public decimal GetAveragePrice()
        {
            return _goldPrices.Average(p => p.Price);
        }

        // a. (method syntax) What are the TOP 3 highest and TOP 3 lowest prices of gold within the last year?
        public (List<GoldPrice> Highest, List<GoldPrice> Lowest) GetTopHighestAndLowestPrices_MethodSyntax(int count = 3)
        {
            // Zamiast przefiltrować po datach tylko ostatni rok, bierzemy ostatnie 12 rekordów
            // albo wszystkie, jeśli jest ich mniej niż 12
            var recentPrices = _goldPrices
                .OrderByDescending(p => p.Date)
                .Take(Math.Min(12, _goldPrices.Count))
                .ToList();

            var highest = recentPrices
                .OrderByDescending(p => p.Price)
                .Take(count)
                .ToList();

            var lowest = recentPrices
                .OrderBy(p => p.Price)
                .Take(count)
                .ToList();

            return (highest, lowest);
        }

        // a. (query syntax) What are the TOP 3 highest and TOP 3 lowest prices of gold within the last year?
        public (List<GoldPrice> Highest, List<GoldPrice> Lowest) GetTopHighestAndLowestPrices_QuerySyntax(int count = 3)
        {
            // Zamiast przefiltrować po datach tylko ostatni rok, bierzemy ostatnie 12 rekordów
            var recentPricesQuery = (from p in _goldPrices
                                     orderby p.Date descending
                                     select p).Take(Math.Min(12, _goldPrices.Count)).ToList();

            var highest = (from p in recentPricesQuery
                           orderby p.Price descending
                           select p).Take(count).ToList();

            var lowest = (from p in recentPricesQuery
                          orderby p.Price ascending
                          select p).Take(count).ToList();

            return (highest, lowest);
        }

        // b. If one had bought gold in January 2020, is it possible that they would have earned more than 5%? On which days?
        public List<GoldPrice> GetDaysWithMoreThanFivePercentReturn()
        {
            // Get January 2020 prices
            var jan2020Prices = _goldPrices
                .Where(p => p.Date.Year == 2020 && p.Date.Month == 1)
                .ToList();

            if (!jan2020Prices.Any())
                return new List<GoldPrice>();

            // Calculate minimum price in January 2020 (buy price)
            var buyPrice = jan2020Prices.Min(p => p.Price);

            // Find the last day of January 2020 (or the last available day in that month)
            var lastJan2020Day = jan2020Prices.Max(p => p.Date);

            // Find all days AFTER January 2020 with 5% or more return compared to the buy price
            var daysWithReturn = _goldPrices
                .Where(p => p.Date > lastJan2020Day) // Tylko dni PO styczniu 2020
                .Where(p => (p.Price / buyPrice) >= 1.05m) // 5% or more return
                .OrderBy(p => p.Date)
                .ToList();

            return daysWithReturn;
        }

        // c. Which 3 dates of 2022-2019 opens the second ten of the prices ranking?
        public List<GoldPrice> GetSecondTenPricesRanking()
        {
            // Filter prices between 2019 and 2022
            var pricesInRange = _goldPrices
                .Where(p => p.Date.Year >= 2019 && p.Date.Year <= 2022)
                .OrderByDescending(p => p.Price)
                .ToList();

            // Get items 11-13 (second ten starts at index 10)
            // Sprawdzamy, czy jest wystarczająca liczba rekordów
            if (pricesInRange.Count <= 10)
            {
                return pricesInRange.Skip(Math.Max(0, pricesInRange.Count - 3)).Take(3).ToList();
            }

            return pricesInRange.Skip(10).Take(3).ToList();
        }

        // d. (query syntax) What are the averages of gold prices in 2020, 2023, 2024?
        public Dictionary<int, decimal> GetAveragePricesByYear_QuerySyntax()
        {
            var years = new[] { 2020, 2023, 2024 };

            var averages = from p in _goldPrices
                           where years.Contains(p.Date.Year)
                           group p by p.Date.Year into yearGroup
                           select new { Year = yearGroup.Key, AveragePrice = yearGroup.Average(x => x.Price) };

            return averages.ToDictionary(a => a.Year, a => a.AveragePrice);
        }

        // e. When it would be best to buy gold and sell it between 2020 and 2024? What would be the return on investment?
        public (GoldPrice BestBuyDay, GoldPrice BestSellDay, decimal ReturnOnInvestment) GetBestBuyAndSellDays()
        {
            // Filter data between 2020 and 2024
            var pricesInRange = _goldPrices
                .Where(p => p.Date.Year >= 2020 && p.Date.Year <= 2024)
                .OrderBy(p => p.Date)
                .ToList();

            if (pricesInRange.Count < 2)
                throw new InvalidOperationException("Not enough data to determine buy and sell days");

            // Initialize with first two days in the range
            var bestBuyDay = pricesInRange[0];
            var bestSellDay = pricesInRange[1];
            var maxProfit = bestSellDay.Price - bestBuyDay.Price;

            // Try every possible buy day
            foreach (var buyDay in pricesInRange)
            {
                // Look for best sell day after buy day
                foreach (var sellDay in pricesInRange.Where(p => p.Date > buyDay.Date))
                {
                    var profit = sellDay.Price - buyDay.Price;
                    if (profit > maxProfit)
                    {
                        maxProfit = profit;
                        bestBuyDay = buyDay;
                        bestSellDay = sellDay;
                    }
                }
            }

            // Calculate ROI as a percentage
            var roi = (bestSellDay.Price / bestBuyDay.Price) - 1m;
            return (bestBuyDay, bestSellDay, roi);
        }

        // 3. Write a method that saves the list of prices to a file in XML format
        public void SavePricesToXmlFile(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<GoldPrice>));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, _goldPrices);
            }
        }

        // 4. Write a method that reads the contents of the XML file from the previous set using one instruction
        public List<GoldPrice> ReadPricesFromXmlFile(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                return new XmlSerializer(typeof(List<GoldPrice>)).Deserialize(stream) as List<GoldPrice> ?? new List<GoldPrice>();
            }
        }
    }
}