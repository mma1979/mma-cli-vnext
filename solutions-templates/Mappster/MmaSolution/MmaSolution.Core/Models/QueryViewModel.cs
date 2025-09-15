namespace MmaSolution.Core.Models
{
    public class QueryViewModel
    {
        public string Language { get; set; } = "ar";
        public Guid? UserId { get; set; }
        public string Order { get; set; }
        public string Filter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long Timestamp { get; set; } = DateTime.UtcNow.ToLinuxTime();

        public bool ShowAll { get; set; } = false;

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, Order, Filter, PageNumber, PageSize, Language, ShowAll);
        }

        public override bool Equals(object obj)
        {
            return obj is QueryViewModel other &&
                other.UserId == UserId &&
                other.Order == Order &&
                  other.Filter == Filter &&
                  other.PageNumber == PageNumber &&
                  other.PageSize == PageSize &&
                  other.Language == Language &&
                  other.ShowAll == ShowAll;
        }
    }
}
