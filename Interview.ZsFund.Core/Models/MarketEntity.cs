namespace Interview.ZsFund.Core.Models;

public record struct MarketData(DateTime Date, decimal Price);

public abstract class MarketEntity
{
    public int SerialNumber { get; set; }

    public string Name { get; set; } = null!;

    public List<MarketData> Data { get; set; } = new();
}

public class StockIndex : MarketEntity
{
}

public class Stock : MarketEntity
{
}
