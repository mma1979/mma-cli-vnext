using MmaSolution.AppApi.Filters;
using MmaSolution.Common;
using MmaSolution.Core.Models;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using MmaSolution.Core.Consts;
using MmaSolution.AppApi.Services;

namespace MmaSolution.AppApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [LoggingFilter]
    public class BaseController : ControllerBase
    {
        private Translator _translator;
        public BaseController(Translator translator)
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
