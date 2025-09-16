using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmaSolution.Core.Models.Attachment;

public class FileMetadataResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public bool IsPublic { get; set; }
    public string PublicUrl { get; set; }
    public DateTime UploadedAt { get; set; }
}

public class UpdateFilePublicStatusRequest
{
    public bool IsPublic { get; set; }
}