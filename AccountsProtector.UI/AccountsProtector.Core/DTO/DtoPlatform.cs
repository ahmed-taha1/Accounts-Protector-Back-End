using System.ComponentModel.DataAnnotations;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoAddPlatformRequest
    {
        [Required(ErrorMessage = "platform name is required")]
        public string? PlatformName { get; set; }
        [Required(ErrorMessage = "icon color is required")]
        public string? IconColor { get; set; }
    }
}