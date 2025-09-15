namespace MmaSolution.Core.Database.Identity;

public class AppFeatureFlag:BaseEntity<Guid>
{
    public Guid FeatureId { get; set; }
    public Guid? ScopeIdentifier { get; set; }
    public bool IsEnabled { get; set; }
    public virtual AppFeature Feature { get; set; } 
}
