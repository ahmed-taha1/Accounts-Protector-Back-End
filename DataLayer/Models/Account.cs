using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? AccountName { get; set; }
        [Required]
        public int PlatformId { get; set; }
        public virtual Platform? Platform { get; set; }
        [ForeignKey(nameof(PlatformId))]
        public virtual List<AccountAttribute>? AccountAttributes { get; set; }
    }
}
