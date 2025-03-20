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

        // other written methods below

        public double GetAveragePriceQuery()
        {
            return (from data in _goldPrices select data.Price).Average();
        }

        public List<GoldPrice> highestPricesWithDateMethod(int n) {
            return _goldPrices.OrderByDescending(p => p.Price) 
                            .Take(n)                         
                            .ToList();                        
        }

        public List<GoldPrice> lowestPricesWithDateMethod(int n) {
            return _goldPrices.OrderBy(p => p.Price) 
                            .Take(n)                         
                            .ToList();                        
        }


        public List<GoldPrice> highestPricesMethod(int n) {
            return _goldPrices.OrderByDescending(p => p.Price).Take(n).ToList();
        }

        public List<GoldPrice> lowestPricesMethod(int n) {
            return _goldPrices.OrderBy(p => p.Price).Take(n).ToList();
        }

        public List<GoldPrice> highestPricesQuery(int n) {
            return (from data in _goldPrices
                    orderby data.Price descending
                    select data) 
                .Take(n)
                .ToList(); 
        }

        public List<GoldPrice> lowestPricesQuery(int n) {
            return (from data in _goldPrices
                    orderby data.Price
                    select data) 
                .Take(n)
                .ToList();  
        }

        public List<GoldPrice> secondTenPrices() {
            return _goldPrices.OrderByDescending(gp => gp.Price).ToArray().Skip(9).Take(3).ToList();
        }

        public List<GoldPrice> upperPrices(double threeshold) {
            return _goldPrices.Where(p => p.Price > threeshold).ToList();
        }
    }
}
