using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthECAPI.Models
{
    public class LibraryManagement
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(450)")]
        public string UserId { get; set; } // The user requesting the card

        [ForeignKey("UserId")]
        public IdentityUser IdentityUser { get; set; }

        public int? LibraryCardId { get; set; } // Assigned after approval

        [ForeignKey("LibraryCardId")]
        public LibraryCard LibraryCard { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovalDate { get; set; }
        public DateTime? RejectionDate { get; set; }
    }
}
