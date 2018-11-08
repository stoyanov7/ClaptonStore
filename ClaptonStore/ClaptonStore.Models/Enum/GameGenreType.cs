namespace ClaptonStore.Models.Enum
{
    using System.ComponentModel.DataAnnotations;

    public enum GameGenreType
    {
        Action = 0,
        [Display(Name = "Action-adventure")]
        ActionAdventure = 1,
        Adventure = 2,
        [Display(Name = "Role-playing")]
        RolePlaying = 3,
        Simulation = 4,
        Strategy = 5,
        Sports = 6,
        Sandbox = 7,
        [Display(Name = "First-Person Shooter")]
        FirstPersonShooter = 8,
        [Display(Name = "Open World")]
        OpenWorld = 9,
        Racing = 10,
        War = 11
    }
}