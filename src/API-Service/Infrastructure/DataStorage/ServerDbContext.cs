using Microsoft.EntityFrameworkCore;
using API_Service.Core.Domain;
using API_Service.Core.Domain.SharedKernel.Storage;
using MassTransit;

namespace API_Service.Infrastructure.DataStorage
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
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServerDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
