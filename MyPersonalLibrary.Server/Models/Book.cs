namespace MyPersonalLibrary.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema; 
                                                       
    [Table("book")]
    public class Book
    {
        [Key]
        [Column("book_id")]
        public int? Id { get; set; }

        [Column("authors")]
        public string? Authors { get; set; }

        [Column("original_publication_year")]
        public double? OriginalPublicationYear { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("language_code")]
        public string? LanguageCode { get; set; }

        [Column("average_rating")]
        public double? AverageRating { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("description")]
        public string? Description { get; set; }
    }
}
