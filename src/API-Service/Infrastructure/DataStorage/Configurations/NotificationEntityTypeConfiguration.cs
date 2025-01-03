using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API_Service.Core.Domain;

namespace API_Service.Infrastructure.DataStorage.Configurations
{
    public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(e => e.Status)
                .HasConversion<string>();

            //builder.Property(e => e.ContentType)
            //    .HasConversion<string>();

            builder.Property(e => e.ChannelType)
                .HasConversion<string>();
        }
    }
}
