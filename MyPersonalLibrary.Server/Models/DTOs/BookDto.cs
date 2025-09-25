namespace MyPersonalLibrary.Server.Models.DTOs
{
    public class BookDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Authors { get; set; } 
        public double? OriginalPublicationYear { get; set; }
        public string? LanguageCode { get; set; }
        public double? AverageRating { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}
