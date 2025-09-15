namespace MmaSolution.Core.Database.Identity;

public class AppUserToken : IdentityUserToken<Guid>, IAuditEntity
{
    public string Token { get; private set; }
    public virtual AppUser AppUser { get; private set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get;  set; }
    public Guid? ModifiedBy { get;  set; }
    public DateTime? ModifiedDate { get;  set; }
    public bool? IsDeleted { get;  set; }
    public Guid? DeletedBy { get;  set; }
    public DateTime? DeletedDate { get;  set; }

    public string Identifier { get; set; }

    public AppUserToken()
    {

    }

    public AppUserToken(AppUserTokenModifyModel model)
    {
        Name = model.Name;
        LoginProvider = model.LoginProvider;
        Value = model.Value;
        Token = model.Token;
        UserId = model.UserId;
    }

}
