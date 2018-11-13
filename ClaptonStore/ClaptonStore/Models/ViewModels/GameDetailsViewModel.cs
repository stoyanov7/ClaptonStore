namespace ClaptonStore.Models.ViewModels
{
    using System;

    public class GameDetailsViewModel
    {
        public string Title { get; set; }

        public string Developer { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public double Size { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}