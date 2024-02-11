using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsProtector.AccountsProtector.Core.Domain.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? AccountName { get; set; }
        [Required]
        [ForeignKey(nameof(Platform))]
        public int PlatformId { get; set; }
        public virtual Platform? Platform { get; set; }
        public virtual List<AccountAttribute>? AccountAttributes { get; set; }
    }
}
