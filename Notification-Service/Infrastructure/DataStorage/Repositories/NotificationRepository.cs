using Microsoft.EntityFrameworkCore;
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

        public async Task<IReadOnlyList<Notification>> SearchNotificationByStatusAndContentType(string status, string contentType, CancellationToken cancellationToken)
        {
            return await Items.Where(x => EF.Functions.Like(x.Status, $"%{status}%") && x.ContentType == contentType).ToListAsync(cancellationToken);
        } 
    }
}
