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
        public string Day { get; set; }
        public string Time { get; set; }
        public string Subject { get; set; }
        public string Class { get; set; }
        public string Teacher { get; set; }
        public string Email { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser IdentityUser { get; set; }
    }
}
