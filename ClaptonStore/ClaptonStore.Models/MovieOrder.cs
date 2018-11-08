namespace ClaptonStore.Models
{
    using Identity;

    public class MovieOrder
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}