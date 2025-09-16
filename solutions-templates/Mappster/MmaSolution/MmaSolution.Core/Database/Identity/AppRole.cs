namespace MmaSolution.Core.Database.Identity;

public class AppRole : IdentityRole<Guid>, IAuditEntity
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool? IsDeleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
    public virtual ICollection<AppUserRole> AppUserRoles { get; set; }
    public virtual ICollection<AppRoleClaim> AppRoleClaims { get; set; }
    public virtual ICollection<AppAccessControlEntry> AccessControlEntries { get; set; }
    public int Hash
    {
        get
        {
            return GetHashCode();
        }
    }



    private AppRole()
    {
        AppUserRoles = new HashSet<AppUserRole>();
        AppRoleClaims = new HashSet<AppRoleClaim>();
    }
    public AppRole(AppRoleModifyModel model)
    {
        Id = model.Id == Guid.Empty ? Guid.CreateVersion7() : model.Id;
        Name = model.Name;
        NormalizedName = model.Name.ToUpper();
        ConcurrencyStamp = DateTime.UtcNow.ToLinuxTime().ToString();
        CreatedDate = DateTime.UtcNow;
        CreatedBy = model.CreatedBy;

    }

    public AppRole Update(AppRoleModifyModel model)
    {
        Name = model.Name;
        NormalizedName = model.Name.ToUpper();
        ConcurrencyStamp = DateTime.UtcNow.ToLinuxTime().ToString();
        return this;
    }

    public AppRole Delete()
    {
        IsDeleted = true;
        DeletedDate = DateTime.UtcNow;
        return this;
    }

    public AppRole(string rolename)
    {
        Name = rolename;
    }

    public AppRole AddClaim(AppRoleClaim claim)
    {
        AppRoleClaims ??= new HashSet<AppRoleClaim>();
        AppRoleClaims.Add(claim);
        return this;
    }

    public AppRole AddUserRole(Guid userId)
    {
        AppUserRoles ??= new HashSet<AppUserRole>();
        AppUserRoles.Add(new AppUserRole { UserId = userId });
        return this;
    }

    public AppRole AddAcl(AppAccessControlEntry adminAcl)
    {
        AccessControlEntries ??= [];
        AccessControlEntries.Add(adminAcl);
        return this;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public override bool Equals(object obj)
    {
        return obj is AppRole other &&
            Name == other.Name;
    }


}
