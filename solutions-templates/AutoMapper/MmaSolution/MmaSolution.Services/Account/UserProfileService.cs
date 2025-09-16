using MmaSolution.Core.Models.Identity.AppUser;
using MmaSolution.Services.Files;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmaSolution.Services.Account;

public class UserProfileService : IDisposable
{
    private readonly AuthenticationDbContext _context;
    private readonly ILogger<UserProfileService> _logger;
    private readonly ICacheService _cacheService;
    private readonly MinioService _minioService;
    private readonly IConfiguration _configuration;

    private readonly string CACHING_PREFIX = "UserProfile:";

    public UserProfileService(AuthenticationDbContext context, ILogger<UserProfileService> logger, ICacheService cacheService, MinioService minioService, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
        _minioService = minioService;
        _configuration = configuration;

        _minioService.SetBucketName("profile")
            .EnsureBucketExists().Wait();
    }





    public async Task<ResultViewModel<UserProfile>> Find(Expression<Func<AppUser, bool>> predicate)
    {
        var cacheKey = $"{CACHING_PREFIX}{predicate.Body.GetHashCode()}";
        try
        {
            var cached = _cacheService.Get<ResultViewModel<UserProfile>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var user = await _context.AppUsers.SingleOrDefaultAsync(predicate);

            if (user == null)
            {
                return new ResultViewModel<UserProfile>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Messages = { ResourcesKeys.ITEM_NOT_FOUND }
                };
            }

            var profile = new UserProfile
            {
                Email = user.Email,
                FullName = user.FullName,
                PictureUrl = user.PictureUrl,
            };
            var result = new ResultViewModel<UserProfile>
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                Data = profile
            };

            _cacheService.Set(cacheKey, result);

            return result;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
            return new()
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = { ResourcesKeys.DATA_LOAD_ERROR },
            };
        }
    }
    public async Task<ResultViewModel<UserProfile>> Find(Guid id)
    {
        var cacheKey = $"{CACHING_PREFIX}{id.GetHashCode()}";
        try
        {

            var cached = _cacheService.Get<ResultViewModel<UserProfile>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var user = await _context.AppUsers.FindAsync(id);



            if (user == null)
            {
                return new ResultViewModel<UserProfile>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Messages = { ResourcesKeys.ITEM_NOT_FOUND }
                };
            }

            var profile = new UserProfile
            {
                Email = user.Email,
                FullName = user.FullName,
                PictureUrl = user.PictureUrl,
            };
            var result = new ResultViewModel<UserProfile>
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                Data = profile
            };

            _cacheService.Set(cacheKey, result);

            return result;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
            return new()
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = { ResourcesKeys.DATA_LOAD_ERROR },
            };
        }
    }


    public async Task<AcknowledgeViewModel> UpdatePicture(Guid userId, IFormFile picture)
    {
        try
        {
            if (picture == null || picture.Length == 0)
            {
                return new AcknowledgeViewModel
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.INVALID_FILE }
                };
            }


            if (picture.Length > 5 * 1024 * 1024) // 5 MB limit
            {
                return new AcknowledgeViewModel
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.FILE_TOO_LARGE }
                };
            }

            if (!picture.ContentType.StartsWith("image/"))
            {
                return new AcknowledgeViewModel
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.INVALID_FILE }
                };
            }


            var user = await _context.AppUsers.FindAsync(userId);
            if (user == null)
            {
                var exp = new KeyNotFoundException($"item number {userId} does not Exist");
                _logger.LogError(exp.Message, exp);
                return new()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.ITEM_NOT_FOUND },
                };
            }


            var pictureFile = await _minioService.UploadProfileImage(userId, new() { File = picture });


            var fileExt = Path.GetExtension(picture.FileName).TrimStart('.');
            var objectName = $"{userId}.{fileExt}";

            user.ChangePicture($"{_configuration.GetValue<string>("AppSettings:APIEndPoint")}/api/v1/user-profile/images/{objectName}");

            _ = await _context.SaveChangesAsync();

            _cacheService.Clear("{CACHING_PREFIX}*");

            return new()
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_MODIFY_SUCCESS }
            };

        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
            return new()
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = { ResourcesKeys.DATA_SAVE_ERROR },
            };
        }
    }

    public async Task<byte[]> DownloadPicture(string objectName)
    {
        var file = await _minioService.DownloadFile(objectName);
        if (file == null)
        {
            _logger.LogError($"File with path {objectName} not found");
            return Array.Empty<byte>();
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    #region IDisposable Support
    public void Dispose(bool dispose)
    {
        if (dispose)
        {
            Dispose();

        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        GC.Collect();
    }







    #endregion

}