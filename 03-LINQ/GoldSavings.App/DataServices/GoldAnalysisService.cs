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

        public double[] Top(int n)
        {
            return _goldPrices.OrderBy(p => -p.Price).Take(n).Select(p => p.Price).ToArray();
        }

        public double[] Bottom(int n)
        {
            return _goldPrices.OrderBy(p => p.Price).Take(n).Select(p => p.Price).ToArray();
        }

        public List<DateTime> BestEarned(DateTime max, double threshold)
        {
            var minPrice = _goldPrices.FindAll(p => p.Date < max).Select(p => p.Price).Min();
            var en = _goldPrices.FindAll(p => p.Date >= max).GetEnumerator();

            List<DateTime> bestdate = new List<DateTime>(); ;

            while (en.MoveNext())
            {
                if (en.Current.Price / minPrice - 1 > threshold)
                {
                    bestdate.Add(en.Current.Date);
                }
            }

            return bestdate;
        }
        
        public double[] SecondTen()
        {
            return _goldPrices.OrderBy(p=>-p.Price).Skip(10).Take(3).Select(p => p.Price).ToArray();
        }
    }
}