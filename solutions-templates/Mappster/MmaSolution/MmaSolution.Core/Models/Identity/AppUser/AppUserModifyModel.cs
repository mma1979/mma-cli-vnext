namespace MmaSolution.Core.Models.Identity.AppUser
{
    public partial class AppUserModifyModel
    {
        public string FullName { get; set; }
        public string PictureUrl { get; set; }
        public TwoFactorMethods TwoFactorMethod { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ICollection<AppUserRoleModifyModel> UserRoles { get; set; }
        public ICollection<AppUserTokenModifyModel> UserTokens { get; set; }
        public ICollection<AppRefreshTokenModifyModel> RefreshTokens { get; set; }
        public ICollection<AppAccessControlEntryModifyModel> AccessControlEntries { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }
}