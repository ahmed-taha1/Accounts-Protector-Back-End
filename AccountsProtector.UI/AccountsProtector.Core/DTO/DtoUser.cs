using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoRegisterRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "first name is required")]
        [MaxLength(50, ErrorMessage = "first name Should be less than 50 char")]
        public string? FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Last Key Should be less than 50 char")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "confirm password is required")]
        [Compare(nameof(Password), ErrorMessage = "passwords do not match")]
        public string? ConfirmPassword { get; set; }

        [Phone(ErrorMessage = "invalid phone number")]
        public string? PhoneNumber { get; set; }
    }
    
    public class DtoLoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
    }

    public class DtoLoginResponse
    {
        public string? Token { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public DateTime? Expiration { get; set; }
    }

    public class DtoChangePasswordRequest
    {
        [Required(ErrorMessage = "Old password is required")]
        public string? OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required")]
        public string? NewPassword { get; set; }
        [Compare(nameof(NewPassword), ErrorMessage = "passwords do not match")]
        public string? NewPasswordRepeat { get; set; }
    }

    public class DtoRemoveAccountRequest
    {
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
    }

    public class DtoGetUserPersonalDataResponse
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class DtoGetAllUserDataResponse : DtoGetUserPersonalDataResponse
    {
        public List<DtoPlatformWithAccounts>? Platforms { get; set; }
    }
}