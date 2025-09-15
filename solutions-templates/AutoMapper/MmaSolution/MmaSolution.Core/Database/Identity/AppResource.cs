namespace MmaSolution.Core.Database.Identity
{
    public class AppResource : BaseEntity<Guid>
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public ResourceTypes ResourceType { get; set; }
        public virtual ICollection<AppAccessControlEntry> AccessControlEntries { get; set; }
    }
}
