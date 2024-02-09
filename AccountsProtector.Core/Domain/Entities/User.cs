using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.Core.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string? PersonName { get; set; }
        public virtual List<Platform>? Platforms { get; set; }
    }
}
