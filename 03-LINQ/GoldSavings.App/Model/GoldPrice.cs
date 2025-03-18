using Newtonsoft.Json;

using System;
using System.Xml.Serialization;

namespace GoldSavings.App.Model
{
    [Serializable]
    public class GoldPrice
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }

        // For XML serialization, good to have a default constructor
        public GoldPrice()
        {
        }

        public GoldPrice(DateTime date, decimal price)
        {
            Date = date;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Date.ToShortDateString()}: {Price:N2} PLN";
        }
    }
}