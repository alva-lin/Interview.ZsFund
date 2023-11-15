namespace Interview.ZsFund.Api.Models;

public class GetRelativeReturnRequest
{
    public IEnumerable<int> SerialNumbers { get; set; } = Array.Empty<int>();

    public int BaseSerialNumber { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
