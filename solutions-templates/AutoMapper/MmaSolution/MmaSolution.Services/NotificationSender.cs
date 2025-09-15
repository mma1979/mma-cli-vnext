namespace MmaSolution.Services
{
    public class NotificationSender
    {
        private readonly ApplicationDbContext _context;

        public NotificationSender(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AcknowledgeViewModel> EmailNotification(EmailNotification notification)
        {
            try
            {
               _ = await _context.EmailNotifications.AddAsync(notification);
                _ = await _context.SaveChangesAsync();

                return AcknowledgeViewModel.Success();

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, notification);
                return AcknowledgeViewModel.Error();
            }
        }

        public async Task<AcknowledgeViewModel> SmsNotification(SmsNotification notification)
        {
            try
            {
                _ = await _context.SmsNotifications.AddAsync(notification);
                _ = await _context.SaveChangesAsync();

                return AcknowledgeViewModel.Success();

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, notification);
                return AcknowledgeViewModel.Error();
            }
        }

        public async Task<AcknowledgeViewModel> PushNotification(PushNotification notification)
        {
            try
            {
                _ = await _context.PushNotifications.AddAsync(notification);
                _ = await _context.SaveChangesAsync();

                return AcknowledgeViewModel.Success();

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, notification);
                return AcknowledgeViewModel.Error();
            }
        }
    }
}
