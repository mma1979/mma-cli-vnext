namespace MmaSolution.Core.Database.Notifications
{
    public class Notification : BaseEntity<Guid>
    {
        NotificationValidator _Validator;
        private NotificationValidator Validator
        {
            get
            {
                _Validator ??= new NotificationValidator();
                return _Validator;
            }
        }

        protected Notification() { }

        public Guid UserId { get; protected set; }
        public string Message { get; protected set; }
        public virtual AppUser User { get; protected set; }

        public NotificationStatuses NotificationStatus { get; protected set; }
        public NotificationTypes NotificationType { get; protected set; }
        public bool? IsRead { get; set; }
        public NotificationPeriorities Periority { get; set; }
        public DateTime? ExpireTime { get; set; }


        public Notification(NotificationModifyModel model)
        {
            ValidationResult result = Validator.Validate(model);
            if (!result.IsValid)
            {
                var messages = result.Errors.Select(e => e.ErrorMessage);
                throw new HttpException(LoggingEvents.Constractor_ERROR, JsonConvert.SerializeObject(messages));
            }

            UserId = model.UserId;
            Message = model.Message;  
            NotificationStatus = model.NotificationStatus;
            NotificationType = model.NotificationType;
            IsRead = model.IsRead;
            Periority = model.Periority;
            ExpireTime = model.ExpireTime;

            CreatedDate = DateTime.UtcNow;

        }

        public Notification Update(NotificationModifyModel model)
        {
            ValidationResult result = Validator.Validate(model);
            if (!result.IsValid)
            {
                var messages = result.Errors.Select(e => e.ErrorMessage);
                throw new HttpException(LoggingEvents.Constractor_ERROR, JsonConvert.SerializeObject(messages));
            }


            UserId = model.UserId;
            Message = model.Message;
            NotificationStatus = model.NotificationStatus;
            NotificationType = model.NotificationType;
            IsRead = model.IsRead;
            Periority = model.Periority;
            ExpireTime = model.ExpireTime;

            ModifiedDate = DateTime.UtcNow;
            return this;
        }

        public Notification Delete()
        {
            IsDeleted = true;
            DeletedDate = DateTime.UtcNow;
            return this;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, Message, NotificationType, NotificationStatus, CreatedDate);
        }

        public override bool Equals(object obj)
        {
            return obj is Notification other &&
                other.UserId == UserId &&
                other.Message == Message &&
                other.NotificationType == NotificationType &&
                other.NotificationStatus == NotificationStatus &&
                other.NotificationType == NotificationType &&
                other.CreatedDate == CreatedDate;
        }


    }

    public class EmailNotification : Notification
    {

    }

    public class SmsNotification : Notification
    {

    }

    public class PushNotification : Notification
    {

    }


}
