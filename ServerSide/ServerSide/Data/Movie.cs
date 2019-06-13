using System;
using System.ComponentModel.DataAnnotations;

namespace ServerSide.Data
{
    public class Movie
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Enter a Title"), StringLength(100, ErrorMessage = "That name is too long")]
        public string Title { get; set; }

        [Required, Range(1920, 2020, ErrorMessage = "Year must be from 1920 to 2020")]
        public int Year { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public string Description { get; set; }
        public Movie()
        {
            Id = Guid.NewGuid();
        }
    }
}
