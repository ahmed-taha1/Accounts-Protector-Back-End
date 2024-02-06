using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? PlatformName { get; set; }
        [Required]
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}