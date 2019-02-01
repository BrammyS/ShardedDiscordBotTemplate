using Bot.Persistence.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bot.Persistence.EntityFrameWork.Configurations
{

    /// <summary>
    /// This class contains the configurations for the <see cref="User"/> table. 
    /// </summary>
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {


        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnType("DECIMAL(20, 0)").HasColumnName("Id").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(MAX)").HasColumnName("Name").IsRequired();
            builder.Property(x => x.SpamWarning).HasColumnType("int").HasColumnName("SpamWarning").IsRequired();
            builder.Property(x => x.TotalTimesTimedOut).HasColumnType("int").HasColumnName("TimesTimedOut").IsRequired();
            builder.Property(x => x.CommandUsed).HasColumnType("datetime").HasColumnName("CommandUsed").IsRequired();
            builder.Property(x => x.CommandSpam).HasColumnType("int").HasColumnName("CommandSpam").IsRequired();
        }
    }
}
