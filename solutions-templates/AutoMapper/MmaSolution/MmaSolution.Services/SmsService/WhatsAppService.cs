
namespace MmaSolution.Services.SmsService;

public class WhatsAppService: ISmsService
{
    private readonly IConfiguration _configuration;
    private readonly RestHelper _restHelper;
    public WhatsAppService(IConfiguration configuration, RestHelper restHelper)
    {
        _configuration = configuration;
        _restHelper = restHelper;
        _restHelper.BaseUrl = _configuration["Waha:BaseUrl"];
    }

    public async Task Send(string to, string message)
    {
        var endpoint = _configuration["Waha:SendEndPoint"];
        var body = new
        {
            chatId = $"{to}@c.us",
            text = message,
            linkPreview = true,
            linkPreviewHighQuality = false,
            session = "default"
        };

        _ = await _restHelper.Post<object>(endpoint, body);

    }
}
