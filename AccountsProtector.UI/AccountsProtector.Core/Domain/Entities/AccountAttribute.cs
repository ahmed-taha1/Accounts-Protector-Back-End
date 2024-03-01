using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsProtector.AccountsProtector.Core.Domain.Entities
{
    public class AccountAttribute
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string? Key { get; set; }
        [Required]
        [StringLength(500)]
        public string? Value { get; set; }
        [Required]
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }
    }
}