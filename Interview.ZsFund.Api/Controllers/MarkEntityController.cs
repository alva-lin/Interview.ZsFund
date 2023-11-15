using Interview.ZsFund.Api.Models;
using Interview.ZsFund.Core;
using Interview.ZsFund.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Interview.ZsFund.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MarkEntityController : ControllerBase
{
    private readonly MarketEntityService _service;

    public MarkEntityController(MarketEntityService service)
    {
        _service = service;
    }

    /// <summary>
    ///     批量获取数据
    /// </summary>
    /// <param name="serialNumbers"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("entities")]
    public IEnumerable<MarketEntity> GetMarketEntitiesAsync([FromBody] IEnumerable<int> serialNumbers,
        CancellationToken cancellationToken)
    {
        return _service.GetMarketEntitiesAsync(serialNumbers, cancellationToken);
    }

    /// <summary>
    ///     获取相对收益
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("relative-return")]
    public IEnumerable<RelativeReturn> GetRelativeReturnAsync([FromBody] GetRelativeReturnRequest model,
        CancellationToken cancellationToken)
    {
        return _service.GetRelativeReturnAsync(model.SerialNumbers, model.BaseSerialNumber, model.StartDate,
            model.EndDate, cancellationToken);
    }
}
