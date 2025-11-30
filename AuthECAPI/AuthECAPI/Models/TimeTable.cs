using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthECAPI.Models
{
    public class TimeTable
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Day { get; set; }

        public string Time { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Subject { get; set; }

        public string Class { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Teacher { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser IdentityUser { get; set; }
    }
}
