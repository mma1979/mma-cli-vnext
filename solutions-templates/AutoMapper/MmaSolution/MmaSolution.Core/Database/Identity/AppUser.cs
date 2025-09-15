namespace MmaSolution.Core.Database.Identity
{
    public partial class AppUser : IdentityUser<Guid>, IAuditEntity
    {

        public int Hash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Mobile { get; private set; }
        public string CountryCode { get; private set; }

        public TwoFactorMethods TwoFactorMethod { get; private set; }

        public Guid? CreatedBy { get;  set; }
        public DateTime? CreatedDate { get;  set; }
        public Guid? ModifiedBy { get;  set; }
        public DateTime? ModifiedDate { get;  set; }
        public bool? IsDeleted { get;  set; }
        public Guid? DeletedBy { get;  set; }
        public DateTime? DeletedDate { get;  set; }
        public MembershipTypes MembershipType { get; private set; }

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
            Email = model.Email;
            NormalizedEmail = model.Email.ToUpper();
            UserName = model.UserName;
            NormalizedUserName = model.UserName.ToUpper();
            FirstName = model.FirstName;
            LastName = model.LastName;
            CountryCode = model.CountryCode;
            PhoneNumber = model.PhoneNumber;
            SecurityStamp = Guid.NewGuid().ToString();
            TwoFactorEnabled = true;
            TwoFactorMethod = Enums.TwoFactorMethods.Email;
            MembershipType = model.MembershipType;
            Mobile = model.Mobile;
            PasswordHash = model.PasswordHash;
            
            Hash = GetHashCode();
        }

        public AppUser Update(AppUserModifyModel model)
        {
            Email = model.Email;
            NormalizedEmail = model.Email.ToUpper();
            UserName = model.UserName;
            NormalizedUserName = model.UserName.ToUpper();
            FirstName = model.FirstName;
            LastName = model.LastName;
            CountryCode = model.CountryCode;
            PhoneNumber = model.PhoneNumber;
            SecurityStamp = Guid.NewGuid().ToString();
            MembershipType = model.MembershipType;
            Mobile = model.Mobile;

            Hash = GetHashCode();

            return this;
        }

        public AppUser SetRole(AppRoleModifyModel role)
        {
            UserRoles = new HashSet<AppUserRole>
            {
                new AppUserRole{RoleId=role.Id}
            };
            

            Hash = GetHashCode();

            return this;
        }

        public AppUser AddUserRoles(HashSet<AppUserRoleModifyModel> userRoles)
        {
            UserRoles = userRoles.Select(r => new AppUserRole { RoleId = r.RoleId }).ToHashSet();
            
            Hash = GetHashCode();

            return this;
        }


        public AppUser AddUserToken(AppUserTokenModifyModel tokenDto)
        {
            UserTokens ??= new HashSet<AppUserToken>();
            UserTokens.Add(new AppUserToken(tokenDto));
            return this;
        }

        public AppUser SetMembership(MembershipTypes memberShip)
        {
            MembershipType = memberShip;

            ModifiedDate = DateTime.UtcNow;
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
        #endregion

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, UserName, Email, PhoneNumber, FirstName, LastName, Mobile, CountryCode);
        }
        public override bool Equals(object obj)
        {
            return obj is AppUser other &&
                Id == other.Id &&
                UserName == other.UserName &&
                Email == other.Email &&
                PhoneNumber == other.PhoneNumber &&
                FirstName == other.FirstName &&
                LastName == other.LastName &&
                Mobile == other.Mobile &&
                CountryCode == other.CountryCode;
        }
    }
}
