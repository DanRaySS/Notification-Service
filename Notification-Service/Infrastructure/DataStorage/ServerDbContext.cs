using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Notification_Service.Core.Domain;
using Notification_Service.Core.Domain.SharedKernel;
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var mediator = this.GetService<IMediator>();
            await this.DispatchDomainEventsAsync(mediator);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    public static class ServerDbContextExtensions
    {
        public static async Task DispatchDomainEventsAsync(this ServerDbContext dbContext, IMediator mediator)
        {
            var domainEntities = dbContext.ChangeTracker.Entries<Entity>().Where(x => x.Entity.DomainEvents?.Any() ?? false).ToList();
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(async domainEvent => await mediator.Publish(domainEvent, CancellationToken.None));
            await Task.WhenAll(tasks);
        }
    }
}
