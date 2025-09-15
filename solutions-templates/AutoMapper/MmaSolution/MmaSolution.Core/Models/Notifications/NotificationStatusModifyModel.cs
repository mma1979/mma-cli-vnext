namespace MmaSolution.Core.Models.Notifications
{
    public partial class NotificationStatusModifyModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Hash { get; set; }
        public int Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}