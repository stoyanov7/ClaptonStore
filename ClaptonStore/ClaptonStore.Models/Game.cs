namespace ClaptonStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enum;

    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // In GB
        [Range(0, double.MaxValue)]
        public double Size { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ThumbnailUrl { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public GameGenreType GameGenreType { get; set; }

        public ICollection<GameOrder> GameOrders { get; set; } = new HashSet<GameOrder>();
    }
}
