using Microsoft.EntityFrameworkCore;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.SharedKernel.Storage;

namespace Notification_Service.Infrastructure.DataStorage
{
    public class ServerDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Notification> Notifications { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
