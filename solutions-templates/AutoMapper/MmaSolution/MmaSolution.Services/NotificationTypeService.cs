namespace MmaSolution.Services
{


    public class NotificationTypeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationTypeService> _logger;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public NotificationTypeService(ApplicationDbContext context, ILogger<NotificationTypeService> logger, ICacheService cacheService, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _cacheService = cacheService;
            _mapper = mapper;
        }



        public async Task<ResultViewModel<List<NotificationTypeReadModel>>> All(QueryViewModel query)
        {
            var cacheKey = $"NotificationType:GetAll_{query.GetHashCode()}".GetHashCode().ToString();
            try
            {
                var cached = _cacheService.Get<ResultViewModel<List<NotificationTypeReadModel>>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }


                var data = query.ShowAll ?
                _context.NotificationTypes.IgnoreQueryFilters().AsQueryable():
                _context.NotificationTypes.AsQueryable();
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

                var result = new ResultViewModel<List<NotificationTypeReadModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Total = count,
                    PageSize = query.PageSize,
                    PageNumber = query.PageNumber,
                    Filter = query.Filter,
                    Data = _mapper.Map<List<NotificationTypeReadModel>>(list)
                };

                _cacheService.Set(cacheKey, result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<List<NotificationTypeReadModel>>
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

        public async Task<ResultViewModel<NotificationTypeModifyModel>> Find(Expression<Func<NotificationType, bool>> predicate)
        {
            var cacheKey = $"NotificationType:Find_{predicate.Body.GetHashCode()}".GetHashCode().ToString();
            try
            {
                var cached = _cacheService.Get<ResultViewModel<NotificationTypeModifyModel>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }

                var data = await _context.NotificationTypes.SingleOrDefaultAsync(predicate);
                var result = new ResultViewModel<NotificationTypeModifyModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Data = _mapper.Map<NotificationTypeModifyModel>(data)
                };

                _cacheService.Set(cacheKey, result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<NotificationTypeModifyModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_LOAD_ERROR },
                };
            }
        }
        public async Task<ResultViewModel<NotificationTypeModifyModel>> Find(int id)
        {
            var cacheKey = $"NotificationType:Find_{id}".GetHashCode().ToString();
            try
            {

                var cached = _cacheService.Get<ResultViewModel<NotificationTypeModifyModel>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }

                var data = await _context.NotificationTypes.FindAsync(id);
                var result = new ResultViewModel<NotificationTypeModifyModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Data = _mapper.Map<NotificationTypeModifyModel>(data)
                };

                _cacheService.Set($"NotificationType:Find_{id}", result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<NotificationTypeModifyModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_LOAD_ERROR },
                };
            }
        }


        public async Task<AcknowledgeViewModel> Add(NotificationTypeModifyModel dto)
        {
            try
            {
                var record = new NotificationType(dto);
                var entity = await _context.NotificationTypes.AddAsync(record);
                _ = await _context.SaveChangesAsync();

                _cacheService.Clear("NotificationType:");
                return new ()
                {

                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_SAVE_SUCCESS },
                };

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_SAVE_ERROR },
                };
            }
        }

        public async Task<AcknowledgeViewModel> Update(NotificationTypeModifyModel dto)
        {
            try
            {
                var record = await _context.NotificationTypes.FindAsync(dto.Id);
                if (record == null)
                {
                    var exp = new KeyNotFoundException($"item number {dto.Id} does not Exist");
                    _logger.LogError(exp.Message, exp);
                    return new ()
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Messages = { ResourcesKeys.ITEM_NOT_FOUND },
                    };
                }


                var entity = record.Update(dto);

                _ = await _context.SaveChangesAsync();

                _cacheService.Clear("NotificationType:");

                return new ()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_MODIFY_SUCCESS }
                };

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_SAVE_ERROR },
                };
            }
        }

        public async Task<AcknowledgeViewModel> Delete(int id)
        {
            try
            {
                var record = await _context.NotificationTypes.FindAsync(id);
                if (record == null)
                {
                    var exp = new KeyNotFoundException($"item number {id} does not Exist");
                    _logger.LogError(exp.Message, exp);
                    return new ()
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Messages = { ResourcesKeys.ITEM_NOT_FOUND },
                    };
                }
                var entity = record.Delete();
                _context.Entry(entity).State = EntityState.Deleted;
                _ = await _context.SaveChangesAsync();

                _cacheService.Clear("NotificationType:");

                return new ()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_REMOVE_SUCCESS }
                };

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_REMOVE_ERROR },
                };
            }
        }


    }
}