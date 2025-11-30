using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthECAPI.Models
{
    public class LibraryCard
    {
        [Key]
        public int LibraryId { get; set; }

        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public IdentityUser IdentityUser { get; set; }

    }
}
