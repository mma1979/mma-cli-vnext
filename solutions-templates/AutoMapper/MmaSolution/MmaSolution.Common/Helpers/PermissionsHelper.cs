using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MmaSolution.Common.Extensions;
using MmaSolution.Common.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MmaSolution.Common.Helpers
{
    public class PermissionsHelper
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly RestHelper _restHelper;
        public PermissionsHelper(IOptions<AppSettings> options, IHttpContextAccessor contextAccessor, RestHelper restHelper)
        {
            _appSettings = options.Value;
            _contextAccessor = contextAccessor;
            _restHelper = restHelper;
        }


        public async Task<T> PagePermissions<T>(string pageCode)
        {
            var token = _contextAccessor.HttpContext.Request.Cookies["Token"];
            var handler = new JwtSecurityTokenHandler();
            var tokenInfo = handler.ReadJwtToken(token);
            var userId = tokenInfo.Claims.FirstOrDefault(c => c.Type == "Id")?.Value.ToNullableInt();

            var res = await _restHelper.Get<T>(_appSettings.APIEndPoint,
               $"api/Permissions/{userId}?pageCode={pageCode}").ConfigureAwait(false);

            return res;
        }
    }


}
