using MmaSolution.Core.Models.Identity.AppResource;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MmaSolution.Services.Account;

public class AccessControlService
{
    private readonly ILogger<AccessControlService> _logger;
    private readonly AuthenticationDbContext _authenticationDbContext;
    private readonly ApplicationDbContext _applicationDbContext;

    public AccessControlService(ILogger<AccessControlService> logger, AuthenticationDbContext authenticationDbContext, ApplicationDbContext applicationDbContext)
    {
        _logger = logger;
        _authenticationDbContext = authenticationDbContext;
        _applicationDbContext = applicationDbContext;
    }


    public async Task<AcknowledgeViewModel> AddReaource(AppResourceModifyModel model)
    {
        try
        {
            var exists = await _authenticationDbContext.AppResources
                .AnyAsync(r => r.Url == model.Url).ConfigureAwait(false);
            if (exists)
            {
                return AcknowledgeViewModel.Error(messages: [ResourcesKeys.RESOURCE_ALREADY_EXISTS]);
            }

            var resource = new AppResource(model);
            var entity = await _authenticationDbContext.AppResources.AddAsync(resource);

            var adminRole = await _authenticationDbContext.AppRoles
                .FirstOrDefaultAsync(r => r.Name == "Admin");

            var adminAcl = entity.Entity.AccessControlEntries
                .FirstOrDefault(a => a.PermissionPattern == "*");

            adminRole.AddAcl(adminAcl);



            _ = await _authenticationDbContext.SaveChangesAsync();

            if (model.ProbgateForAllUsers)
            {
                var acl = entity.Entity.AccessControlEntries
                .FirstOrDefault(a => a.PermissionPattern == model.ProbgatePermissionPattern);

                _ = await ProbgateResourceToUserAsync(_authenticationDbContext.Database.GetDbConnection(), acl.Id);
            }
            return new()
            {

                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_SAVE_SUCCESS },
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

    public record AssignResourceToUserRequest(string ResourceUrl, Guid UserId, string AccessPermissionPatterns);

    public async Task<AcknowledgeViewModel> AssignResourceToUser(AssignResourceToUserRequest request)
    {
        try
        {
            var resource = await _authenticationDbContext.AppResources
                .FirstOrDefaultAsync(r => r.Url == request.ResourceUrl).ConfigureAwait(false);
            if (resource == null)
            {
                _logger.LogInformation("Resource does not exist. Url: {Url}, UserId: {UserId}, AccessPermissionPatterns: {AccessPermissionPatterns}", request.ResourceUrl, request.UserId, request.AccessPermissionPatterns);
                return AcknowledgeViewModel.Error(messages: [ResourcesKeys.ITEM_NOT_FOUND]);
            }

            var user = _authenticationDbContext.AppUsers.Find(request.UserId);
            if (user == null)
            {
                _logger.LogInformation("User does not exist. Url: {Url}, UserId: {UserId}, AccessPermissionPatterns: {AccessPermissionPatterns}", request.ResourceUrl, request.UserId, request.AccessPermissionPatterns);
                return AcknowledgeViewModel.Error(messages: [ResourcesKeys.ITEM_NOT_FOUND]);
            }

            var acl = _authenticationDbContext.AccessControlEntries
                .Where(a => a.ResourceId == resource.Id && a.PermissionPattern == request.AccessPermissionPatterns)
                .FirstOrDefault();

            if (acl == null)
            {
                _logger.LogInformation("Acl does not exist. Url: {Url}, UserId: {UserId}, AccessPermissionPatterns: {AccessPermissionPatterns}", request.ResourceUrl, request.UserId, request.AccessPermissionPatterns);
                return AcknowledgeViewModel.Error(messages: [ResourcesKeys.ITEM_NOT_FOUND]);
            }

            var sql = """
                INSERT INTO AppUserAccessControlEntries (AccessControlEntriesId,AppUsersId)
                Values (@AccessControlEntriesId,@AppUsersId)
             """;

            using var connection = _authenticationDbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var saved = await connection.ExecuteAsync(sql, new { AccessControlEntriesId = acl.Id, AppUsersId =user.Id});
            connection.Close();


            return new()
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_SAVE_SUCCESS },
            };
        }
        catch (Exception)
        {

            throw;
        }
    }

    private static async Task<bool> ProbgateResourceToUserAsync(DbConnection connection, Guid aclId)
    {
        var sql = """
                INSERT INTO AppUserAccessControlEntries (AccessControlEntriesId,AppUsersId)
                    Select distinct @AccessControlEntriesId, u.Id UserId
                    from AppUsers u 
                    where  u.IsDeleted <> 0

            """;

        if (connection.State != ConnectionState.Open)
            connection.Open();

        var saved = await connection.ExecuteAsync(sql, new { AccessControlEntriesId = aclId.ToString() });
        connection.Close();

        return saved > 0;
    }


}