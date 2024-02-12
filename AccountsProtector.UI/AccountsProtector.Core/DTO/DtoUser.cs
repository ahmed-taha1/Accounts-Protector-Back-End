﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AccountsProtector.AccountsProtector.Core.DTO
{
    public class DtoRegisterUser
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "invalid email form")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name Should be less than 50 char")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "confirm password is required")]
        [Compare(nameof(Password), ErrorMessage = "passwords do not match")]
        public string? ConfirmPassword { get; set; }
        [Phone(ErrorMessage = "invalid phone number")]
        public string? PhoneNumber { get; set; }
    }
    
    public class DtoUserLoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "invalid email")]
        public string? Email { get; set; }
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
        [Compare(nameof(NewPassword), ErrorMessage = "passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}