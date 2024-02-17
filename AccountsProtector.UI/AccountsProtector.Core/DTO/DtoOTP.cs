using System.ComponentModel.DataAnnotations;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoSendOTPRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class DtoVerifyOTPRequest
    {
        [Required]
        public int OTPCode { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    public class DtoVerifyOTPResponse
    {
        public string? Token { get; set; }
    }

    public class DtoResetPasswordRequest
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "passwords do not match")]
        public string NewPasswordRepeat { get; set; }
    }
}
