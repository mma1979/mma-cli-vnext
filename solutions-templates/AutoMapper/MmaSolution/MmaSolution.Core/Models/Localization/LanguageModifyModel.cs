using System.Collections.Generic;

namespace MmaSolution.Core.Models.Localization
{
    public partial class LanguageModifyModel
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public ICollection<ResourceModifyModel> Resources { get; set; }
    }
}