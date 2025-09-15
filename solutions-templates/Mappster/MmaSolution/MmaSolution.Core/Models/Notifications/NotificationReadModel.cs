namespace MmaSolution.Core.Models.Notifications
{
    public partial class NotificationReadModel
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public NotificationStatuses NotificationStatus { get; set; }
        public NotificationTypes NotificationType { get; set; }
        public bool? IsRead { get; set; }
        public NotificationPeriorities Periority { get; set; }
        public DateTime? ExpireTime { get; set; }
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