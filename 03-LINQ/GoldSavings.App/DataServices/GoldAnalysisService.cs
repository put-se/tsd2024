using GoldSavings.App.Model;

namespace GoldSavings.App.Services;

public class GoldAnalysisService
{
    private readonly List<GoldPrice> _goldPrices;

    public GoldAnalysisService(List<GoldPrice> goldPrices)
    {
        _goldPrices = goldPrices;
    }

    public double GetAveragePrice(DateTime start, DateTime end)
    {
        return (
            from goldprice in _goldPrices
            where goldprice.Date < end && goldprice.Date >= start
            select goldprice
        ).Average(p => p.Price);
        //return _goldPrices.FindAll(p=> p.Date < end && p.Date >= start).Average(p => p.Price);
    }

    public GoldPrice[] Top(DateTime start, DateTime end, int n)
    {
        return _goldPrices.FindAll(p => p.Date < end && p.Date >= start)
                .OrderBy(p => -p.Price)
                .Take(n)
                .ToArray()
            ;
    }

    public GoldPrice[] Bottom(DateTime start, DateTime end, int n)
    {
        return (
            from gold in _goldPrices
            where gold.Date < end && gold.Date >= start
            orderby gold.Price
            select gold
        ).Take(n).ToArray();
    }

    public GoldPrice[] BestEarned(DateTime start, DateTime end, DateTime max, double threshold)
    {
        var all = _goldPrices.FindAll(p => p.Date < end && p.Date >= start);
        var minPrice = all.FindAll(p => p.Date < max).Select(p => p.Price).Min();
        var en = all.FindAll(p => p.Date >= max).GetEnumerator();

        var bestdate = new List<GoldPrice>();
        ;

        while (en.MoveNext())
            if (en.Current.Price / minPrice - 1 > threshold)
                bestdate.Add(en.Current);

        return bestdate.ToArray();
    }

    public GoldPrice[] SecondTen(DateTime start, DateTime end, int n)
    {
        return _goldPrices.FindAll(p => p.Date < end && p.Date >= start).OrderBy(p => -p.Price).Skip(10).Take(n)
            .ToArray();
    }
}