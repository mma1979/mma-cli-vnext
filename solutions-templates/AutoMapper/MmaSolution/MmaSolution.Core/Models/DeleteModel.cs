namespace MmaSolution.Core.Models
{
    public class DeleteModel
    {
        public bool IsDeleted { get; init; }
        public long? DeletedBy { get; init; }
        public DateTime? DeletedDate { get; init; }
    }
}
