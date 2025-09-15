namespace MmaSolution.Services
{


    public class NotificationStatusService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationStatusService> _logger;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public NotificationStatusService(ApplicationDbContext context, ILogger<NotificationStatusService> logger, ICacheService cacheService, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _cacheService = cacheService;
            _mapper = mapper;
        }



        public async Task<ResultViewModel<List<NotificationStatusReadModel>>> All(QueryViewModel query)
        {
            var cacheKey = $"NotificationStatus:GetAll_{query.GetHashCode()}".GetHashCode().ToString();
            try
            {
                var cached = _cacheService.Get<ResultViewModel<List<NotificationStatusReadModel>>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }


                var data = query.ShowAll ?
                _context.NotificationStatuses.IgnoreQueryFilters().AsQueryable():
                _context.NotificationStatuses.AsQueryable();
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

                var result = new ResultViewModel<List<NotificationStatusReadModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Total = count,
                    PageSize = query.PageSize,
                    PageNumber = query.PageNumber,
                    Filter = query.Filter,
                    Data = list.Adapt<List<NotificationStatusReadModel>>()
                };

                _cacheService.Set(cacheKey, result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<List<NotificationStatusReadModel>>
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

        public async Task<ResultViewModel<NotificationStatusModifyModel>> Find(Expression<Func<NotificationStatus, bool>> predicate)
        {
            var cacheKey = $"NotificationStatus:Find_{predicate.Body.GetHashCode()}".GetHashCode().ToString();
            try
            {
                var cached = _cacheService.Get<ResultViewModel<NotificationStatusModifyModel>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }

                var data = await _context.NotificationStatuses.SingleOrDefaultAsync(predicate);
                var result = new ResultViewModel<NotificationStatusModifyModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Data = data.Adapt<NotificationStatusModifyModel>()
                };

                _cacheService.Set(cacheKey, result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<NotificationStatusModifyModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_LOAD_ERROR },
                };
            }
        }
        public async Task<ResultViewModel<NotificationStatusModifyModel>> Find(int id)
        {
            var cacheKey = $"NotificationStatus:Find_{id}".GetHashCode().ToString();
            try
            {

                var cached = _cacheService.Get<ResultViewModel<NotificationStatusModifyModel>>(cacheKey);

                if (cached != null)
                {
                    return cached;
                }

                var data = await _context.NotificationStatuses.FindAsync(id);
                var result = new ResultViewModel<NotificationStatusModifyModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_LOAD_SUCCESS },
                    Data = data.Adapt<NotificationStatusModifyModel>()
                };

                _cacheService.Set(cacheKey, result);

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
                return new ResultViewModel<NotificationStatusModifyModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Messages = { ResourcesKeys.DATA_LOAD_ERROR },
                };
            }
        }


        public async Task<AcknowledgeViewModel> Add(NotificationStatusModifyModel model)
        {
            try
            {
                var record = new NotificationStatus(model);
                var entity = await _context.NotificationStatuses.AddAsync(record);
                _ = await _context.SaveChangesAsync();

                _cacheService.Clear("NotificationStatus:");
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

        public async Task<AcknowledgeViewModel> Update(NotificationStatusModifyModel model)
        {
            try
            {
                var record = await _context.NotificationStatuses.FindAsync(model.Id);
                if (record == null)
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


                var entity = record.Update(model);

                _ = await _context.SaveChangesAsync();

                _cacheService.Clear("NotificationStatus:");

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

        public async Task<AcknowledgeViewModel> Delete(int id)
        {
            try
            {
                var record = await _context.NotificationStatuses.FindAsync(id);
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

                _cacheService.Clear("NotificationStatus:");

                return new ()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Messages = { ResourcesKeys.DATA_REMOVE_SUCCESS },
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