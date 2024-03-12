using GoldSavings.App.Model;
using GoldSavings.App.Client;
namespace GoldSavings.App;
using System.Xml.Serialization;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Saver!");

        GoldClient goldClient = new GoldClient();

        GoldPrice currentPrice = goldClient.GetCurrentGoldPrice().GetAwaiter().GetResult();
        Console.WriteLine($"The price for today is {currentPrice.Price}");

        List<GoldPrice> thisMonthPrices = goldClient.GetGoldPrices(new DateTime(2024, 03, 01), new DateTime(2024, 03, 11)).GetAwaiter().GetResult();
        foreach(var goldPrice in thisMonthPrices)
        {
            Console.WriteLine($"The price for {goldPrice.Date} is {goldPrice.Price}");
        }

        
        //Zadanie 1.3
        // Pobierz ceny złota za ostatni rok
        DateTime lastYear = DateTime.Today.AddYears(-1);
        DateTime today = DateTime.Today;
        List<GoldPrice> lastYearPrices = goldClient.GetGoldPrices(lastYear, today).GetAwaiter().GetResult();

        // Znajdź TOP 3 najwyższe i najniższe ceny
        var top3HighestPrices = lastYearPrices.OrderByDescending(p => p.Price).Take(3).ToList();
        var top3LowestPrices = lastYearPrices.OrderBy(p => p.Price).Take(3).ToList();

        var top3HighestPricesQuery = (from p in lastYearPrices orderby p.Price descending select p).Take(3).ToList();
        var top3LowestPricesQuery = (from p in lastYearPrices orderby p.Price ascending select p).Take(3).ToList();

        Console.WriteLine("TOP 3 highest gold prices in the last year:");
        foreach (var price in top3HighestPrices)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }
        foreach (var price in top3HighestPricesQuery)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }

        Console.WriteLine("TOP 3 lowest gold prices in the last year:");
        foreach (var price in top3LowestPrices)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }
        foreach (var price in top3LowestPricesQuery)
        {
            Console.WriteLine($"Date: {price.Date}, Price: {price.Price}");
        }

        
        //zadanie 1.4
        // Pobierz ceny złota w styczniu 2020
        DateTime startJanuary2020 = new DateTime(2020, 01, 01);
        DateTime endJanuary2020 = new DateTime(2020, 01, 31);
        List<GoldPrice> january2020Prices = goldClient.GetGoldPrices(startJanuary2020, endJanuary2020).GetAwaiter().GetResult();

        // Oblicz średnią cenę zakupu w styczniu 2020
        double averageBuyPrice = january2020Prices.Average(p => p.Price);

        List<GoldPrice> allPricesAfterJanuary2020 = new List<GoldPrice>();

        DateTime startPeriod = new DateTime(2020, 02, 01); // Start od lutego 2020
        DateTime endPeriod = DateTime.Today; // Do dzisiaj
        DateTime tempEnd;

        while (startPeriod < endPeriod)
        {
            tempEnd = startPeriod.AddYears(1) > endPeriod ? endPeriod : startPeriod.AddYears(1).AddDays(-1);
            List<GoldPrice> yearlyPrices = goldClient.GetGoldPrices(startPeriod, tempEnd).GetAwaiter().GetResult();
            allPricesAfterJanuary2020.AddRange(yearlyPrices);
            startPeriod = tempEnd.AddDays(1);
        }

        var profitableDaysQuery = (from p in allPricesAfterJanuary2020 where ((p.Price - averageBuyPrice) / averageBuyPrice * 100) > 5 select p.Date).ToList();


        // Sprawdź, kiedy zysk przekroczył 5%
        List<DateTime> profitableDays = new List<DateTime>();
        foreach (var price in allPricesAfterJanuary2020)
        {
            double profit = (price.Price - averageBuyPrice) / averageBuyPrice * 100; // Zysk w procentach
            //Console.WriteLine($"Date: {price.Date.ToShortDateString()}, Profit: {profit}%"); // Dodane do debugowania
            if (profit > 5) // 5% zysku
            {
                profitableDays.Add(price.Date);
            }
        
        }

        // Wyświetl dni, w których zysk przekroczył 5%
        Console.WriteLine("Days when the profit exceeded 5%:");
        //foreach (var day in profitableDays)
        //{
            //Console.WriteLine(day.ToShortDateString());
        //}


        //zadanie 1.5

        List<GoldPrice> combinedPrices = new List<GoldPrice>();
        // Pobieranie danych za każdy rok oddzielnie i łączenie ich w jedną listę
        for (int year = 2019; year <= 2022; year++)
        {
            DateTime startOfYear = new DateTime(year, 01, 01);
            DateTime endOfYear = new DateTime(year, 12, 31);
            List<GoldPrice> yearlyPrices = goldClient.GetGoldPrices(startOfYear, endOfYear).GetAwaiter().GetResult();
            combinedPrices.AddRange(yearlyPrices);
        }

        // Sortowanie połączonych danych cenowych od najniższej do najwyższej ceny
        var sortedPrices = combinedPrices.OrderBy(p => p.Price).ToList();

        // Wybieranie dat dla cen na pozycjach 11-13 w rankingu
        if (sortedPrices.Count >= 13) // Upewnienie się, że mamy wystarczającą ilość danych
        {
            var secondTenDates = sortedPrices.Skip(10).Take(3).Select(p => p.Date).ToList();
            var secondTenDatesQuery = (from p in combinedPrices
                           orderby p.Price
                           select p.Date)
                           .Skip(10)
                           .Take(3)
                           .ToList();

            Console.WriteLine("Dates that open the second ten of the gold price ranking from 2019 to 2022:");
            foreach (var date in secondTenDates)
            {
                Console.WriteLine(date.ToShortDateString());
            }
            foreach (var date in secondTenDatesQuery)
            {
                Console.WriteLine(date.ToShortDateString());
            }
        }
        else
        {
            Console.WriteLine("Not enough data to determine the second ten of the gold price ranking.");
        }


        //Zadanie 1.6
        // Lista lat, dla których chcemy obliczyć średnie ceny
        int[] years = new int[] { 2021, 2022, 2023 };
        foreach (int year in years)
        {
            DateTime startOfYear = new DateTime(year, 01, 01);
            DateTime endOfYear = new DateTime(year, 12, 31);
            
            // Pobieranie danych cenowych za dany rok
            List<GoldPrice> yearlyPrices = goldClient.GetGoldPrices(startOfYear, endOfYear).GetAwaiter().GetResult();
            
            // Obliczanie średniej ceny złota za dany rok, jeśli są dostępne jakiekolwiek dane
            if (yearlyPrices.Count > 0)
            {
                double averagePrice = yearlyPrices.Average(p => p.Price);
                Console.WriteLine($"The average gold price in {year} was: {averagePrice}");
                double averagePriceQuery = (from p in yearlyPrices select p.Price).Average();
                Console.WriteLine($"[Query] The average gold price in {year} was: {averagePriceQuery}");
            }
            else
            {
                Console.WriteLine($"No data available for the year {year}.");
            }
        }


        //Zadanie 1.7
        // Znajdowanie najlepszego dnia na zakup
        var bestBuy = combinedPrices.OrderBy(p => p.Price).FirstOrDefault();

        // Znajdowanie najlepszego dnia na sprzedaż po dacie zakupu
        var bestSell = combinedPrices.Where(p => p.Date > bestBuy.Date).OrderByDescending(p => p.Price).FirstOrDefault();

        // Obliczanie zwrotu z inwestycji (ROI), tylko jeśli znaleziono obie daty
        if (bestBuy != null && bestSell != null)
        {
            double roi = (bestSell.Price - bestBuy.Price) / bestBuy.Price * 100;

            // Wyświetlanie wyników
            Console.WriteLine($"Najlepszy moment na zakup: {bestBuy.Date.ToShortDateString()}, cena: {bestBuy.Price}");
            Console.WriteLine($"Najlepszy moment na sprzedaż: {bestSell.Date.ToShortDateString()}, cena: {bestSell.Price}");
            Console.WriteLine($"Zwrot z inwestycji: {roi}%");
        }
        else
        {
            Console.WriteLine("Nie można znaleźć optymalnych dat zakupu i sprzedaży w dostępnych danych.");
        }

        //Zadanie 1.8
        string filePath = @"prices.xml";
        SavePricesToXml(combinedPrices, filePath);

        //Zadanie 1.9
        List<GoldPrice> prices = LoadPricesFromXml(filePath);

    }

    //Zadanie 1.8
    public static void SavePricesToXml(List<GoldPrice> prices, string filePath)
    {
        // Tworzenie XmlSerializer do serializacji obiektów GoldPrice
        XmlSerializer serializer = new XmlSerializer(typeof(List<GoldPrice>));

        // Utworzenie strumienia plikowego z użyciem ścieżki filePath
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            // Serializacja listy prices do pliku XML
            serializer.Serialize(fileStream, prices);
        }
    }

    //Zadanie 1.9
    public static List<GoldPrice> LoadPricesFromXml(string filePath) => 
        (List<GoldPrice>)new XmlSerializer(typeof(List<GoldPrice>)).Deserialize(new FileStream(filePath, FileMode.Open, FileAccess.Read));
}
