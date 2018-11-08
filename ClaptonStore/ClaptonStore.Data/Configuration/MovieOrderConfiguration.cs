namespace ClaptonStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class MovieOrderConfiguration : IEntityTypeConfiguration<MovieOrder>
    {
        public void Configure(EntityTypeBuilder<MovieOrder> builder)
        {
            builder
                .HasKey(mo => new { mo.UserId, mo.MovieId });

            builder
                .HasOne(u => u.User)
                .WithMany(mo => mo.MovieOrders)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(m => m.Movie)
                .WithMany(mo => mo.MovieOrders)
                .HasForeignKey(m => m.MovieId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}