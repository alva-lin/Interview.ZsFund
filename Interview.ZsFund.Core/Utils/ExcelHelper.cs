using System.Text.RegularExpressions;
using Interview.ZsFund.Core.Models;
using NPOI.XSSF.UserModel;
using static System.Int32;

namespace Interview.ZsFund.Core.Utils;

public static partial class ExcelHelper
{
    [GeneratedRegex(@"^(.*)?\((\d+)\)$")]
    private static partial Regex MyRegex();

    public static List<MarketEntity> ReadMarketData(Stream stream)
    {
        using var workbook = new XSSFWorkbook(stream);
        var sheet = workbook.GetSheetAt(0);

        var result = new List<MarketEntity>();

        // 读取第一行
        var entityDict = new Dictionary<int, MarketEntity>();
        foreach (var cell in sheet.GetRow(0).Cells.Skip(1).Where(e => e is not null))
        {
            var match = MyRegex().Matches(cell.StringCellValue);
            if (match.Count <= 0)
            {
                continue;
            }

            var name = match[0].Groups[1].Value;
            _ = TryParse(match[0].Groups[2].Value, out var serialNumber);

            MarketEntity entity;

            // 简单判断
            if (name.Contains("指数"))
            {
                entity = new StockIndex
                {
                    Name = name,
                    SerialNumber = serialNumber
                };
            }
            else
            {
                entity = new Stock
                {
                    Name = name,
                    SerialNumber = serialNumber
                };
            }

            entityDict.Add(cell.ColumnIndex, entity);
            result.Add(entity);
        }

        // 读取数据
        for (var rowIndex = sheet.FirstRowNum + 1; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            var date = row.GetCell(0).DateCellValue;
            foreach (var cell in row.Cells.Skip(1))
            {
                if (cell is not null && entityDict.TryGetValue(cell.ColumnIndex, out var entity))
                {
                    var price = (decimal)cell.NumericCellValue;
                    entity.Data.Add(new MarketData(date, price));
                }
            }
        }

        // 排序整理
        foreach (var entity in result)
        {
            entity.Data = entity.Data.OrderBy(x => x.Date).ToList();
        }

        return result.OrderBy(e => e.SerialNumber).ToList();
    }
}
