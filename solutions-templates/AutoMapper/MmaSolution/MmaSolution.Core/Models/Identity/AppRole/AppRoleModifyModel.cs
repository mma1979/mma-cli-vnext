namespace MmaSolution.Core.Models.Identity.AppRole
{
    public partial class AppRoleModifyModel
    {
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ICollection<AppUserRoleModifyModel> AppUserRoles { get; set; }
        public ICollection<AppRoleClaimModifyModel> AppRoleClaims { get; set; }
        public ICollection<AppAccessControlEntryModifyModel> AccessControlEntries { get; set; }
        public int Hash { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}