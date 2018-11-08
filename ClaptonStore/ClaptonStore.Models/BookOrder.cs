namespace ClaptonStore.Models
{
    using Identity;

    public class BookOrder
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}