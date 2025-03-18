using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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
        
        public void SaveToXML(List<GoldPrice> prices, string filePath)
        {
            var xml = new XElement("GoldPrices",
                prices.Select(p => 
                    new XElement("GoldPrice",
                        new XElement("Date", p.Date.ToString("yyyy-MM-dd")),
                        new XElement("Price", p.Price)
                    )
                )
            );

            xml.Save(filePath);
        }
        
        public List<GoldPrice> LoadFromXML(string filePath) => 
            File.Exists(filePath) ? 
                XDocument.Load(filePath).Root.Elements("GoldPrice")
                    .Select(x => new GoldPrice
                    {
                        Date = DateTime.Parse(x.Element("Date").Value),
                        Price = double.Parse(x.Element("Price").Value, CultureInfo.InvariantCulture)
                    }).ToList() : new List<GoldPrice>();
    }
}
