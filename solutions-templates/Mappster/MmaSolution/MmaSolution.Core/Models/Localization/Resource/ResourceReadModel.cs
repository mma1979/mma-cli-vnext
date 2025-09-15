namespace MmaSolution.Core.Models.Localization.Resource
{
    public partial class ResourceReadModel
    {
        public long Id { get; set; }
        public int LanguageId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}