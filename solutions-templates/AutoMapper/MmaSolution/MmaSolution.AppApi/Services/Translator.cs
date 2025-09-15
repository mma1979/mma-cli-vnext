namespace MmaSolution.AppApi.Services;

public class Translator
{
    private readonly LocalizationSerivce _service;

    public Translator(LocalizationSerivce service)
    {
        _service = service;
    }

    public string Translate(string key, string lang = LanguageCode.Arabic)
    {
        try
        {
            var res = _service.Translate(key, lang);

            return res;
        }
        catch (Exception)
        {

            return key;
        }
    }

    public async Task<string> TranslateAsync(string key, string lang= LanguageCode.Arabic)
    {
        try
        {
          var res = await _service.TranslateAsync(key, lang);

            return res;
        }
        catch (Exception)
        {

            return key;
        }
    }

    public string[] Translate(string[] keys, string lang = LanguageCode.Arabic)
    {
        try
        {
            var res = _service.TranslateAsync(keys, lang).Result;

            return res;
        }
        catch (Exception)
        {
            return keys;
        }
    }

    public async Task<string[]> TranslateAsync(string[] keys, string lang = LanguageCode.Arabic)
    {
        try
        {
            var res = await _service.TranslateAsync(keys, lang);

            return res;
        }
        catch (Exception)
        {
            return keys;
        }
    }

    public Dictionary<string, string> TranslateDict(string[] keys, string lang = LanguageCode.Arabic)
    {
        try
        {
            var res = _service.TranslateDictAsync(keys, lang).Result;

            return res;
        }
        catch (Exception)
        {
            return keys.ToDictionary(e => e, e => e);
        }
    }

    public async Task<Dictionary<string, string>> TranslateDictAsync(string[] keys, string lang = LanguageCode.Arabic)
    {
        try
        {
            var res = await _service.TranslateDictAsync(keys, lang);

            return res;
        }
        catch (Exception)
        {
            return keys.ToDictionary(e => e, e => e);
        }
    }
    public HtmlString TranslateHtml(string key, string lang = LanguageCode.Arabic)
    {
        try
        {
            var res =  _service.Translate(key, lang);
            return new HtmlString(res.ToString());
        }
        catch (Exception)
        {

            return new HtmlString(key);
        }
    }

    public async Task<HtmlString> TranslateHtmlAsync(string key, string  lang = LanguageCode.Arabic)
    {
        try
        {
            var res = await _service.TranslateAsync(key, lang);
            return new HtmlString(res.ToString());
        }
        catch (Exception)
        {

            return new HtmlString(key);
        }
    }
}
