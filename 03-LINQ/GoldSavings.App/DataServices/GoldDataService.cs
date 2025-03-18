using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoldSavings.App.Client;
using GoldSavings.App.Model;

namespace GoldSavings.App.Services
{
    public class GoldDataService
    {
        private readonly GoldClient _goldClient;

        public GoldDataService()
        {
            _goldClient = new GoldClient();
        }

        public async Task<List<GoldPrice>> GetGoldPrices(DateTime startDate, DateTime endDate)
        {
            var prices = await _goldClient.GetGoldPrices(startDate, endDate);
            return prices ?? new List<GoldPrice>();  // Prevent null values
        }
    }
}
