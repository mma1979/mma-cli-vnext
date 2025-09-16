using MmaSolution.Core.Models.Identity.AppResource;

using static MmaSolution.Services.Account.AccessControlService;

namespace MmaSolution.AppApi.Controllers.v1.Account;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
[ApiController]

public class AccessControlController : BaseController
{
    private readonly Services.Translator _translator;
    private readonly ILogger<AccessControlController> _logger;
    private readonly AccessControlService _accessControlService;
    public AccessControlController(Services.Translator translator, ILogger<AccessControlController> logger, AccessControlService accessControlService) : base(translator)
    {
        _translator = translator;
        _logger = logger;
        _accessControlService = accessControlService;
    }

    [HttpPost("add-resource")]
    [RequiredPermission("Create")]
    public async Task<IActionResult> PostAccessControl([FromBody] AppResourceModifyModel model, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
            });
        }

        try
        {
            var data = await _accessControlService.AddReaource(model);
            data.Messages = [.. data.Messages.Select(m => _translator.Translate(m, Language))];

            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);
        }
        catch (HttpException ex)
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(HandleHttpException(ex));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(new
            {
                IsSuccess = false,
                StatusCode = 500
            });
        }
    }

    [HttpPost("assign-resource-to-client")]
    [RequiredPermission("Create")]
    public async Task<IActionResult> AssignResourceToClient([FromBody] AssignResourceToUserRequest model, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
            });
        }

        try
        {
            var data = await _accessControlService.AssignResourceToUser(model);
            data.Messages = [.. data.Messages.Select(m => _translator.Translate(m, Language))];

            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);
        }
        catch (HttpException ex)
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(HandleHttpException(ex));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(new
            {
                IsSuccess = false,
                StatusCode = 500
            });
        }
    }
}