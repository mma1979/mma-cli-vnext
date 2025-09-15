namespace MmaSolution.Core.Database.Localization
{
    public class Resource
    {
        public Int64 Id { get; set; }
        public int LanguageId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public virtual Language Language { get; set; }
    }
}
