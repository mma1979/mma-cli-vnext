namespace MmaSolution.Core.Models.Identity.AppFeatureFlag
{
    public partial class AppFeatureFlagReadModel
    {
        public Guid FeatureId { get; set; }
        public Guid? ScopeIdentifier { get; set; }
        public bool IsEnabled { get; set; }
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