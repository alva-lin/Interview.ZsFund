// See https://aka.ms/new-console-template for more information

using Interview.ZsFund.Core.Utils;
Console.WriteLine("Hello, World!");

var path = Path.Combine("data", "2019.stock.xlsx");
using var file = File.Open(path, FileMode.Open);
var result = ExcelHelper.ReadMarketData(file);

Console.WriteLine(result.Count);
