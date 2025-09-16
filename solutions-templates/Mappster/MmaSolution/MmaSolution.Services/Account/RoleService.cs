namespace MmaSolution.Services.Account;

public class RoleService
{
    private readonly AuthenticationDbContext _context;
    private readonly ILogger<RoleService> _logger;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;

    public RoleService(AuthenticationDbContext context, ILogger<RoleService> logger, ICacheService cacheService, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<ResultViewModel<List<AppRoleReadModel>>> All(QueryViewModel query)
    {
        var cacheKey = $"AppRole:GetAll_{query.GetHashCode()}".GetHashCode().ToString();
        try
        {
            var cached = _cacheService.Get<ResultViewModel<List<AppRoleReadModel>>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }


            var data = query.ShowAll ?
            _context.Roles.IgnoreQueryFilters().AsQueryable() :
            _context.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(query.Filter))
            {
                data = data.Where(query.Filter);
            }

            query.Order = string.IsNullOrEmpty(query.Order) ? "Id Desc" : query.Order;
            data = data.OrderBy(query.Order);

            var page = query.PageNumber <= 0 ? data :
                       data.Skip((query.PageNumber - 1) * query.PageSize)
                       .Take(query.PageSize);

            var count = await data.CountAsync();
            var list = await page.ToListAsync();

            var result = new ResultViewModel<List<AppRoleReadModel>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                Total = count,
                PageSize = query.PageSize,
                PageNumber = query.PageNumber,
                Filter = query.Filter,
                Data = _mapper.Map<List<AppRoleReadModel>>(list)
            };

            _cacheService.Set(cacheKey, result);

            return result;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
            return new ResultViewModel<List<AppRoleReadModel>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = { ResourcesKeys.DATA_READ_ERROR },
                Filter = query.Filter,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
            };
        }
    }

    public async Task<ResultViewModel<AppRoleModifyModel>> Find(Expression<Func<AppRole, bool>> predicate)
    {
        var cacheKey = $"AppRole:Find_{predicate.Body}".GetHashCode().ToString();
        try
        {
            var cached = _cacheService.Get<ResultViewModel<AppRoleModifyModel>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var data = await _context.Roles.SingleOrDefaultAsync(predicate);
            var result = new ResultViewModel<AppRoleModifyModel>
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                Data = _mapper.Map<AppRoleModifyModel>(data)
            };

            _cacheService.Set(cacheKey, result);

            return result;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
            return new ResultViewModel<AppRoleModifyModel>
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = { ResourcesKeys.DATA_LOAD_ERROR },
            };
        }
    }
    public async Task<ResultViewModel<AppRoleModifyModel>> Find(Guid id)
    {
        var cacheKey = $"AppRole:Find_{id}".GetHashCode().ToString();
        try
        {

            var cached = _cacheService.Get<ResultViewModel<AppRoleModifyModel>>($"AppRole:Find_{id}");

            if (cached != null)
            {
                return cached;
            }

            var data = await _context.Roles.FindAsync(id);
            var result = new ResultViewModel<AppRoleModifyModel>
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                Data = _mapper.Map<AppRoleModifyModel>(data)
            };

            _cacheService.Set(cacheKey, result);

            return result;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
            return new ResultViewModel<AppRoleModifyModel>
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = { ResourcesKeys.DATA_LOAD_ERROR },
            };
        }
    }


    public async Task<AcknowledgeViewModel> Add(AppRoleModifyModel model)
    {
        try
        {
            var Role = new AppRole(model);
            var entity = await _context.Roles.AddAsync(Role);
            _ = await _context.SaveChangesAsync();

            _cacheService.Clear("AppRole:");
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

    public async Task<AcknowledgeViewModel> Update(AppRoleModifyModel model)
    {
        try
        {
            var Role = await _context.Roles.FindAsync(model.Id);
            if (Role == null)
            {
                var exp = new KeyNotFoundException($"item number {model.Id} does not Exist");
                _logger.LogError(exp.Message, exp);
                return new()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.ITEM_NOT_FOUND },
                };
            }


            var entity = Role.Update(model);

            _ = await _context.SaveChangesAsync();

            _cacheService.Clear("AppRole:");

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

    public async Task<AcknowledgeViewModel> Delete(Guid id)
    {
        try
        {
            var Role = await _context.Roles.FindAsync(id);
            if (Role == null)
            {
                var exp = new KeyNotFoundException($"item number {id} does not Exist");
                _logger.LogError(exp.Message, exp);
                return new()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.ITEM_NOT_FOUND },
                };
            }
            var entity = Role.Delete();
            _context.Entry(entity).State = EntityState.Deleted;
            _ = await _context.SaveChangesAsync();

            _cacheService.Clear("AppRole:");

            return new()
            {
                IsSuccess = true,
                StatusCode = 200,
                Messages = { ResourcesKeys.DATA_REMOVE_SUCCESS }
            };

        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
            return new()
            {
                IsSuccess = false,
                StatusCode = 500,
                Messages = { ResourcesKeys.DATA_REMOVE_ERROR },
            };
        }
    }

}
