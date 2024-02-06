using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class AccountAttribute
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Value { get; set; }
        [Required]
        public int AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public virtual Account? Account { get; set; }
    }
}