namespace MmaSolution.Core.Database.Localization
{
    public class Language
    {
        public Language()
        {
            Resources = new HashSet<Resource>();
        }
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }
    }
}
