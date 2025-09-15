namespace MmaSolution.Core.Models.Identity.AppUserRole
{
    public partial class AppUserRoleModifyModel
    {
        public AppUserModifyModel AppUser { get; set; }
        public AppRoleModifyModel AppRole { get; set; }
        public int Hash { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}