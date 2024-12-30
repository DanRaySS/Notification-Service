using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.Repositories;

namespace Notification_Service.Infrastructure.DataStorage.Repositories
{
    public sealed class NotificationRepository(ServerDbContext context) : EFRepository<Notification, ServerDbContext>(context), INotificationRepository
    {

    }
}

