using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthECAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string BookTitle { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Genre { get; set; } // e.g., Tech, History, etc.

        [Required]
        public bool IsBorrowed { get; set; } // Indicates if the book is borrowed

        [Column(TypeName = "nvarchar(450)")]
        public string? BorrowedByUserId { get; set; } // User ID of the borrower (nullable if not borrowed)

        [Column(TypeName = "nvarchar(150)")]
        public string? BorrowedByEmail { get; set; } // Navigation property to the user who borrowed the book

        [ForeignKey("BorrowedByUserId")]
        public IdentityUser IdentityUser { get; set; }
    }
}
