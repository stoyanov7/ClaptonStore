namespace ClaptonStore.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class BookOrderConfiguration : IEntityTypeConfiguration<BookOrder>
    {
        public void Configure(EntityTypeBuilder<BookOrder> builder)
        {
            builder
                .HasKey(bo => new { bo.UserId, bo.BookId });

            builder
                .HasOne(u => u.User)
                .WithMany(bo => bo.BookOrders)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.Book)
                .WithMany(bo => bo.BookOrders)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}