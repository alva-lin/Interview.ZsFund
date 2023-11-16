using System.Runtime.CompilerServices;
using Interview.ZsFund.Core.Models;
using Interview.ZsFund.Core.Utils;

namespace Interview.ZsFund.Core;

public class MarketEntityService
{

    private static readonly List<MarketEntity> data = new();

    static MarketEntityService()
    {
        var path = Path.Combine("data", "2019.stock.xlsx");
        using var file = File.Open(path, FileMode.Open);
        var result = ExcelHelper.ReadMarketData(file);
        data.AddRange(result);
    }

    /// <summary>
    ///     批量获取数据
    /// </summary>
    /// <param name="serialNumbers">股票编号</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public List<MarketEntity> GetMarketEntitiesAsync(IEnumerable<int>? serialNumbers,
        CancellationToken cancellationToken)
    {
        return serialNumbers == null ? data.ToList() : data.Where(e => serialNumbers.Contains(e.SerialNumber)).ToList();
    }

    /// <summary>
    ///     获取相对收益
    /// </summary>
    /// <param name="serialNumbers">需要对比的股票编号</param>
    /// <param name="baseSerialNumber">基准股票编号</param>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public List<RelativeReturn> GetRelativeReturnAsync(
        IEnumerable<int> serialNumbers, int baseSerialNumber,
        DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var entities = GetMarketEntitiesAsync(serialNumbers.Union(new[] { baseSerialNumber }), cancellationToken);

        var baseEntity = entities.FirstOrDefault(e => e.SerialNumber == baseSerialNumber);
        if (baseEntity is null)
        {
            // 抛出异常
            return new List<RelativeReturn>();
        }

        var result = entities
            .Where(e => e.SerialNumber != baseSerialNumber)
            .Select(e => e.CalcRelativeReturn(baseEntity, startDate, endDate))
            .ToList();
        return result;
    }
}
