namespace MmaSolution.AppApi.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
[ApiController]
public class RolesController : BaseController
{
    private readonly RoleService _roleService;
    private readonly Services.Translator _translator;
    private readonly ILogger<RolesController> _logger;

    public RolesController(RoleService roleService, Services.Translator translator, ILogger<RolesController> logger) : base(translator)
    {
        _roleService = roleService;
        _translator = translator;
        _logger = logger;
    }

    [HttpGet]
    [RequiredPermission("Read")]
    public async Task<IActionResult> GetAll([FromQuery] QueryViewModel query, CancellationToken cancellationToken)
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
            _ = Guid.TryParse(User.FindFirstValue("Id"), out Guid userId);
            query.UserId = userId;
            var data = await _roleService.All(query);
            data.Messages = data.Messages.Select(m => _translator.Translate(m, Language)).ToList();
            if (data.IsSuccess)
                return Ok(data);

            return BadRequest(data);

        }
        catch (HttpException ex)
        {
            _logger.LogError(ex.Message, query, ex);
            return BadRequest(HandleHttpException<List<AppRoleReadModel>>(ex));
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

    [HttpGet("{id}")]
    [RequiredPermission("Read")]
    public async Task<IActionResult> GetOne(Guid id, CancellationToken cancellationToken)
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
            var data = await _roleService.Find(id);
            data.Messages = data.Messages.Select(m => _translator.Translate(m, Language)).ToList();

            if (data.IsSuccess)

                return Ok(data);

            return BadRequest(data);

        }
        catch (HttpException ex)
        {

            _logger.LogError(ex.Message, ex);
            return BadRequest(HandleHttpException<AppRoleModifyModel>(ex));
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



    [HttpPost]
    [RequiredPermission("Create")]
    public async Task<IActionResult> PostRole([FromBody] AppRoleModifyModel model, CancellationToken cancellationToken)
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
            var data = await _roleService.Add(model);
            data.Messages = data.Messages.Select(m => _translator.Translate(m, Language)).ToList();

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

    [HttpPut("{id}")]
    [RequiredPermission("Update")]
    public async Task<IActionResult> PutRole(Guid id, [FromBody] AppRoleModifyModel model, CancellationToken cancellationToken)
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
            if (model.Id.Equals(id) == false)           
            {
                var result = new AcknowledgeViewModel
                {
                    IsSuccess = false
                };
                result.Messages.Add(_translator.Translate("InvalidData", Language));
                return BadRequest(result);
            }
            var data = await _roleService.Update(model);
            data.Messages = data.Messages.Select(m => _translator.Translate(m, Language)).ToList();

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

    [HttpDelete("{id}")]
    [RequiredPermission("Update,Delete")]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
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
            var data = await _roleService.Delete(id);
            data.Messages = data.Messages.Select(m => _translator.Translate(m, Language)).ToList();

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
