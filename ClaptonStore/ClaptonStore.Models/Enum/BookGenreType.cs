namespace ClaptonStore.Models.Enum
{
    using System.ComponentModel.DataAnnotations;

    public enum BookGenreType
    {
        [Display(Name = "Science Fiction")]
        ScienceFiction = 0,
        Satire = 1,
        Drama = 2,
        [Display(Name = "Action and adventure")]
        ActionAndAdventure = 3,
        Romance = 4,
        Mystery = 5,
        Horror = 6,
        [Display(Name = "Self help")]
        SelfHelp = 7,
        Health = 8,
        Guide = 9,
        Travel = 10,
        Childrens = 11,
        Religion = 12,
        Science = 13,
        History = 14,
        Math = 15,
        Anthology = 16,
        Poetry = 17,
        Encyclopedias = 18,
        Dictionaries = 19,
        Comics = 20,
        Art = 21,
        Cookbooks = 22,
        Diaries = 23,
        Journals = 24,
        [Display(Name = "Prayer books")]
        PrayerBooks = 25,
        Series = 26,
        Trilogy = 27,
        Biographies = 28,
        Autobiographies = 29,
        Fantasy = 30
    }
}