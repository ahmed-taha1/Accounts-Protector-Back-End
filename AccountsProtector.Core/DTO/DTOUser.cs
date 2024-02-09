using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.Core.DTO
{
    public class DtoRegisterUser
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "invalid email")]
        [Remote(action: "IsEmailIsAlreadyRegistered", controller: "UserController", ErrorMessage = "Email is already registered before")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name Should be less than 100 char")]
        public string? PersonName { get; set; }

        [DataType(DataType.Password, ErrorMessage = "invalid password")]
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "invalid phone number")]
        public string? PhoneNumber { get; set; }
    }
    
    public class DtoUserLoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "invalid email")]
        public string? Email { get; set; }
        [DataType(DataType.Password, ErrorMessage = "invalid password")]
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
    }

    public class DtoUserLoginResponse
    {
        public string? Token { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PersonName { get; set; } = string.Empty;
        public DateTime? Expiration { get; set; }
    }

    public class DtoUserChangePassword
    {
        [Required(ErrorMessage = "Old password is required")]
        public string? OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required")]
        public string? NewPassword { get; set; }
    }
}