namespace MmaSolution.Core.Models.Identity.AppAccessControlEntry
{
    public partial class AppAccessControlEntryModifyModel
    {
        public string ResourcePattern { get; set; }
        public string PermissionPattern { get; set; }
        public Guid? FeatureId { get; set; }
        public AppFeatureModifyModel Feature { get; set; }
        public ICollection<AppRoleModifyModel> AppRoles { get; set; }
        public ICollection<AppUserModifyModel> AppUsers { get; set; }
        public AppResourceModifyModel AppResource { get; set; }
        public Guid? ResourceId { get; set; }
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}