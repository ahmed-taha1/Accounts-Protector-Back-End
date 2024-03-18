using System.ComponentModel.DataAnnotations;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoSetPinRequest
    {
        [Required(ErrorMessage = "pin is required")]
        public string? PinHash { get; set; }
        [Required(ErrorMessage = "pin confirmation is required")]
        [Compare(nameof(PinHash), ErrorMessage = "Pins doesn't match")]
        public string? PinHashConfirmation { get; set; }
    }

    public class DtoPin
    {
        public string? PinHash { get; set; }
    }

    public class DtoMsgResponse
    {
        public string? Message { get; set; }
    }
}
