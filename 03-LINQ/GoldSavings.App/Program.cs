using GoldSavings.App.Model;
using GoldSavings.App.Client;
namespace GoldSavings.App;
using System.Xml.Serialization;

class Program
{
    static void SavePricesToXml(List<GoldPrice> prices, string fileName)
    {
        try
        {

            XmlSerializer serializer = new XmlSerializer(typeof(List<GoldPrice>));

            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, prices);
            }

            Console.WriteLine($"Prices has been saved in  {fileName} with XML format");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in this xml file: {ex.Message}");
        }
    }
    static List<GoldPrice> ReadPricesFromXml(string fileName)
    {
        string xmlContent = File.ReadAllText(fileName);
        return new XmlSerializer(typeof(List<GoldPrice>)).Deserialize(new StringReader(xmlContent)) as List<GoldPrice>;
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Saver!");

        GoldClient goldClient = new GoldClient();

        GoldPrice currentPrice = goldClient.GetCurrentGoldPrice().GetAwaiter().GetResult();
        Console.WriteLine($"The price for today is {currentPrice.Price}");

        List<GoldPrice> thisMonthPrices = goldClient.GetGoldPrices(new DateTime(2024, 03, 01), new DateTime(2024, 03, 11)).GetAwaiter().GetResult();
        foreach (var goldPrice in thisMonthPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }
        Console.WriteLine("exercice 3 : ");
        List<GoldPrice> lastYearPrices = goldClient.GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31)).GetAwaiter().GetResult();
        foreach (var goldPrice in lastYearPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }
        IEnumerable<GoldPrice> filteredGoldPrices = lastYearPrices.OrderBy(goldPrice => goldPrice.Price);
        Console.WriteLine("exercice 4 : ");
        List<GoldPrice> lastYearsPrices = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 12, 31)).GetAwaiter().GetResult();
        foreach (var goldPrice in lastYearsPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }
        List<GoldPrice> january2020Prices = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 01, 31)).GetAwaiter().GetResult();
        List<GoldPrice> lastMonth2020Prices = goldClient.GetGoldPrices(new DateTime(2020, 12, 01), new DateTime(2020, 12, 31)).GetAwaiter().GetResult();
        double januaryPrice = january2020Prices.FirstOrDefault()?.Price ?? 0;
        double lastMonthPrice = lastMonth2020Prices.FirstOrDefault()?.Price ?? 0;
        double profitPercentage = ((lastMonthPrice - januaryPrice) / januaryPrice) * 100;

        Console.WriteLine($"Profit percentage from January to last month of 2020: {profitPercentage}%");
        IEnumerable<GoldPrice> profitableDates = january2020Prices.Concat(lastMonth2020Prices)
            .Where(goldPrice => ((goldPrice.Price - januaryPrice) / januaryPrice) * 100 > 5)
            .OrderBy(goldPrice => goldPrice.Date);

        Console.WriteLine("Dates with more than 5% profit:");

        foreach (var goldPrice in profitableDates)
        {
            Console.WriteLine($"Date: {goldPrice.Date}, Price: {goldPrice.Price}");
        }
        Console.WriteLine("exercice 5 : ");

        List<GoldPrice> Price2022 = goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> Price2021 = goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> Price2020 = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> Price2019 = goldClient.GetGoldPrices(new DateTime(2019, 01, 01), new DateTime(2019, 12, 31)).GetAwaiter().GetResult();

        List<GoldPrice> Prices2022to2019 = Price2022.Concat(Price2021).Concat(Price2020).Concat(Price2019).ToList();
        List<GoldPrice> Pricessorted = Prices2022to2019.OrderBy(p => p.Price).ToList();

        if (Pricessorted.Count >= 13)
        {
            Console.WriteLine($"Top 11 - The price of {Pricessorted[10].Date} is {Pricessorted[10].Price}");
            Console.WriteLine($"Top 12 - The price of {Pricessorted[11].Date} is {Pricessorted[11].Price}");
            Console.WriteLine($"Top 13 - The price of {Pricessorted[12].Date} is {Pricessorted[12].Price}");
        }
        Console.WriteLine("exercice 6 : ");
        List<GoldPrice> Pricein2021 = goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31)).GetAwaiter().GetResult();
        double averageprice2021 = Pricein2021.Average(p => p.Price);
        Console.WriteLine($"The average gold price for the year 2021: {averageprice2021}");
        List<GoldPrice> Pricein2022 = goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31)).GetAwaiter().GetResult();
        double averageprice2022 = Pricein2022.Average(p => p.Price);
        Console.WriteLine($"The average gold price for the year 2022: {averageprice2022}");
        List<GoldPrice> Pricein2023 = goldClient.GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31)).GetAwaiter().GetResult();
        double averageprice2023 = Pricein2023.Average(p => p.Price);
        Console.WriteLine($"The average gold price for the year 2023: {averageprice2023}");
        Console.WriteLine("exercice 7 : ");
        List<GoldPrice> price2023 = goldClient.GetGoldPrices(new DateTime(2023, 01, 01), new DateTime(2023, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> price2022 = goldClient.GetGoldPrices(new DateTime(2022, 01, 01), new DateTime(2022, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> price2021 = goldClient.GetGoldPrices(new DateTime(2021, 01, 01), new DateTime(2021, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> price2020 = goldClient.GetGoldPrices(new DateTime(2020, 01, 01), new DateTime(2020, 12, 31)).GetAwaiter().GetResult();
        List<GoldPrice> price2019 = goldClient.GetGoldPrices(new DateTime(2019, 01, 01), new DateTime(2019, 12, 31)).GetAwaiter().GetResult();

        List<GoldPrice> prices2019to2023 = price2023.Concat(price2022).Concat(price2021).Concat(price2020).Concat(price2019).ToList();
        if (prices2019to2023.Count > 1)
        {
            double buyPrice = double.MaxValue;
            double sellPrice = double.MinValue;

            foreach (var price in prices2019to2023)
            {
                if (price.Price < buyPrice)
                    buyPrice = price.Price;

                if (price.Price > sellPrice)
                    sellPrice = price.Price;
            }

            double roi = ((sellPrice - buyPrice) / buyPrice) * 100;

            Console.WriteLine($" best to buy gold : {buyPrice}");
            Console.WriteLine($" best to sell gold : {sellPrice}");
            Console.WriteLine($"return on investment : {roi}%");
        }
        else
        {
            Console.WriteLine("No price detected between 2019 to 2023.");
        }
        Console.WriteLine("exercice 8 : ");
        SavePricesToXml(prices2019to2023, "gold_prices.xml");
        Console.WriteLine("exercice 9 : ");
        List<GoldPrice> pricesFromXml = ReadPricesFromXml("gold_prices.xml");
    }

}
