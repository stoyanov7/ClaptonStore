namespace ClaptonStore.Models.Enum
{
    using System.ComponentModel.DataAnnotations;

    public enum MovieGenreType
    {
        Action = 0,
        Adventure = 1,
        Animation = 2,
        Biography = 3,
        Comedy = 4,
        Crime = 5,
        Documentary = 6,
        Drama = 7,
        Familly = 8,
        Fantasy = 9,
        History = 10,
        Horror = 11,
        Musical = 12,
        Mystery = 13,
        Romance = 14,
        [Display(Name = "Sci-fi")]
        SciFi = 15,
        Superhero = 16,
        Thriller = 17,
        War = 18,
        Western = 19
    }
}