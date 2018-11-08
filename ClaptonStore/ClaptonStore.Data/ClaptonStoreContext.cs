namespace ClaptonStore.Data
{
    using Configuration;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Identity;

    public class ClaptonStoreContext : IdentityDbContext<ApplicationUser>
    {
        public ClaptonStoreContext()
        {
        }

        public ClaptonStoreContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookOrder> BookOrders { get; set; }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<Director> Directors { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameOrder> GameOrders { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieOrder> MovieOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GameOrderConfiguration());
            builder.ApplyConfiguration(new BookOrderConfiguration());
            builder.ApplyConfiguration(new MovieOrderConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
