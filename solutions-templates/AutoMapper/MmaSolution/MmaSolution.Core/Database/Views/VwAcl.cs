namespace MmaSolution.Core.Database.Views
{
    public class VwAcl
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
        public string ResourcePattern { get; set; }
        public string PermissionPattern { get; set; }
        public Guid? ResourceId { get; set; }
        public string ResourceUrl { get; set; }
        public FeatureScope? FeatureScope { get; set; }
        public bool? FeatureIsEnabled { get; set; }
        public Guid? FlagScopeIdentifier { get; set; }
        public bool? FlagIsEnabled { get; set; }
    }
}
