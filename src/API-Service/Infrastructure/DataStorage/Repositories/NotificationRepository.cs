using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using API_Service.Core.Domain;
using API_Service.Core.Domain.Repositories;

namespace API_Service.Infrastructure.DataStorage.Repositories
{
    public sealed class NotificationRepository(ServerDbContext context) : EFRepository<Notification, ServerDbContext>(context), INotificationRepository
    {

    }
}

