using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.AccountsProtector.Core.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        public virtual List<Platform>? Platforms { get; set; }
    }
}