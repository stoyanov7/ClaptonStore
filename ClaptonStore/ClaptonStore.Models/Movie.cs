namespace ClaptonStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enum;

    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public MovieGenreType MovieGenreType { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int DirectorId { get; set; }

        public Director Director { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ThumbnailUrl { get; set; }

        public ICollection<MovieOrder> MovieOrders { get; set; } = new HashSet<MovieOrder>();
    }
}