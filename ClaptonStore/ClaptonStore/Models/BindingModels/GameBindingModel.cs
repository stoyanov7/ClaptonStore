﻿namespace ClaptonStore.Models.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Enum;

    public class GameBindingModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public decimal Price { get; set; }

        public double Size { get; set; }

        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public GameGenreType GameType { get; set; }

        public string Developer { get; set; }
    }
}