namespace MmaSolution.Core.Database.Tables
{
    public class Attachment : BaseEntity<Guid>
    {
        public string AttachmentName { get; private set; }
        public string FilePath { get; private set; }
        public decimal? FileSize { get; private set; }
        public string ContentType { get; private set; }
        public string Key { get; private set; }
    }
}
