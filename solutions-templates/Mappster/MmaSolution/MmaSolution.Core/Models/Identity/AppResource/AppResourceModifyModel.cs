namespace MmaSolution.Core.Models.Identity.AppResource
{
    public partial class AppResourceModifyModel
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public ResourceTypes ResourceType { get; set; }
        public ICollection<AppAccessControlEntryModifyModel> AccessControlEntries { get; set; }
        public Guid Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool ProbgateForAllUsers { get; set; } = true;
        public string ProbgatePermissionPattern { get; set; } = "Read";
    }
}