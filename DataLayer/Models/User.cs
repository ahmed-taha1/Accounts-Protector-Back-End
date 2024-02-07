using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Models
{
    public class User : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string? PersonName { get; set; }
        public virtual List<Platform>? Platforms { get; set; }
    }
}
