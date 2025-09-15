namespace MmaSolution.Core.Models.Attachment
{
    public class FileBase64
    {
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
    }
}
