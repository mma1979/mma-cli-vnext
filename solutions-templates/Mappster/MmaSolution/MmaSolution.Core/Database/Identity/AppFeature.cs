namespace MmaSolution.Core.Database.Identity;

public class AppFeature: BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsEnabled { get; set; }
    public FeatureScope Scope { get; set; }
    public virtual ICollection<AppFeatureFlag> FeatureFlags { get; set; }
    public virtual ICollection<AppAccessControlEntry> AccessControlEntries { get; set; }
}
