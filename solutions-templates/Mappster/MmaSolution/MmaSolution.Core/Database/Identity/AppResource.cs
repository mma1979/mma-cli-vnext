namespace MmaSolution.Core.Database.Identity;

public class AppResource : BaseEntity<Guid>
{
    public string Url { get; set; }
    public string Description { get; set; }
    public ResourceTypes ResourceType { get; set; }
    public virtual ICollection<AppAccessControlEntry> AccessControlEntries { get; set; }

    private AppResource()
    {
        AccessControlEntries = new HashSet<AppAccessControlEntry>();
    }

    public AppResource(AppResourceModifyModel model)
    {
        Id = model.Id == Guid.Empty ? Guid.CreateVersion7() : model.Id;
        Url = model.Url;
        Description = model.Description;
        ResourceType = model.ResourceType;
        AccessControlEntries = model.AccessControlEntries?.Select(a => new AppAccessControlEntry(a)).ToHashSet() ?? [];
    }

    public AppResource Update(AppResourceModifyModel model)
    {
        Url = model.Url;
        Description = model.Description;
        ResourceType = model.ResourceType;
        AccessControlEntries = model.AccessControlEntries?.Select(a => new AppAccessControlEntry(a)).ToHashSet() ?? [];

        return this;
    }

    public AppResource AddAccessControl(AppAccessControlEntryModifyModel model)
    {
        AccessControlEntries ??= [];
        AccessControlEntries.Add(new AppAccessControlEntry(model));
        return this;
    }

    public AppResource Delete()
    {
        IsDeleted = true;
        DeletedDate = DateTime.UtcNow;
        return this;
    }
}
