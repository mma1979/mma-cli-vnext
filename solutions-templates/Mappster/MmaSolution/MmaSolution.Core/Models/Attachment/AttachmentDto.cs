namespace MmaSolution.Core.Models.Attachment
{
    public partial class AttachmentDto
    {
        public string AttachmentName { get; set; }
        public string FilePath { get; set; }
        public decimal? FileSize { get; set; }
        public string ContentType { get; set; }
        public long KeyId { get; set; }
        public long Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}