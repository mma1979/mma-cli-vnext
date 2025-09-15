namespace MmaSolution.AppApi.Hubs
{
    public class DatabaseNotificationsHub:Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public DatabaseNotificationsHub(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task ReadMessages()
        {
            
            while (true)
            {
                var userId = Guid.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value);
                var messages = _context.PushNotifications
                    .Where(e=>e.NotificationStatus == NotificationStatuses.Pending && e.IsRead==false && 
                    e.UserId == userId)
                    .ToList();
                if (messages.Any())
                {
                    await Clients.All.SendAsync("LoadData", messages);
                }
            }

        }
    }
}
