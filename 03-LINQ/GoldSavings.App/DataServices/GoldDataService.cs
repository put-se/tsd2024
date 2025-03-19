using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

        public void GoldPricesToXml(List<GoldPrice> goldPrices, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<GoldPrice>));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, goldPrices);
                }
                Console.WriteLine($"Gold prices saved to {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to XML: {ex.Message}.");
            }
        }

        public List<GoldPrice> GoldPricesFromXml(string filePath) =>
            (List<GoldPrice>)new XmlSerializer(typeof(List<GoldPrice>)).Deserialize(File.OpenRead(filePath));
    }
}
