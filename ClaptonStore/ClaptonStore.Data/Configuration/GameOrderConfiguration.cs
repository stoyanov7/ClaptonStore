namespace ClaptonStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class GameOrderConfiguration : IEntityTypeConfiguration<GameOrder>
    {
        public void Configure(EntityTypeBuilder<GameOrder> builder)
        {
            builder
                .HasKey(go => new { go.UserId, go.GameId, });

            builder
                .HasOne(u => u.User)
                .WithMany(go => go.GameOrders)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(g => g.Game)
                .WithMany(go => go.GameOrders)
                .HasForeignKey(g => g.GameId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}