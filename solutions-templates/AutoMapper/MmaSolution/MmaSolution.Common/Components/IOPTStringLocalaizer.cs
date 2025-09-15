namespace MmaSolution.Common.Components
{
    public class IOPTStringLocalaizer : IStringLocalizer
    {
        private readonly Translator _translator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        string language = "ar";
        public IOPTStringLocalaizer(Translator translator, IHttpContextAccessor httpContextAccessor)
        {
            _translator = translator;
            _httpContextAccessor = httpContextAccessor;
            language = _httpContextAccessor.HttpContext.Request.Cookies["lang"] ?? "ar";
        }

        public LocalizedString this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
              
                var value = _translator.Translate(key, language);
                return new LocalizedString(key, value);
            }
        }

        public LocalizedString this[string key, params object[] arguments]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                var args = !string.IsNullOrEmpty(language) ? language :
                   arguments.HasAny() ? arguments[0].ToString() : "ar";

                var value = _translator.Translate(key, args);
                return new LocalizedString(key, value);
            }
        }
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
