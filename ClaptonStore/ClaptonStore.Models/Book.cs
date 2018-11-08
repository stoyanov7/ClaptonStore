namespace ClaptonStore.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enum;

    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ThumbnailUrl { get; set; }

        public int PageCount { get; set; }

        public BookGenreType BookGenre { get; set; }

        public ICollection<BookOrder> BookOrders { get; set; } = new HashSet<BookOrder>();
    }
}