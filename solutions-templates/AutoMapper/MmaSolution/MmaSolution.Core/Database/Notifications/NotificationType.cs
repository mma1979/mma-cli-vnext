namespace MmaSolution.Core.Database.Notifications
{

    public class NotificationType : BaseEntity<int>
    {

        public string Name { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public int Hash
        {
            get
            {
                return GetHashCode();
            }
        }

        NotificationTypeValidator _Validator;
        private NotificationTypeValidator Validator
        {
            get
            {
                _Validator ??= new NotificationTypeValidator();
                return _Validator;
            }
        }


        private NotificationType()
        {

        }



        public NotificationType(NotificationTypeModifyModel model)
        {
            ValidationResult result = Validator.Validate(model);
            if (!result.IsValid)
            {
                var messages = result.Errors.Select(e => e.ErrorMessage);
                throw new HttpException(LoggingEvents.Constractor_ERROR, JsonConvert.SerializeObject(messages));
            }


            CreatedDate = DateTime.UtcNow;

        }

        public NotificationType Update(NotificationTypeModifyModel model)
        {
            ValidationResult result = Validator.Validate(model);
            if (!result.IsValid)
            {
                var messages = result.Errors.Select(e => e.ErrorMessage);
                throw new HttpException(LoggingEvents.Constractor_ERROR, JsonConvert.SerializeObject(messages));
            }



            ModifiedDate = DateTime.UtcNow;
            return this;
        }

        public NotificationType Delete()
        {
            IsDeleted = true;
            DeletedDate = DateTime.UtcNow;
            return this;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public override bool Equals(object obj)
        {
            return obj is NotificationType other &&
                Name == other.Name;
        }
    }
}