using System.ComponentModel.DataAnnotations;

namespace UserApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The author is required.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "The genre is required.")]
        public Genre Genre { get; set; }

        [Required(ErrorMessage = "The publication date is required.")]
        public DateOnly PublicationDate { get; set; }

        public string? ImagePath { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Number of copies must be at least 1.")]
        public int NumberOfCopies { get; set; }

        public int NumberOfLoans { get; set; }
    }
}
