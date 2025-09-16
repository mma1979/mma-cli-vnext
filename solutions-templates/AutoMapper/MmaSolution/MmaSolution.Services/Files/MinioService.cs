using Minio.DataModel.Args;
using Minio;

namespace MmaSolution.Services.Files;

public class MinioService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AuthenticationDbContext _authContext;
    private readonly ILogger<MinioService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMinioClient _minioClient;
    private string _bucketName;

    public MinioService(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<MinioService> logger,
        AuthenticationDbContext authContext)
    {
        _dbContext = dbContext;
        _configuration = configuration;

        var minioConfig = configuration.GetSection("MinioSettings");
        _bucketName = minioConfig["BucketName"]!;

        _minioClient = new MinioClient()
            .WithEndpoint(minioConfig["Endpoint"])
            .WithCredentials(minioConfig["AccessKey"], minioConfig["SecretKey"])
            .WithSSL(minioConfig.GetValue<bool>("UseSSL"))
            .Build();

        EnsureBucketExists().Wait();
        _logger = logger;
        _authContext = authContext;
    }

    public MinioService SetBucketName(string bucketName)
    {
        _bucketName = bucketName;
        return this;
    }

    public async Task EnsureBucketExists()
    {
        var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
        }
    }

    #region User Profile

    public async Task<FileMetadataResponse> UploadProfileImage(Guid userId, UploadFileRequest request)
    {

        var fileExt = Path.GetExtension(request.File.FileName).TrimStart('.');
        var objectName = $"{userId}.{fileExt}";

        using var stream = request.File.OpenReadStream();

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(request.File.ContentType);

        await _minioClient.PutObjectAsync(putObjectArgs);




        return new FileMetadataResponse
        {
            Id = userId
        };
    }

    public async Task<Stream> DownloadFile(string objectName)
    {

        var memoryStream = new MemoryStream();

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));

        await _minioClient.GetObjectAsync(getObjectArgs);

        memoryStream.Position = 0;
        return memoryStream;
    }

    #endregion

    public async Task<string> GetPublicUrl(string bucketName, string objectName)
    {
        var expiry = TimeSpan.FromDays(7); // Use a longer expiry for "public" links

        var presignedArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry((int)expiry.TotalSeconds);

        return await _minioClient.PresignedGetObjectAsync(presignedArgs);
    }

}