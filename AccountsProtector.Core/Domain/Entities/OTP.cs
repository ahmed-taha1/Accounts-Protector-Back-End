using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AccountsProtector.Core.Domain.Entities
{
    public class OTP
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserEmail { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddMinutes(5);
        [Required]
        public int OTPCode { get; set; } = new Random().Next(100000, 999999);
    }
}