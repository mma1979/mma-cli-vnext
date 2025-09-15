namespace MmaSolution.Core.Models.Identity.AppAccessControlEntry
{
    public partial class AppAccessControlEntryReadModel
    {
        public string ResourcePattern { get; set; }
        public string PermissionPattern { get; set; }
        public Guid? FeatureId { get; set; }
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