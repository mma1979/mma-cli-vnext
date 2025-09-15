namespace MmaSolution.Services
{
    public class AttachmentsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public string WebRoot { get; }
        public AttachmentsService(ApplicationDbContext context, IHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            WebRoot = Path.Combine(env.ContentRootPath, "wwwroot", "files");
        }


        public AttachmentDto Find(int id)
        {
            var entity = _context.Attachments.Find(id);
            return _mapper.Map<AttachmentDto>(entity);
        }

        #region Upload

        public async Task<string> SaveBase64(FileBase64 formFile)
        {
            if (formFile.Length > 0)
            {
                var fullFileName = formFile.FileName + Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                fullFileName = fullFileName.Replace(" ", "_");
                string folderPath = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                Directory.CreateDirectory(Path.Combine(WebRoot, folderPath));
                var strdata = formFile.ContentDisposition.Substring(formFile.ContentDisposition.IndexOf(",", StringComparison.Ordinal) + 1);
                await File.WriteAllBytesAsync(Path.Combine(WebRoot, folderPath, fullFileName), Convert.FromBase64String(strdata));
                return Path.Combine(folderPath, fullFileName).Replace("\\", "/");
            }
            return "";
        }

        public List<Tuple<string, string, string, DateTime>> GetCKBrowse()
        {
            List<Tuple<string, string, string, DateTime>> fileList = new List<Tuple<string, string, string, DateTime>>();

            var files = Directory.GetFiles(Path.Combine(WebRoot, "CKFiles")).Select(x => x.Replace(x, "CKFiles/" + Path.GetFileName(x))).ToList();
            files.ForEach(x =>
            {
                var loadDate = File.GetLastWriteTime(Path.Combine(WebRoot, x));
                fileList.Add(new Tuple<string, string, string, DateTime>(x, loadDate.ToLongDateString(), Path.GetFileName(x), loadDate));

                // x = Path.Combine("CKFiles", Path.GetFileName(x));
            });

            fileList = fileList.OrderByDescending(x => x.Item4).ToList();
            return fileList;
        }

        public async Task<object> CKUpload(IFormFile file, string uri)
        {
            var path = Path.Combine(WebRoot, "CKFiles",
                  file.FileName);

            await using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream);
            }
            return new
            {
                uploaded = 1,
                fileName = file.FileName,
                url = uri + "/CKFiles/" + file.FileName
            };
        }

        public async Task<string> Upload(IFormFile formFile)
        {
            if (formFile.Length <= 0) return "";

            var fullFileName = formFile.FileName + Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            fullFileName = fullFileName.Replace(" ", "_");
            string folderPath = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            Directory.CreateDirectory(Path.Combine(WebRoot, folderPath));
            await using (var stream = new FileStream(Path.Combine(WebRoot, folderPath, fullFileName), FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return Path.Combine(folderPath, fullFileName).Replace("\\", "/");
        }

        #endregion

    }
}
