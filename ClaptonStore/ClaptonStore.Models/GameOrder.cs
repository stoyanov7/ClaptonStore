namespace ClaptonStore.Models
{
    using Identity;

    public class GameOrder
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}