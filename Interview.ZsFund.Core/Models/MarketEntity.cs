namespace Interview.ZsFund.Core.Models;

/// <summary>
///     市值数据
/// </summary>
/// <param name="Date">日期</param>
/// <param name="Price">市值</param>
public record struct MarketDataItem(DateTime Date, decimal Price);

public abstract class MarketEntity
{
    public int SerialNumber { get; set; }

    public string Name { get; set; } = null!;

    public List<MarketDataItem> MarketData { get; set; } = new();

    public RelativeReturn CalcRelativeReturn(MarketEntity baseEntity, DateTime startDate, DateTime endDate)
    {
        // 筛选出日期在 startDate 和 endDate 之间的数据
        var predicate = new Func<DateTime, bool>(e => e >= startDate && e <= endDate);
        var dataA = MarketData.Where(e => predicate(e.Date)).OrderBy(e => e.Date).ToList();
        var dataB = baseEntity.MarketData.Where(e => predicate(e.Date)).OrderBy(e => e.Date).ToList();
        if(dataA.Count == 0 || dataB.Count == 0)
        {
            return new RelativeReturn(SerialNumber, baseEntity.SerialNumber, startDate, endDate, new List<RelativeReturnItem>());
        }

        var result = new List<RelativeReturnItem>();
        int indexA = 0, indexB = 0;
        MarketDataItem lastDataItemA = dataA[0], lastDataItemB = dataB[0];
        while (indexA < dataA.Count && indexB < dataB.Count)
        {
            MarketDataItem itemA = dataA[indexA], itemB = dataB[indexB];
            if (itemA.Date > itemB.Date)
            {
                indexB++;
            }
            else if (itemA.Date < itemB.Date)
            {
                indexA++;
            }
            else
            {
                // 找到相同日期的数据，再进行计算
                // 如果 dataA[0] 和 dataB[0] 的日期不同，会跳过一次

                if (lastDataItemA.Date == lastDataItemB.Date)
                {
                    var returnA = itemA.Price / lastDataItemA.Price;
                    var returnB = itemB.Price / lastDataItemB.Price;
                    var relativeReturn = result.Count > 0 ? result[^1].Return : 1;
                    relativeReturn *= (returnA - returnB + 1);
                    result.Add(new RelativeReturnItem(itemA.Date, relativeReturn));
                }

                (lastDataItemA, lastDataItemB) = (itemA, itemB);
                indexA++;
                indexB++;
            }
        }

        return new RelativeReturn(SerialNumber, baseEntity.SerialNumber, startDate, endDate, result);
    }
}

public class StockIndex : MarketEntity
{
}

public class Stock : MarketEntity
{
}
