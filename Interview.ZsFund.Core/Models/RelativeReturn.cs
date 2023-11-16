using System.Text.Json.Serialization;

namespace Interview.ZsFund.Core.Models;

/// <summary>
///     相对收益
/// </summary>
/// <param name="Date">日期</param>
/// <param name="Return">相对收益</param>
public record struct RelativeReturnItem(DateTime Date, [property: JsonPropertyName("relativeReturn")] decimal Return);

/// <summary>
///     相对收益对比结果
/// </summary>
/// <param name="EntitySerialNumber"></param>
/// <param name="BaseEntitySerialNumber"></param>
/// <param name="StartDate"></param>
/// <param name="EndDate"></param>
/// <param name="Data"></param>
public record struct RelativeReturn(long EntitySerialNumber, long BaseEntitySerialNumber, DateTime StartDate, DateTime EndDate, List<RelativeReturnItem> Data);
