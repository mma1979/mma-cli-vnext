namespace MmaSolution.Services
{


    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationService> _logger;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public NotificationService(ApplicationDbContext context, ILogger<NotificationService> logger, ICacheService cacheService, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _cacheService = cacheService;
            _mapper = mapper;
        }



        public async Task<ResultViewModel<List<NotificationReadModel>>> All(QueryViewModel query)
        {
            var cacheKey = $"Notification:GetAll_{query.GetHashCode()}".GetHashCode().ToString();
            try
            {
                var cached = _cacheService.Get<ResultViewModel<List<NotificationReadModel>>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }


                var data = query.ShowAll ?
                _context.Notifications.IgnoreQueryFilters().AsQueryable() :
                _context.Notifications.AsQueryable();
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

                var result = new ResultViewModel<List<NotificationReadModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Total = count,
                    PageSize = query.PageSize,
                    PageNumber = query.PageNumber,
                    Filter = query.Filter,
                    Data = _mapper.Map<List<NotificationReadModel>>(list)
                };

                _cacheService.Set(cacheKey, result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<List<NotificationReadModel>>
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

        public async Task<ResultViewModel<NotificationModifyModel>> Find(Expression<Func<Notification, bool>> predicate)
        {
            var cacheKey = $"Notification:Find_{predicate.Body}".GetHashCode().ToString();
            try
            {
                var cached = _cacheService.Get<ResultViewModel<NotificationModifyModel>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }

                var data = await _context.Notifications.SingleOrDefaultAsync(predicate);
                var result = new ResultViewModel<NotificationModifyModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Data = _mapper.Map<NotificationModifyModel>(data)
                };

                _cacheService.Set(cacheKey, result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<NotificationModifyModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_LOAD_ERROR },
                };
            }
        }
        public async Task<ResultViewModel<NotificationModifyModel>> Find(Guid id)
        {
            var cacheKey = $"Notification:Find_{id}".GetHashCode().ToString();
            try
            {

                var cached = _cacheService.Get<ResultViewModel<NotificationModifyModel>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }

                var data = await _context.Notifications.FindAsync(id);
                var result = new ResultViewModel<NotificationModifyModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Data = _mapper.Map<NotificationModifyModel>(data)
                };

                _cacheService.Set(cacheKey, result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<NotificationModifyModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_LOAD_ERROR },
                };
            }
        }


        public async Task<AcknowledgeViewModel> Add(NotificationModifyModel model)
        {
            try
            {
                var tag = new Notification(model);
                var entity = await _context.Notifications.AddAsync(tag);
                _ = await _context.SaveChangesAsync();

                _cacheService.Clear("Notification:");
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
                return new()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_SAVE_ERROR },
                };
            }
        }

        public async Task<AcknowledgeViewModel> Update(NotificationModifyModel model)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(model.Id);
                if (notification == null)
                {
                    var exp = new KeyNotFoundException($"item number {model.Id} does not Exist");
                    _logger.LogError(exp.Message, exp);
                    return new ()
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Messages = { ResourcesKeys.ITEM_NOT_FOUND },
                    };
                }


                var entity = notification.Update(model);

                _ = await _context.SaveChangesAsync();

                _cacheService.Clear("Notification:");

                return new ()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_MODIFY_SUCCESS },
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

        public async Task<AcknowledgeViewModel> Delete(Guid id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
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
                var entity = notification.Delete();
                _context.Entry(entity).State = EntityState.Deleted;
                _ = await _context.SaveChangesAsync();

                _cacheService.Clear("Notification:");

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