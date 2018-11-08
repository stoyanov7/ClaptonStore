namespace ClaptonStore.Models.Identity
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ICollection<GameOrder> GameOrders { get; set; } = new HashSet<GameOrder>();

        public ICollection<BookOrder> BookOrders { get; set; } = new HashSet<BookOrder>();

        public ICollection<MovieOrder> MovieOrders { get; set; } = new HashSet<MovieOrder>();
    }
}