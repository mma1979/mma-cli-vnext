namespace MmaSolution.Core.Database.Identity;

public partial class AppUser : IdentityUser<Guid>, IAuditEntity
{

    public string FullName { get; private set; }
    public string PictureUrl { get; private set; }
    public TwoFactorMethods TwoFactorMethod { get; private set; }

    public Guid? CreatedBy { get;  set; }
    public DateTime? CreatedDate { get;  set; }
    public Guid? ModifiedBy { get;  set; }
    public DateTime? ModifiedDate { get;  set; }
    public bool? IsDeleted { get;  set; }
    public Guid? DeletedBy { get;  set; }
    public DateTime? DeletedDate { get;  set; }

    public virtual ICollection<AppUserRole> UserRoles { get; private set; }
    public virtual ICollection<AppUserToken> UserTokens { get; private set; }
    public virtual ICollection<AppRefreshToken> RefreshTokens { get; private set; }
    public virtual ICollection<AppAccessControlEntry> AccessControlEntries { get; set; }


    #region Actions
    private AppUser()
    {
        UserRoles = new HashSet<AppUserRole>();
        UserTokens = new HashSet<AppUserToken>();
        RefreshTokens = new HashSet<AppRefreshToken>();

    }

    public AppUser(AppUserModifyModel model)
    {
        FullName = model.FullName;
        Email = model.Email;
        NormalizedEmail = model.Email.ToUpper();
        UserName = model.UserName;
        NormalizedUserName = model.UserName.ToUpper();
        PhoneNumber = model.PhoneNumber;
        SecurityStamp = Guid.NewGuid().ToString();
        TwoFactorEnabled = true;
        TwoFactorMethod = Enums.TwoFactorMethods.Email;
        PasswordHash = model.PasswordHash;
    }

    public AppUser Update(AppUserModifyModel model)
    {
        FullName = model.FullName;
        Email = model.Email;
        NormalizedEmail = model.Email.ToUpper();
        UserName = model.UserName;
        NormalizedUserName = model.UserName.ToUpper();
        PhoneNumber = model.PhoneNumber;
        SecurityStamp = Guid.NewGuid().ToString();
        TwoFactorEnabled = true;
        TwoFactorMethod = Enums.TwoFactorMethods.Email;
        PasswordHash = model.PasswordHash;

        return this;
    }
    public static AppUser TempUser(AppUserModifyModel model)
    {
        return new AppUser
        {
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Id = model.Id,
        };
    }

    public AppUser SetRole(AppRoleModifyModel role)
    {
        UserRoles = new HashSet<AppUserRole>
        {
            new AppUserRole{RoleId=role.Id}
        };
        

        return this;
    }

    public AppUser AddUserRoles(HashSet<AppUserRoleModifyModel> userRoles)
    {
        UserRoles = userRoles.Select(r => new AppUserRole { RoleId = r.RoleId }).ToHashSet();
        
        return this;
    }


    public AppUser AddUserToken(AppUserTokenModifyModel tokenDto)
    {
        UserTokens ??= new HashSet<AppUserToken>();
        UserTokens.Add(new AppUserToken(tokenDto));
        return this;
    }

   
    public AppUser ResetPassword(string passwordHash)
    {
      PasswordHash = passwordHash;
        return this;
    }

    public AppUser Delete()
    {
        IsDeleted = true;
        DeletedDate = DateTime.UtcNow;
        return this;
    }

    public AppUser Restore()
    {
        IsDeleted = false;
        DeletedDate = null;
        return this;
    }

    public AppUser ConfirmPhoneNumber()
    {
        PhoneNumberConfirmed = true;
        ModifiedDate = DateTime.UtcNow;
        return this;
    }

    public AppUser ChangePicture(string pictureUrl)
    {
        PictureUrl = pictureUrl;
        ModifiedDate = DateTime.UtcNow;
        return this;
    }
    #endregion

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, UserName, Email, PhoneNumber, FullName);
    }
    public override bool Equals(object obj)
    {
        return obj is AppUser other &&
            Id == other.Id &&
            UserName == other.UserName &&
            Email == other.Email &&
            PhoneNumber == other.PhoneNumber &&
            FullName == other.FullName;
    }
}
