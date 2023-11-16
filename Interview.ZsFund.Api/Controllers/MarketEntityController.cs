using Interview.ZsFund.Api.Models;
using Interview.ZsFund.Core;
using Interview.ZsFund.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Interview.ZsFund.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MarketEntityController : ControllerBase
{
    private readonly MarketEntityService _service;

    public MarketEntityController(MarketEntityService service)
    {
        _service = service;
    }

    /// <summary>
    ///     获取所有股票列表
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<MarketEntity> GetMarketEntitiesAsync(CancellationToken cancellationToken)
    {
        return _service.GetMarketEntitiesAsync(null, cancellationToken);
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
