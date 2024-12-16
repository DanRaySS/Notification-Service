using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.Repositories;

namespace Notification_Service.Infrastructure.DataStorage.Repositories
{
    public class NotificationRepository : EFRepository<Notification, ServerDbContext>, INotificationRepository
    {
       private readonly ServerDbContext _context;
        public NotificationRepository(ServerDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
