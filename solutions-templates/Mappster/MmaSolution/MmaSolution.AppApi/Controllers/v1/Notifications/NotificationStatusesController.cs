namespace MmaSolution.AppApi.Controllers.v1.Notifications
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class NotificationStatusesController : BaseController
    {
        private readonly NotificationStatusService _notificationStatusService;
        private readonly Services.Translator _translator;
        private readonly ILogger<NotificationStatusesController> _logger;

        public NotificationStatusesController(NotificationStatusService notificationStatusService, Services.Translator translator, ILogger<NotificationStatusesController> logger) : base(translator)
        {
            _notificationStatusService = notificationStatusService;
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
            var result = new ResultViewModel<List<NotificationStatusReadModel>>();
            try
            {
                _ = Guid.TryParse(User.FindFirstValue("Id"), out Guid userId);
                query.UserId = userId;
                var data = await _notificationStatusService.All(query);
                data.Messages = data.Messages.Select(m => _translator.Translate(m, Language)).ToList();
                if (data.IsSuccess)
                    return Ok(data);

                return BadRequest(data);

            }
            catch (HttpException ex)
            {
                _logger.LogError(ex.Message, query, ex);
                return BadRequest(HandleHttpException<List<NotificationStatusReadModel>>(ex));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                result.IsSuccess = false;
                result.StatusCode = 500;
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        [RequiredPermission("Read")]
        public async Task<IActionResult> GetOne(int id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }
            var result = new ResultViewModel<NotificationStatusModifyModel>();
            try
            {
                var data = await _notificationStatusService.Find(id);
                data.Messages = data.Messages.Select(m => _translator.Translate(m, Language)).ToList();

                if (data.IsSuccess)

                    return Ok(data);

                return BadRequest(data);

            }
            catch (HttpException ex)
            {

                _logger.LogError(ex.Message, ex);
                return BadRequest(HandleHttpException<NotificationStatusModifyModel>(ex));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                result.IsSuccess = false;
                result.StatusCode = 500;
                return BadRequest(result);
            }
        }



        [HttpPost]
        [RequiredPermission("Create")]
        public async Task<IActionResult> PostNotificationStatus([FromBody] NotificationStatusModifyModel model, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }
            var result = new AcknowledgeViewModel();
            try
            {
                var data = await _notificationStatusService.Add(model);
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
                result.IsSuccess = false;
                result.StatusCode = 500;
                return BadRequest(result);
            }
        }

        [HttpPut("{id}")]
        [RequiredPermission("Update")]
        public async Task<IActionResult> PutNotificationStatus(int id, [FromBody] NotificationStatusModifyModel model, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }

            var result = new AcknowledgeViewModel();
            try
            {
                if (model.Id != id)
                {
                    result.IsSuccess = false;
                    result.Messages.Add(_translator.Translate("InvalidData", Language));
                    return BadRequest(result);
                }
                var data = await _notificationStatusService.Update(model);
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
                result.IsSuccess = false;
                result.StatusCode = 500;
                return BadRequest(result);
            }
        }

        [HttpDelete("{id}")]
        [RequiredPermission("Update,Delete")]
        public async Task<IActionResult> DeleteNotificationStatus(int id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
                });
            }

            var result = new AcknowledgeViewModel();
            try
            {
                var data = await _notificationStatusService.Delete(id);
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
                result.IsSuccess = false;
                result.StatusCode = 500;
                return BadRequest(result);
            }
        }


    }

}