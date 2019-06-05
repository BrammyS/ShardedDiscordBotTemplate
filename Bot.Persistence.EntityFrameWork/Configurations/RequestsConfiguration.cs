using Bot.Persistence.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bot.Persistence.EntityFrameWork.Configurations
{

    /// <summary>
    /// This class contains the configurations for the <see cref="Request"/> table. 
    /// </summary>
    public class RequestsConfiguration : IEntityTypeConfiguration<Request>
    {


        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Requests");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("bigint").HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.ServerId).HasColumnType("DECIMAL(20, 0)").HasColumnName("ServerId");
            builder.Property(x => x.UserId).HasColumnType("DECIMAL(20, 0)").HasColumnName("UserId").IsRequired();
            builder.Property(x => x.Command).HasColumnType("nvarchar(MAX)").HasColumnName("Command").IsRequired();
            builder.Property(x => x.ErrorMessage).HasColumnType("nvarchar(MAX)").HasColumnName("ErrorMessage").IsRequired(false);
            builder.Property(x => x.IsSuccessFull).HasColumnType("bit").HasColumnName("IsSuccessFull").IsRequired();
            builder.Property(x => x.RunTime).HasColumnType("bigint").HasColumnName("RunTime").IsRequired();
            builder.Property(x => x.TimeStamp).HasColumnType("datetime").HasColumnName("TimeStamp").IsRequired();
        }
    }
}
