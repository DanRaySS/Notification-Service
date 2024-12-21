using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_Service.Core.Domain;

namespace Notification_Service.Infrastructure.DataStorage.Configurations
{
    public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(e => e.Status)
                .HasConversion<string>();

            builder.Property(e => e.ContentType)
                .HasConversion<string>();

            builder.Property(e => e.ChannelType)
                .HasConversion<string>();
        }
    }
}
