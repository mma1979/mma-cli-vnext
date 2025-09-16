namespace MmaSolution.AppApi.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1.0)]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class AttachmentController : ControllerBase
{
    private readonly AttachmentsService _service;

    public string AttachmentPath { get; }
    public string DefaultImgName => "no-image.png";

    public AttachmentController(AttachmentsService service, IHostEnvironment env)
    {
        _service = service;
        AttachmentPath = Path.Combine(env.ContentRootPath, "wwwroot", "files");
    }

    // POST: api/Attachment/upload
    [HttpPost("upload"), DisableRequestSizeLimit]
    [RequiredPermission("Create")]
    public async Task<IActionResult> Upload([FromForm] IFormFile myFile)
    {
        var res = await _service.Upload(myFile);

        return Ok(res);
    }

    // POST: api/Attachment/UploadFile
    [HttpPost("UploadFile")]
    [RequiredPermission("Create")]
    public async Task<IActionResult> UploadFile(FileBase64 fileAttachment)
    {
        var res = await _service.SaveBase64(fileAttachment);

        return Ok(res);

    }

    // POST: api/Attachment/CKUpload
    [HttpPost("CKUpload")]
    [RequiredPermission("Create")]
    public async Task<IActionResult> CKUpload()
    {

        IFormFile fileAttachment = Request.Form.Files[0];
        string uri = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;

        var res = await _service.CKUpload(fileAttachment, uri);

        return Ok(res);

    }

    // POST: api/Attachment/GetCKBrowse
    [HttpGet("GetCKBrowse")]
    [RequiredPermission("Read")]
    public IActionResult  GetCKBrowse()
    {
        var list = _service.GetCKBrowse();

        return Ok(list);

    }

    // GET: api/Attachment/Files/2
    [HttpGet("Files/{id}")]
    [RequiredPermission("Read")]
    public IActionResult GetFiles(int id)
    {
        var attachment = _service.Find(id);
        var filePath = attachment == null ? DefaultImgName : attachment.FilePath;
        var contentType = attachment == null ?
            GetContentType(filePath) :
            attachment.ContentType ?? GetContentType(filePath);
        var file = System.IO.File.ReadAllBytes(Path.Combine(AttachmentPath, filePath));

        return File(file, contentType);

    }

    // GET: api/Attachment/Files/Images/Default
    [HttpGet("Files/Images/{*imgName}")]
    [RequiredPermission("Read")]
    public async Task<IActionResult> GetImageByNameAsync(string imgName)
    {
        if (string.IsNullOrEmpty(imgName) || imgName == "null")
        {
            imgName = "default";
        }
        var filePath = imgName.ToLower() == "default" ? DefaultImgName : imgName;
        var contentType = GetContentType(filePath);
        var file = await System.IO.File.ReadAllBytesAsync(Path.Combine(AttachmentPath, filePath));

        return File(file, contentType);

    }

    private string GetContentType(string fileName)
    {

        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
        return contentType ?? "application/octet-stream";
    }

}
