using Bot.Persistence.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bot.Persistence.EntityFrameWork.Configurations
{

    /// <summary>
    /// This class contains the configurations for the <see cref="Server"/> table. 
    /// </summary>
    public class ServersConfiguration : IEntityTypeConfiguration<Server>
    {


        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.ToTable("Servers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("bigint").HasColumnName("Id").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(255)").HasColumnName("Name").IsRequired();
            builder.Property(x => x.JoinDate).HasColumnType("date").HasColumnName("JoinDate").IsRequired();
            builder.Property(x => x.Active).HasColumnType("bit").HasColumnName("Active").IsRequired();
            builder.Property(x => x.TotalMembers).HasColumnType("int").HasColumnName("TotalMembers").IsRequired();
            builder.Property(x => x.Prefix).HasColumnType("nvarchar(255)").HasColumnName("Prefix").IsRequired(false);
        }
    }
}
