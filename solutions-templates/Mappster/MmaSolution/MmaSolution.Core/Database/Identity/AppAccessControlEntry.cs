namespace MmaSolution.Core.Database.Identity;

public class AppAccessControlEntry:BaseEntity<Guid>
{
    public string ResourcePattern { get; set; }
    public string PermissionPattern { get; set; }
    public Guid? FeatureId { get; set; }  
    public virtual AppFeature Feature { get; set; } 
    public virtual ICollection<AppRole> AppRoles { get; set; }
    public virtual ICollection<AppUser> AppUsers { get; set; }
    public virtual AppResource AppResource { get; set; }
    public Guid? ResourceId { get; set; }
}


