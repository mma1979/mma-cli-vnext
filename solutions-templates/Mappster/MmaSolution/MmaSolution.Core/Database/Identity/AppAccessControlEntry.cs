namespace MmaSolution.Core.Database.Identity;

public class AppAccessControlEntry : BaseEntity<Guid>
{
    public string ResourcePattern { get; set; }
    public string PermissionPattern { get; set; }
    public Guid? FeatureId { get; set; }
    public virtual AppFeature Feature { get; set; }
    public virtual ICollection<AppRole> AppRoles { get; set; }
    public virtual ICollection<AppUser> AppUsers { get; set; }
    public virtual AppResource AppResource { get; set; }
    public Guid? ResourceId { get; set; }

    private AppAccessControlEntry()
    {
        AppRoles = new HashSet<AppRole>();
        AppUsers = new HashSet<AppUser>();
    }

    public AppAccessControlEntry(AppAccessControlEntryModifyModel model)
    {
        Id = model.Id == Guid.Empty ? Guid.CreateVersion7() : model.Id;
        ResourceId = model.ResourceId;
        ResourcePattern = model.ResourcePattern;
        PermissionPattern = model.PermissionPattern;
        FeatureId = model.FeatureId;

    }


    public AppAccessControlEntry Update(AppAccessControlEntryModifyModel model)
    {
        ResourceId = model.ResourceId;
        ResourcePattern = model.ResourcePattern;
        PermissionPattern = model.PermissionPattern;
        FeatureId = model.FeatureId;

        return this;
    }


    public AppAccessControlEntry Delete()
    {
        IsDeleted = true;

        return this;
    }

}


