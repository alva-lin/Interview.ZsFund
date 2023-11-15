// See https://aka.ms/new-console-template for more information

using Interview.ZsFund.Core;
using Interview.ZsFund.Core.Utils;

Console.WriteLine("Hello, World!");

var path = Path.Combine("data", "2019.stock.xlsx");
using var file = File.Open(path, FileMode.Open);
var result = ExcelHelper.ReadMarketData(file);

Console.WriteLine(result.Count);

var 茅台 = result.First(e => e.Name.Contains("茅台"));
var 上证指数 = result.First(e => e.Name.Contains("上证指数"));

var relativeReturn = 茅台.CalcRelativeReturn(上证指数, new DateTime(2019, 3, 1), new DateTime(2019, 5, 31));

Console.WriteLine(relativeReturn.Data.Count);

var service = new MarketEntityService();
var data = service.GetRelativeReturnAsync(
    new[] { 1, 600519, 601066, 688001, 600647 },
    0,
    new DateTime(2019, 1, 1),
    new DateTime(2019, 12, 31),
    CancellationToken.None);

Console.WriteLine(data.Count);
