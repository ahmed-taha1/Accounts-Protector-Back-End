using System.ComponentModel.DataAnnotations;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoSetPinRequest
    {
        [Required(ErrorMessage = "pin is required")]
        public string? Pin { get; set; }
        [Required(ErrorMessage = "pin confirmation is required")]
        [Compare(nameof(Pin), ErrorMessage = "Pins doesn't match")]
        public string? PinConfirmation { get; set; }
    }

    public class DtoPin
    {
        public string? Pin { get; set; }
    }

    public class DtoMsgResponse
    {
        public string? Message { get; set; }
    }
}
