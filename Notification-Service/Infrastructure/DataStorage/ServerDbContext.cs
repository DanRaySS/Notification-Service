using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.SharedKernel.Storage;

namespace Notification_Service.Infrastructure.DataStorage
{
    public class ServerDbContext : DbContext, IUnitOfWork
    {
        public ServerDbContext(DbContextOptions<ServerDbContext> contextOptions) : base(contextOptions)
        {

        }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServerDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //var mediator = this.GetService<IMediator>();
            //await this.DispatchDomainEventsAsync(mediator);
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
