using System.ComponentModel.DataAnnotations;

namespace AccountsProtector.AccountsProtector.Core.Domain.Entities
{
    public class OTP
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string? UserEmail { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddMinutes(5);
        [Required]
        [MaxLength(10)]
        public int OTPCode { get; set; } = new Random().Next(100000, 999999);
    }
}