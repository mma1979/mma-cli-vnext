namespace MmaSolution.AppApi.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    [LoggingFilter]
    public class BaseController : ControllerBase
    {
        private Services.Translator _translator;
        public BaseController(Services.Translator translator)
        {
            _translator = translator;
        }


        protected string Language
        {
            get
            {
                var lang = string.IsNullOrWhiteSpace(HttpContext.Request.Headers["Accept-Language"]) ?
                    "en" : HttpContext.Request.Headers["Accept-Language"].ToString();
                return lang;

            }
        }

        protected ResultViewModel<T> HandleHttpException<T>(HttpException exception)
        {
            var result = new ResultViewModel<T>
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = (JsonConvert.DeserializeObject<List<string>>(exception.Message) ?? new List<string>())
                    .Select(m => _translator.Translate(m, Language)).ToList()
            };

            return result;
        }

        protected AcknowledgeViewModel HandleHttpException(HttpException exception)
        {
            var result = new AcknowledgeViewModel
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = (JsonConvert.DeserializeObject<List<string>>(exception.Message) ?? new List<string>())
                    .Select(m => _translator.Translate(m, Language)).ToList()
            };

            return result;
        }

        protected IActionResult RequestCanceled()
        {

            return BadRequest(new
            {
                IsSuccess = false,
                Messages = new[] { _translator.Translate(ResourcesKeys.REQUEST_CANCELED, Language) }
            });

        }
    }
}
