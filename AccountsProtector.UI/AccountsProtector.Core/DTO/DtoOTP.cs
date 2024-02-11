using System.ComponentModel.DataAnnotations;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoSendOTPRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class DtoForgetPasswordRequest
    {
        [Required]
        public int OTPCode { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
