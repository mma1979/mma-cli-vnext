namespace MmaSolution.Core.Database.Notifications
{

    public class NotificationStatus : BaseEntity<int>
    {

        public string Name { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public int Hash { get
            {
                return GetHashCode();
            }
        }

        NotificationStatusValidator _Validator;
        private NotificationStatusValidator Validator
        {
            get
            {
                _Validator ??= new NotificationStatusValidator();
                return _Validator;
            }
        }


        private NotificationStatus()
        {

        }



        public NotificationStatus(NotificationStatusModifyModel model)
        {
            ValidationResult result = Validator.Validate(model);
            if (!result.IsValid)
            {
                var messages = result.Errors.Select(e => e.ErrorMessage);
                throw new HttpException(LoggingEvents.Constractor_ERROR, JsonConvert.SerializeObject(messages));
            }
            Name = model.Name;
            Description = model.Description;

            CreatedDate = DateTime.UtcNow;

        }

        public NotificationStatus Update(NotificationStatusModifyModel model)
        {
            ValidationResult result = Validator.Validate(model);
            if (!result.IsValid)
            {
                var messages = result.Errors.Select(e => e.ErrorMessage);
                throw new HttpException(LoggingEvents.Constractor_ERROR, JsonConvert.SerializeObject(messages));
            }

            Name = model.Name;
            Description = model.Description;

            ModifiedDate = DateTime.UtcNow;
            return this;
        }

        public NotificationStatus Delete()
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
            return obj is NotificationStatus other &&
                Name == other.Name;
        }
    }
}