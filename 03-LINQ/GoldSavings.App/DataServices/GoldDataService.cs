using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
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
        
        public void SaveToXML(List<GoldPrice> goldPrices, string fileName)
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("GoldPrices");
            doc.AppendChild(root);

            foreach (var price in goldPrices)
            {
                var priceNode = doc.CreateElement("Price");
                priceNode.SetAttribute("Date", price.Date.ToString("yyyy-MM-dd"));
                priceNode.SetAttribute("Price", price.Price.ToString());
                root.AppendChild(priceNode);
            }
            doc.Save(fileName);
            Console.WriteLine($"Saved {goldPrices.Count} records to {fileName}");
        }
        
        public List<GoldPrice> LoadFromXML(string fileName)
        {
            var doc = new XmlDocument();
            doc.Load(fileName);
            var prices = new List<GoldPrice>();
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var price = new GoldPrice()
                {
                    Date = DateTime.Parse(node.Attributes["Date"].Value),
                    Price = double.Parse(node.Attributes["Price"].Value)
                };
                prices.Add(price);
            }
            return prices;
        }
    }
}
