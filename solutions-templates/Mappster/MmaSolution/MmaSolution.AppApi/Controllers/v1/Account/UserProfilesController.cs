using MmaSolution.Core.Models.Identity.AppUser;

namespace MmaSolution.AppApi.Controllers.v1.Account;

[Route("api/v{version:apiVersion}/user-profile")]
[ApiVersion(1.0)]
[ApiController]
public class UserProfilesController : BaseController
{
    private readonly UserProfileService _userProfileService;
    private readonly Services.Translator _translator;
    private readonly ILogger<UserProfilesController> _logger;

    public UserProfilesController(UserProfileService userProfileService, Services.Translator translator, ILogger<UserProfilesController> logger) : base(translator)
    {
        _userProfileService = userProfileService;
        _translator = translator;
        _logger = logger;
    }





    [HttpGet]
    [RequiredPermission("Read")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
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
            var userId = User.FindFirstValue("Id").ToGuid();
            var data = await _userProfileService.Find(userId);
            data.Messages = [.. data.Messages.Select(m => _translator.Translate(m, Language))];

            if (data.IsSuccess)

                return Ok(data);

            return BadRequest(data);

        }
        catch (HttpException ex)
        {

            _logger.LogError(ex.Message, ex);
            return BadRequest(HandleHttpException<UserProfile>(ex));
        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
            var result = new ResultViewModel<List<UserProfile>>
            {
                IsSuccess = false,
                StatusCode = 500
            };
            return BadRequest(result);
        }
    }


    [HttpPost("change-picture")]
    [RequiredPermission("Update")]
    public async Task<IActionResult> PutUserProfile(IFormFile file, CancellationToken cancellationToken)
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
            var userId = User.FindFirstValue("Id").ToGuid();

            var data = await _userProfileService.UpdatePicture(userId, file);
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
            var result = new AcknowledgeViewModel
            {
                IsSuccess = false,
                StatusCode = 500
            };
            return BadRequest(result);
        }
    }

    // GET: api/v1/user-profile/images/Default
    [HttpGet("images/{*objectName}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImageByNameAsync(string objectName)
    {
        var contentType = GetContentType("Default.png");
        var userId = User.FindFirstValue("Id").ToGuid();

        var fileStream = await _userProfileService.DownloadPicture(objectName);
        if (fileStream.Length > 0)
        {
            return File(fileStream, contentType);
        }

        return NotFound(new
        {
            IsSuccess = false,
            Messages = new[] { _translator.Translate(ResourcesKeys.ITEM_NOT_FOUND, Language) }
        });

    }

    private string GetContentType(string fileName)
    {

        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
        return contentType ?? "application/octet-stream";
    }


}