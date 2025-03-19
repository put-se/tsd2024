using GoldSavings.App.Model;

namespace GoldSavings.App.Services;

public static class GoldResultPrinter
{
    public static void PrintPrices(List<GoldPrice> prices, string title)
    {
        Console.WriteLine($"\n--- {title} ---");
        foreach (var price in prices) Console.WriteLine($"{price.Date:yyyy-MM-dd} - {price.Price} PLN");
    }

    public static void PrintSingleValue<T>(T value, string title)
    {
        Console.WriteLine($"\n{title}: {value}");
    }
}